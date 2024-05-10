using Application.Models;
using Application.Pocos;
using Application.Services;
using Application.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

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

    [HttpGet]
    [Authorize(Roles="librarian")]
    public IActionResult Index([FromQuery] int page = 1)
    {
        const int pageSize = 15;

        var usersList = _userService.GetUsersByPage(page, pageSize).ToList();

        ViewBag.PageNumber = page;
        ViewBag.HasPreviousPage = page > 1;
        ViewBag.HasNextPage = usersList.Count >= pageSize;

        return View("Index", usersList);
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

    [HttpGet]
    public async Task<IActionResult> EditProfile([FromQuery] Guid userId)
    {
        Guid id = Guid.Empty;

        if (userId == Guid.Empty)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Profile", "User");
            }

            id = Guid.Parse(user.Id);
        }
        else if (User.IsInRole("librarian"))
        {
            id = userId;
        }

        var userProfile = _userService.GetUser(id);
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

            if (User.IsInRole("librarian") && userPoco.Id != Guid.Parse(_userManager.GetUserId(User) ?? string.Empty))
            {
                return RedirectToAction("Index", "User");
            }

            return RedirectToAction("Profile", "User");
        }

        return View("EditProfile");
    }
}
