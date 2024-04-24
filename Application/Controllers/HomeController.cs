using System.Diagnostics;

using Application.Models;
using Application.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

public class HomeController : Controller
{
    private readonly IBookService _bookService;

    public HomeController(IBookService bookService)
    {
        _bookService = bookService;
    }

    public IActionResult Index([FromQuery] int page = 1)
    {
        const int pageSize = 15;

        if (User.Identity is { IsAuthenticated: true })
        {
            var postPreviews = _bookService.GetBooksPreviewByPage(page, pageSize);

            ViewBag.PageNumber = page;
            ViewBag.HasPreviousPage = page > 1;
            ViewBag.HasNextPage = postPreviews.Count >= pageSize;

            return View("UserIndex", postPreviews);
        }
        else
        {
            return View();
        }
    }

    public IActionResult UserIndex([FromQuery] int page = 1)
    {
        const int pageSize = 15;
        var bookPreviews = _bookService.GetBooksPreviewByPage(page, pageSize);

        ViewBag.PageNumber = page;
        ViewBag.HasPreviousPage = page > 1;
        ViewBag.HasNextPage = bookPreviews.Count >= pageSize;

        return View(bookPreviews);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
