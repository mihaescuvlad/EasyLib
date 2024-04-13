using Application.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Index", "Home");
        }

        ApplicationUser userModel = new()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PostalCode = user.PostalCode,
        };

        return View(userModel);
    }
}
