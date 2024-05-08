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

    public void SubmitEditBookBookData(SubmitEditBookPoco newBookData)
    {
        UpdateBookAuthors(newBookData.BookData.Isbn, newBookData.BookData.Authors);
        UpdateBookStocks(newBookData.BookData.Isbn, newBookData.LibraryStocks);

        var existingBook = LibraryContext.Books.FirstOrDefault(b => b.Isbn == newBookData.BookData.Isbn);

        if (existingBook != null)
        {
            existingBook.Title = newBookData.BookData.Title;
            existingBook.Description = newBookData.BookData.Description;

            LibraryContext.SaveChanges();
        }
        else
        {
            throw new InvalidOperationException("Book not found.");
        }
    }

    private void UpdateBookStocks(string isbn, Dictionary<string, int> stockDictionary)
    {
        foreach ((string libraryIdString, int stock) in stockDictionary)
        {
            var libraryId = Guid.Parse(libraryIdString);

            var existingBookStock = LibraryContext.Set<BookStock>()
                .FirstOrDefault(bs => bs.BookIsbn == isbn && bs.LibraryId == libraryId);

            if (existingBookStock != null)
            {
                existingBookStock.Stock = stock;
            }
            else
            {
                var newBookStock = new BookStock
                {
                    BookIsbn = isbn,
                    LibraryId = libraryId,
                    Stock = stock,
                };

                LibraryContext.Set<BookStock>().Add(newBookStock);
            }
        }

        LibraryContext.SaveChanges();
    }

    private void UpdateBookAuthors(string isbn, IEnumerable<string> authors)
    {
        var authorIds = UpdateAuthors(authors);
        LinkAuthorsToBook(isbn, authorIds);
    }

    private IEnumerable<Guid> UpdateAuthors(IEnumerable<string> authors)
    {
        var authorIds = new List<Guid>();

        foreach (var authorName in authors)
        {
            var author = LibraryContext.Authors.FirstOrDefault(a => a.AuthorName == authorName);

            if (author == null)
            {
                author = new Author
                {
                    Id = Guid.NewGuid(),
                    AuthorName = authorName,
                };

                LibraryContext.Authors.Add(author);
                LibraryContext.SaveChanges();
            }

            authorIds.Add(author.Id);
        }

        return authorIds;
    }

    private void LinkAuthorsToBook(string isbn, IEnumerable<Guid> authorIds)
    {
        var existingBookAuthors = LibraryContext.BookAuthors.Where(ba => ba.BookIsbn == isbn).ToList();

        foreach (var existingAuthor in existingBookAuthors)
        {
            if (!authorIds.Contains(existingAuthor.AuthorId))
            {
                LibraryContext.BookAuthors.Remove(existingAuthor);
            }
        }

        LibraryContext.SaveChanges();

        authorIds = authorIds.Except(existingBookAuthors.Select(ba => ba.AuthorId)).ToList();

        foreach (var authorId in authorIds)
        {
            var bookAuthor = new BookAuthor
            {
                BookIsbn = isbn,
                AuthorId = authorId,
            };

            LibraryContext.BookAuthors.Add(bookAuthor);
        }

        LibraryContext.SaveChanges();
    }
}