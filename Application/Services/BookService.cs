using Application.Pocos;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;

namespace Application.Services;

public class BookService : IBookService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public BookService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public List<BookPoco> GetBooksPreviewByPage(int pageNumber, int pageSize = 15)
    {
        var itemsToSkip = (pageNumber - 1) * pageSize;

        var bookPocos = _repositoryWrapper.BookRepository.GetBooksWithAuthors();

        var booksPreview = bookPocos
            .Skip(itemsToSkip)
            .Take(pageSize)
            .ToList();

        return booksPreview;
    }

    public BookPoco? GetBook(string isbn)
    {
        return _repositoryWrapper.BookRepository.GetBookWithAuthorsByIsbn(isbn);
    }

    public List<BookPoco> SearchBooks(string query, int pageNumber = 1, int pageSize = 5)
    {
        throw new NotImplementedException();
    }

    public int GetTotalSearchResultsCount(string query)
    {
        throw new NotImplementedException();
    }
}
