using Application.Models;
using Application.Pocos;
using Application.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[Authorize]
public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly IAuthorService _authorService;

    public BookController(IBookService bookService, IAuthorService authorService)
    {
        _bookService = bookService;
        _authorService = authorService;
    }

    public IActionResult Index([FromQuery] string isbn)
    {
        var book = _bookService.GetBook(isbn);

        if (book == null)
        {
            RedirectToAction("Index", "Home");
        }

        ViewBag.Isbn = isbn;

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
        return RedirectToAction("Index", "Home"); // Not implemented
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
}
