﻿using Application.Pocos;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;

using Newtonsoft.Json;

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

    public List<BookPoco> SearchBooks(string query, int pageNumber = 1, int pageSize = 15)
    {
        var itemsToSkip = (pageNumber - 1) * pageSize;

        var bookPocos = _repositoryWrapper.BookRepository.GetBooksWithAuthors()
            .Where(book =>
                book.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                book.Description.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                book.Authors.Any(author => author.Contains(query, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        var searchResults = bookPocos
            .Skip(itemsToSkip)
            .Take(pageSize)
            .ToList();

        return searchResults;
    }

    public int GetTotalSearchResultsCount(string query)
    {
        var totalResultsCount = _repositoryWrapper.BookRepository.GetBooksWithAuthors()
            .Count(book =>
                book.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                book.Description.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                book.Authors.Any(author => author.Contains(query, StringComparison.OrdinalIgnoreCase)));

        return totalResultsCount;
    }

    public EditBookPoco? GetEditBookBookData(string isbn)
    {
        return _repositoryWrapper.BookRepository.GetEditBookBookData(isbn);
    }

    public void SubmitEditBookBookData(SubmitEditBookPoco newBookData)
    {
        newBookData.BookData.Authors = JsonConvert.DeserializeObject<string[]>(newBookData.BookData.Authors[0]) ?? throw new InvalidOperationException("Authors list can't be empty.");

        _repositoryWrapper.BookRepository.SubmitEditBookBookData(newBookData);
    }
}
