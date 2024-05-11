using Application.Models;
using Application.Pocos;

namespace Application.Repositories.Interfaces;

public interface IBookRepository : IRepositoryBase<Book>
{
    IEnumerable<BookPoco> GetBooksWithAuthors();
    BookPoco? GetBookWithAuthorsByIsbn(string isbn);

    bool IsInStock(string isbn);

    EditBookPoco? GetEditBookBookData(string isbn);

    void SubmitEditBookBookData(SubmitEditBookPoco newBookData);

    public void DeleteBook(string isbn);

    public EditBookPoco? GetAddBookBookData();

    public void AddBook(SubmitEditBookPoco newBookData);
}