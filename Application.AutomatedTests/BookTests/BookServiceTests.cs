using Application.Models;
using Application.Pocos;
using Application.Repositories;
using Application.Repositories.Interfaces;
using Application.Services;
using Moq;

using Newtonsoft.Json;

namespace Application.AutomatedTests.BookTests;

[TestClass]
public class BookServiceTests
{
    private Mock<IRepositoryWrapper> _mockRepo = null!;
    private BookService _bookService = null!;
    private List<BookPoco> _bookPocos = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<IRepositoryWrapper>();
        _bookService = new BookService(_mockRepo.Object);
        _bookPocos = new List<BookPoco>
        {
            new BookPoco
            {
                Isbn = "0007149212",
                Title = "The Fellowship of the Ring",
                Authors = new string[] { "John Ronald Reuel Tolkien" },
                Description = "The first book in the Lord of the rings trilogy.",
            },
            new BookPoco
            {
                Isbn = "9780596004194",
                Title = "Practical C++ Programming",
                Authors = new string[] { "Steve Oualline" },
                Description =
                    "Teaches the programming language, covering topics including syntax, coding standards, object classes, templates, debugging, and the C++ preprocessor.",
            },
            new BookPoco
            {
                Isbn = "9780321635372",
                Title = "Elements of Programming",
                Authors = new[] { "Paul McJones", "Alexander A. Stepanov" },
                Description =
                    "A truly foundational book on the discipline of generic programming reveals how to write better software by mastering the development of abstract components. The authors show programmers how to use mathematics to compose reliable algorithms from components, and to design effective interfaces between algorithms and data structures.",
            },
        };
    }

    [TestMethod]
    public void GetBooksPreviewByPage_ReturnsCorrectPageSize()
    {
        _mockRepo.Setup(x => x.BookRepository.GetBooksWithAuthors()).Returns(_bookPocos);

        var result = _bookService.GetBooksPreviewByPage(1, 1);

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("The Fellowship of the Ring", result.First().Title);
    }

    [TestMethod]
    public void GetBook_ReturnsCorrectBook()
    {
        _mockRepo.Setup(x => x.BookRepository.GetBookWithAuthorsByIsbn("0007149212")).Returns(_bookPocos.First());

        var result = _bookService.GetBook("0007149212");

        Assert.IsNotNull(result);
        Assert.AreEqual("The Fellowship of the Ring", result.Title);
    }

    [TestMethod]
    public void IsInStock_BookInStock_ReturnsTrue()
    {
        _mockRepo.Setup(x => x.BookRepository.IsInStock("0007149212")).Returns(true);

        var result = _bookService.IsInStock("0007149212");

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void DeleteBook_BookExists_DeletesBook()
    {
        _mockRepo.Setup(x => x.BookRepository.GetBookWithAuthorsByIsbn("0007149212")).Returns(_bookPocos.First());
        _mockRepo.Setup(x => x.BookRepository.DeleteBook("0007149212"));

        _bookService.DeleteBook("0007149212");

        _mockRepo.Verify(x => x.BookRepository.DeleteBook("0007149212"), Times.Once);
    }

    [TestMethod]
    public void SubmitEditBookBookData_BookExists_UpdatesBook()
    {
        var bookData = new SubmitEditBookPoco
        {
            BookData = new BookPoco
            {
                Isbn = "9780321635372",
                Title = "Elements of Programming 2",
                Authors = new[] { "[\"Paul McJones\",\"Alexander A. Stepanov\",\"Gigel Parcangiu\"]" },
                Description = "A better description tbf...",
            },
            LibraryStocks = new Dictionary<string, int> { { "1183c44f-12d1-4c31-ab96-ea2179ab755e", 5 } },
        };

        _mockRepo.Setup(x => x.BookRepository.GetBookWithAuthorsByIsbn(bookData.BookData.Isbn))
            .Returns(_bookPocos[2]);

        _bookService.SubmitEditBookBookData(bookData);

        _mockRepo.Verify(x => x.BookRepository.SubmitEditBookBookData(It.IsAny<SubmitEditBookPoco>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException), "Book not found.")]
    public void SubmitEditBookBookData_BookDoesNotExist_ThrowsException()
    {
        var nonExistingBookData = new SubmitEditBookPoco
        {
            BookData = new BookPoco
            {
                Isbn = "1111111111111",
                Title = "Elements of Programming",
                Authors = new[] { "[\"Paul McJones\",\"Alexander A. Stepanov\"]" },
                Description = "A truly foundational book on the discipline of generic programming reveals how to write better software by mastering the development of abstract components. The authors show programmers how to use mathematics to compose reliable algorithms from components, and to design effective interfaces between algorithms and data structures.",
            },
            LibraryStocks = new Dictionary<string, int> { { "1183c44f-12d1-4c31-ab96-ea2179ab755e", 5 } },
        };

        _mockRepo.Setup(x => x.BookRepository.GetBookWithAuthorsByIsbn(nonExistingBookData.BookData.Isbn))
            .Returns(default(BookPoco?));

        _mockRepo.Setup(x => x.BookRepository.SubmitEditBookBookData(It.IsAny<SubmitEditBookPoco>()))
            .Callback(() => throw new InvalidOperationException("Book not found."));

        _bookService.SubmitEditBookBookData(nonExistingBookData);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException), "Failed to parse book details.")]
    public void SubmitEditBookBookData_InvalidJson_ThrowsException()
    {
        var bookData = new SubmitEditBookPoco
        {
            BookData = new BookPoco
            {
                Isbn = "9780321635372",
                Title = "Elements of Programming",
                Authors = new[] { "Paul McJones", "Alexander A. Stepanov" },
                Description =
                    "A truly foundational book on the discipline of generic programming reveals how to write better software by mastering the development of abstract components. The authors show programmers how to use mathematics to compose reliable algorithms from components, and to design effective interfaces between algorithms and data structures.",
            },
            LibraryStocks = new Dictionary<string, int> { { "1183c44f-12d1-4c31-ab96-ea2179ab755e", 5 } },
        };

        _bookService.SubmitEditBookBookData(bookData);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException), "Failed to parse book details.")]
    public void AddBook_InvalidJson_ThrowsException()
    {
        var bookData = new SubmitEditBookPoco
        {
            BookData = new BookPoco
            {
                Isbn = "9780321635372",
                Title = "Elements of Programming",
                Authors = new[] { "Paul McJones", "Alexander A. Stepanov" },
                Description =
                    "A truly foundational book on the discipline of generic programming reveals how to write better software by mastering the development of abstract components. The authors show programmers how to use mathematics to compose reliable algorithms from components, and to design effective interfaces between algorithms and data structures.",
            },
            LibraryStocks = new Dictionary<string, int> { { "1183c44f-12d1-4c31-ab96-ea2179ab755e", 5 } },
        };

        _bookService.AddBook(bookData);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException), "A book with ISBN 9780131103627 already exists.")]
    public void AddBook_ExistingISBN_ThrowsException()
    {
        var existingBookData = new SubmitEditBookPoco
        {
            BookData = new BookPoco
            {
                Isbn = "9780131103627",
                Title = "Clean Code: A Handbook of Agile Software Craftsmanship",
                Authors = new[] { "[\"Robert C. Martin\"]" },
                Description = "This book is a must-read for all software developers.",
            },
            LibraryStocks = new Dictionary<string, int> { { "1183c44f-12d1-4c31-ab96-ea2179ab755e", 10 } },
        };

        _mockRepo.Setup(x => x.BookRepository.AddBook(existingBookData))
                 .Throws(new InvalidOperationException($"A book with ISBN {existingBookData.BookData.Isbn} already exists."));

        _bookService.AddBook(existingBookData);
    }

    [TestMethod]
    public void AddBook_BookDoesNotExist_AddsNewBook()
    {
        var newBookData = new SubmitEditBookPoco
        {
            BookData = new BookPoco
            {
                Isbn = "0787305820",
                Title = "The Inventions, Researches and Writings of Nikola Tesla",
                Authors = new[] { "[\"Thomas Commerford Martin\"]" },
                Description = "1894 with special reference to his work in polyphase currents and high potential lighting. Contents: Ployphase Currents; Biographical & Introductory; a New System of Alternating Current Motors & Transformers; Tesla Rotating Magnetic Field; Modifica.",
            },
            LibraryStocks = new Dictionary<string, int> { { "1183c44f-12d1-4c31-ab96-ea2179ab755e", 5 } },
        };

        _mockRepo.Setup(x => x.BookRepository.GetBookWithAuthorsByIsbn(newBookData.BookData.Isbn))
         .Returns(default(BookPoco));

        _bookService.AddBook(newBookData);

        _mockRepo.Verify(x => x.BookRepository.AddBook(It.Is<SubmitEditBookPoco>(b => b.BookData.Isbn == newBookData.BookData.Isbn)), Times.Once);
    }
}