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

    public async Task<IActionResult> EditProfile()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return RedirectToAction("Profile", "User");
        }

        var userProfile = _userService.GetUser(Guid.Parse(user.Id));
        if (userProfile == null)
        {
            return RedirectToAction("Profile", "User");
        }

        return View(userProfile);
    }

    [HttpPost]
    public IActionResult UpdateProfile(UserPoco userPoco)
    {
        if (ModelState.IsValid)
        {
            _userService.UpdateUser(userPoco);
            return RedirectToAction("Profile", "User");
        }

        return View("EditProfile");
    }
}
