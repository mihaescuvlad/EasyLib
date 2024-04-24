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
}