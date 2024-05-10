using Application.Models;
using Application.Pocos;
using Application.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace Application.Controllers;

[Authorize]
public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly IAuthorService _authorService;
    private readonly UserManager<ApplicationUser> _userManager;

    public BookController(IBookService bookService, IAuthorService authorService, UserManager<ApplicationUser> userManager)
    {
        _bookService = bookService;
        _authorService = authorService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index([FromQuery] string isbn)
    {
        var book = _bookService.GetBook(isbn);

        if (book == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Isbn = isbn;
        ViewBag.InStock = _bookService.IsInStock(isbn);
        ViewBag.IsBlacklisted = user.Blacklisted;

        return View(book);
    }

    [Authorize(Roles="librarian")]
    [HttpGet]
    public IActionResult Edit([FromQuery] string isbn)
    {
        var editFormData = _bookService.GetEditBookBookData(isbn);

        var authors = _authorService.GetAllAuthors();
        var authorNames = authors.Select(author => author.AuthorName).ToList();

        ViewBag.AuthorNames = authorNames;

        return View(editFormData);
    }

    [HttpPost]
    [Authorize(Roles = "librarian")]
    public IActionResult Edit(SubmitEditBookPoco editData)
    {
        if (ModelState.IsValid)
        {
            _bookService.SubmitEditBookBookData(editData);

            return RedirectToAction("Index", "Book", new { isbn = editData.BookData.Isbn });
        }

        return RedirectToAction("Index", "Home");
    }

    [Authorize(Roles = "librarian")]
    public IActionResult Delete([FromQuery] string isbn)
    {
        if (string.IsNullOrEmpty(isbn))
        {
            return RedirectToAction("Index", "Home");
        }

        var book = _bookService.GetBook(isbn);

        if (book == null)
        {
            return RedirectToAction("Index", "Home");
        }

        return View("Delete", book);
    }

    [HttpPost]
    [Authorize(Roles = "librarian")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(string isbn)
    {
        if (string.IsNullOrEmpty(isbn))
        {
            return RedirectToAction("Index", "Home");
        }

        _bookService.DeleteBook(isbn);

        return RedirectToAction("Index", "Home");
    }

    [Authorize(Roles = "librarian")]
    [HttpGet]
    public IActionResult Add()
    {
        var addFormData = _bookService.GetAddBookBookData();

        var authors = _authorService.GetAllAuthors();
        var authorNames = authors.Select(author => author.AuthorName).ToList();

        ViewBag.AuthorNames = authorNames;

        return View(addFormData);
    }

    [Authorize(Roles = "librarian")]
    [HttpPost]
    public IActionResult Add(SubmitEditBookPoco newBookData)
    {
        if (ModelState.IsValid)
        {
            _bookService.AddBook(newBookData);
            return RedirectToAction("Index", "Book", new { isbn = newBookData.BookData.Isbn });
        }

        return RedirectToAction("Index", "Home");
    }
}
