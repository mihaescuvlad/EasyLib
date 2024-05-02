using Application.Repositories.Interfaces;
using Application.Services;
using Application.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

public class BorrowHistoryController : Controller
{
    private readonly IBorrowHistorySerivce _borrowHistoryService;
    public BorrowHistoryController(IBorrowHistoryService borrowHistoryService)
    {
        _borrowHistoryService = borrowHistoryService;
    }

    public IActionResult Index()
    {
        return View();
    }
}
