using System.Diagnostics;
using System.Security.Claims;

using Application.Controllers;
using Application.Models;
using Application.Pocos;
using Application.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Moq;

namespace Application.AutomatedTests.BookTests;

[TestClass]
public class BookControllerTests
{
    private Mock<IBookService> _mockBookService = null!;
    private Mock<IAuthorService> _mockAuthorService = null!;
    private UserManager<ApplicationUser> _userManager = null!;
    private BookController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockBookService = new Mock<IBookService>();
        _mockAuthorService = new Mock<IAuthorService>();

        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        var optionsMock = new Mock<IOptions<IdentityOptions>>();
        var passwordHasherMock = new Mock<IPasswordHasher<ApplicationUser>>();
        var userValidatorMock = new Mock<IUserValidator<ApplicationUser>>();
        var passwordValidatorMock = new Mock<IPasswordValidator<ApplicationUser>>();
        var keyNormalizerMock = new Mock<ILookupNormalizer>();
        var errorsMock = new Mock<IdentityErrorDescriber>();
        var servicesMock = new Mock<IServiceProvider>();
        var loggerMock = new Mock<ILogger<UserManager<ApplicationUser>>>();

        _userManager = new UserManager<ApplicationUser>(
            userStoreMock.Object,
            optionsMock.Object,
            passwordHasherMock.Object,
            new IUserValidator<ApplicationUser>[] { userValidatorMock.Object },
            new IPasswordValidator<ApplicationUser>[] { passwordValidatorMock.Object },
            keyNormalizerMock.Object,
            errorsMock.Object,
            servicesMock.Object,
            loggerMock.Object);

        _userManager.UserValidators.Add(new UserValidator<ApplicationUser>());
        _userManager.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());
        userStoreMock.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ApplicationUser
            {
                UserName = "jimmytester@protonmail.com",
                FirstName = "Jimmy",
                LastName = "Tester",
                PostalCode = "1355007",
                Blacklisted = false,
            });

        _controller = new BookController(_mockBookService.Object, _mockAuthorService.Object, _userManager);

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, "1") }));
        _controller.ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext { User = claimsPrincipal } };
    }

    [TestMethod]
    public async Task Index_BookExists_ReturnsView()
    {
        var bookPoco = new BookPoco
        {
            Isbn = "0007149212",
            Title = "The Fellowship of the Ring",
            Authors = new string[] { "John Ronald Reuel Tolkien" },
            Description = "The first book in the Lord of the rings trilogy.",
        };

        _mockBookService.Setup(s => s.GetBook("0007149212")).Returns(bookPoco);

        var result = await _controller.Index("0007149212") as ViewResult;

        Assert.IsNotNull(result);
        var model = result.Model as BookPoco;

        Assert.AreEqual("The Fellowship of the Ring", model?.Title);
    }

    [TestMethod]
    public async Task Index_BookDoesNotExist_RedirectsToHome()
    {
        _mockBookService.Setup(s => s.GetBook("0007149212")).Returns((BookPoco)null!);

        var result = await _controller.Index("0007149212") as RedirectToActionResult;

        Assert.IsNotNull(result);
        Assert.AreEqual("Home", result.ControllerName);
        Assert.AreEqual("Index", result.ActionName);
    }
}