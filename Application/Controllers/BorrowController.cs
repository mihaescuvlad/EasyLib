using Application.Models;
using Application.Pocos;
using Application.Services;
using Application.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[Authorize]
public class BorrowController : Controller
{
    private readonly IBorrowHistoryService _borrowHistoryService;
    private readonly ILibraryLocationService _libraryLocationService;
    private readonly UserManager<ApplicationUser> _userManager;

    public BorrowController(
        IBorrowHistoryService borrowHistoryService,
        ILibraryLocationService libraryLocationService,
        UserManager<ApplicationUser> userManager)
    {
        _borrowHistoryService = borrowHistoryService;
        _libraryLocationService = libraryLocationService;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Index([FromQuery] Guid userId)
    {
        Guid id = Guid.Empty;

        if (userId == Guid.Empty)
        {
            var currentUserId = _userManager.GetUserId(User);

            if (currentUserId == null)
            {
                return RedirectToAction("Profile", "User");
            }

            id = Guid.Parse(currentUserId);
        }
        else if (User.IsInRole("librarian"))
        {
            id = userId;
        }

        var userHistory = _borrowHistoryService.GetHistoryForUser(id).ToList();

        return View(userHistory);
    }

    [HttpGet]
    public IActionResult Borrow([FromQuery] string isbn)
    {
        var libraryLocations = _libraryLocationService.GetLibraryLocationsWithAddress(isbn);

        if (libraryLocations == null)
        {
            RedirectToAction("Index", "Home");
        }

        ViewBag.LibraryLocations = libraryLocations;
        ViewBag.Isbn = isbn;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> BorrowBook(BorrowHistoryPoco borrowHistory)
    {
        try
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Borrow", "Borrow", borrowHistory.BookIsbn);
            }

            borrowHistory.UserId = new Guid(currentUser.Id);

            _borrowHistoryService.BorrowBook(borrowHistory);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error borrowing book: {ex.Message}");
            return View("Index");
        }
    }
}
