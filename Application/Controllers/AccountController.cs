using Application.Data;
using Application.Models;
using Application.Pocos;
using Application.Services.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly LibraryContext _context;
    private readonly IAddressService _addressService;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, LibraryContext context, IAddressService addressService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _addressService = addressService;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterPoco registerPoco)
    {
        if (ModelState.IsValid)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == registerPoco.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "User with this email already exists.");
                return BadRequest(new { success = false, message = "User with this email already exists." });
            }

            Address address = new()
            {
                Id = Guid.NewGuid(),
                Address1 = registerPoco.Address1,
                Address2 = registerPoco.Address2,
            };

            // registerPoco.Password = PasswordHasher.HashPassword(registerPoco.Password, out string salt);
            ApplicationUser user = new()
            {
                FirstName = registerPoco.FirstName,
                LastName = registerPoco.LastName,
                UserName = registerPoco.Email,
                Email = registerPoco.Email,
                PostalCode = registerPoco.PostalCode,
                AddressId = address.Id,
            };

            _addressService.CreateAddress(address);

            var result = await _userManager.CreateAsync(user, registerPoco.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "standard");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(registerPoco);
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginPoco loginPoco)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(loginPoco.Email, loginPoco.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(loginPoco);
            }
        }

        return View(loginPoco);
    }
}
