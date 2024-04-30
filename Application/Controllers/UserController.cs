using Application.Models;
using Application.Pocos;
using Application.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserService _userService;
    public UserController(UserManager<ApplicationUser> userManager, IUserService userService)
    {
        _userManager = userManager;
        _userService = userService;
    }

    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var userProfile = _userService.GetUser(Guid.Parse(user.Id));
        if (userProfile == null)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(userProfile);
    }

    public IActionResult EditProfile()
    {
        return View();
    }

    [Authorize(Roles = "librarian")]
    [HttpGet("EditProfile")]
    public IActionResult EditProfileById([FromQuery] Guid Id)
    {
        return View("EditProfile");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateProfile(UserPoco newProfileData)
    {
        return RedirectToAction("Index", "Home");
    }
}
