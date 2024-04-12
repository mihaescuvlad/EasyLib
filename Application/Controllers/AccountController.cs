using Application.Data;
using Application.Helpers;
using Application.Models;
using Application.Pocos;

using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

public class AccountController : Controller
{
    private readonly LibraryContext _context;

    public AccountController(LibraryContext context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        return View("Login");
    }

    public IActionResult Register()
    {
        return View("Register");
    }

    [HttpPost]
    public IActionResult Register(RegisterPoco registerPoco)
    {
        if (ModelState.IsValid)
        {
            var existingUser = _context.Logins.SingleOrDefault(u => u.Email == registerPoco.Email);
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

            registerPoco.Password = PasswordHasher.HashPassword(registerPoco.Password, out string salt);

            Guid userId = Guid.NewGuid();
            Login login = new()
            {
                Id = userId,
                Email = registerPoco.Email,
                Password = registerPoco.Password,
                Salt = salt,
            };

            Guid roleId = new("6A5E8047-BC28-435E-9A09-92AB3D47BBB0");
            User user = new()
            {
                Id = userId,
                FirstName = registerPoco.FirstName,
                LastName = registerPoco.LastName,
                AddressId = address.Id,
                PostalCode = registerPoco.PostalCode,
                Blacklisted = false,
                RoleId = roleId,
            };

            _context.Addresses.Add(address);
            _context.Logins.Add(login);
            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login", "Account");
        }

        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        return BadRequest(new { success = false, message = "Validation failed", errors });
    }

    [HttpPost]
    public IActionResult Login(LoginPoco loginPoco)
    {
        if (ModelState.IsValid)
        {
            var existingUser = _context.Logins.SingleOrDefault(u => u.Email == loginPoco.Email);

            if (existingUser != null)
            {
                if (PasswordHasher.VerifyPassword(loginPoco.Password, existingUser.Password, existingUser.Salt))
                {
                    HttpContext.Session.SetString("UserId", existingUser.Id.ToString());

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Wrong email or password.");
            return BadRequest(new { success = false, message = "Wrong email or password." });
        }

        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        return BadRequest(new { success = false, message = "Validation failed", errors });
    }
}
