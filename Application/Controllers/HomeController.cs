using System.Diagnostics;

using Application.Models;
using Application.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

public class HomeController : Controller
{
    private readonly IBookService _bookService;

    public HomeController(IBookService bookService)
    {
        _bookService = bookService;
    }

    public IActionResult Index([FromQuery] int page = 1, [FromQuery] string search = "")
    {
        const int pageSize = 15;

        if (User.Identity is { IsAuthenticated: true })
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchResults = _bookService.SearchBooks(search, page, pageSize);

                var totalResultsCount = _bookService.GetTotalSearchResultsCount(search);
                var totalPages = (int)Math.Ceiling((double)totalResultsCount / pageSize);
                var hasNextPage = page < totalPages;
                var hasPreviousPage = page > 1;

                ViewBag.SearchQuery = search;
                ViewBag.PageNumber = page;
                ViewBag.HasPreviousPage = hasPreviousPage;
                ViewBag.HasNextPage = hasNextPage;

                return View("UserIndex", searchResults);
            }
            else
            {
                var postPreviews = _bookService.GetBooksPreviewByPage(page, pageSize);

                ViewBag.PageNumber = page;
                ViewBag.HasPreviousPage = page > 1;
                ViewBag.HasNextPage = postPreviews.Count >= pageSize;

                return View("UserIndex", postPreviews);
            }
        }
        else
        {
            return View();
        }
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
