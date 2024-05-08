using Application.Models;
using Application.Pocos;

namespace Application.Repositories.Interfaces;

public interface IBookRepository : IRepositoryBase<Book>
{
    IEnumerable<BookPoco> GetBooksWithAuthors();
    BookPoco? GetBookWithAuthorsByIsbn(string isbn);

    EditBookPoco? GetEditBookBookData(string isbn);

    void SubmitEditBookBookData(SubmitEditBookPoco newBookData);
}