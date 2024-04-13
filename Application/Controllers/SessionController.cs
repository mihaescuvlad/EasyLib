using Application.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;
public class SessionController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<SessionController> _logger;

    public SessionController(SignInManager<ApplicationUser> signInManager, ILogger<SessionController> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        // RedirectToAction("Index", "Home");
        return LocalRedirect("/");
    }

    [HttpPost]
    public async Task<IActionResult> LogoutPost()
    {
        await _signInManager.SignOutAsync();

        // RedirectToAction("Index", "Home");
        return LocalRedirect("/");
    }
}

