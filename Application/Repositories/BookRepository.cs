using Application.Data;
using Application.Models;
using Application.Pocos;
using Application.Repositories.Interfaces;

namespace Application.Repositories;

public class BookRepository : RepositoryBase<Book>, IBookRepository
{
    public BookRepository(LibraryContext context)
        : base(context)
    {
    }

    public IEnumerable<BookPoco> GetBooksWithAuthors()
    {
        return LibraryContext.Books
            .Join(
                LibraryContext.BookAuthors,
                book => book.Isbn,
                bookAuthor => bookAuthor.BookIsbn,
                (book, bookAuthor) => new { Book = book, BookAuthor = bookAuthor })
            .Join(
                LibraryContext.Authors,
                combined => combined.BookAuthor.AuthorId,
                author => author.Id,
                (combined, author) => new { combined.Book, Author = author })
            .GroupBy(combined => combined.Book, combined => combined.Author)
            .Select(group => new BookPoco
            {
                Isbn = group.Key.Isbn,
                Title = group.Key.Title,
                Description = group.Key.Description,
                Thumbnail = group.Key.Thumbnail,
                Authors = group.Select(a => a.AuthorName).ToArray(),
            })
            .ToList();
    }

    public BookPoco? GetBookWithAuthorsByIsbn(string isbn)
    {
        var book = LibraryContext.Books
            .FirstOrDefault(b => b.Isbn == isbn);

        if (book == null)
        {
            return null;
        }

        var authors = LibraryContext.BookAuthors
            .Where(ba => ba.BookIsbn == isbn)
            .Join(
                LibraryContext.Authors,
                ba => ba.AuthorId,
                author => author.Id,
                (ba, author) => author.AuthorName)
            .ToArray();

        var bookPoco = new BookPoco
        {
            Isbn = book.Isbn,
            Title = book.Title,
            Description = book.Description,
            Thumbnail = book.Thumbnail,
            Authors = authors,
        };

        return bookPoco;
    }

    public EditBookPoco? GetEditBookBookData(string isbn)
    {
        var bookData = GetBookWithAuthorsByIsbn(isbn);
        if (bookData == null)
        {
            return null;
        }

        var libraryStocks = LibraryContext.LibraryLocations
            .Join(
                LibraryContext.BookStocks.Where(bs => bs.BookIsbn == isbn),
                location => location.Id,
                bookStock => bookStock.LibraryId,
                (location, bookStock) => new { Location = location, BookStock = bookStock })
            .Join(
                LibraryContext.Addresses,
                combined => combined.Location.AddressId,
                address => address.Id,
                (combined, address) => new
                {
                    combined.Location,
                    combined.BookStock,
                    Address = address,
                })
            .Select(joinResult => new
            {
                LocationWithAddress = new LibraryLocationWithAddressPoco
                {
                    Id = joinResult.Location.Id,
                    Address1 = joinResult.Address.Address1,
                    Address2 = joinResult.Address.Address2,
                    OpenTime = joinResult.Location.OpenTime,
                    CloseTime = joinResult.Location.CloseTime,
                },
                BookStock = new BookStock
                {
                    BookIsbn = isbn,
                    LibraryId = joinResult.Location.Id,
                    Stock = joinResult.BookStock.Stock,
                },
            })
            .ToList();

        var libraryStocksMap = libraryStocks.ToDictionary(
            result => result.LocationWithAddress,
            result => result.BookStock.Stock);

        return new EditBookPoco
        {
            BookData = bookData,
            LibraryStocks = libraryStocksMap,
        };
    }
}