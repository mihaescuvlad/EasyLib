using Application.Models;
using Application.Pocos;

namespace Application.Services.Interfaces;

public interface IBookService
{
    List<BookPoco> GetBooksPreviewByPage(int pageNumber, int pageSize = 15);

    BookPoco? GetBook(string isbn);

    List<BookPoco> SearchBooks(string query, int pageNumber = 1, int pageSize = 15);

    int GetTotalSearchResultsCount(string query);

    EditBookPoco? GetEditBookBookData(string isbn);
}
