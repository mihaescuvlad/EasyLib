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
        return View();
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
        if (!ModelState.IsValid)
        {
            return View(newBookData);
        }

        // _bookService.AddBook(newBookData);
        return RedirectToAction("Index", "Book");
    }
}
