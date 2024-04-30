using Application.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

public class BookController : Controller
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    public IActionResult Index([FromQuery] string isbn)
    {
        var book = _bookService.GetBook(isbn);

        if (book == null)
        {
            RedirectToAction("Index", "Home");
        }

        return View(book);
    }
}
