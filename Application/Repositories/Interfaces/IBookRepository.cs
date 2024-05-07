using Application.Models;
using Application.Pocos;

namespace Application.Repositories.Interfaces;

public interface IBookRepository : IRepositoryBase<Book>
{
    IEnumerable<BookPoco> GetBooksWithAuthors();
    public BookPoco? GetBookWithAuthorsByIsbn(string isbn);

    public EditBookPoco? GetEditBookBookData(string isbn);
    public EditBookPoco? GetAddBookBookData();
    public void AddBook(SubmitEditBookPoco newBookData);
}