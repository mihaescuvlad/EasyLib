using System.Linq.Expressions;

using Application.Data;
using Application.Models;
using Application.Pocos;
using Application.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public class BorrowHistoryRepository : RepositoryBase<BorrowHistory>, IBorrowHistoryRepository
{
    public BorrowHistoryRepository(LibraryContext context)
        : base(context)
    {
    }

    public void BorrowBook(BorrowHistoryPoco borrowHistory)
    {
        var libraryLocation = LibraryContext.LibraryLocations
            .Include(l => l.OpenTime)
            .Include(l => l.CloseTime)
            .FirstOrDefault(l => l.Id == borrowHistory.LibraryId);

        if (libraryLocation == null)
        {
            throw new ArgumentException("Invalid Library Id");
        }

        var borrowTime = borrowHistory.BorrowDate.TimeOfDay;
        if (borrowTime < libraryLocation.OpenTime.TimeOfDay || borrowTime > libraryLocation.CloseTime.TimeOfDay)
        {
            throw new InvalidOperationException("BorrowDate is not within library open hours");
        }

        var bookStock = LibraryContext.BookStocks
            .FirstOrDefault(bs => bs.BookIsbn == borrowHistory.BookIsbn && bs.LibraryId == borrowHistory.LibraryId);

        if (bookStock == null)
        {
            throw new ArgumentException("Book not found in stock");
        }

        if (bookStock.Stock < 1)
        {
            throw new InvalidOperationException("Book is out of stock");
        }

        bookStock.Stock--;

        var borrowHistoryRecord = new BorrowHistory
        {
            UserId = borrowHistory.UserId,
            BookIsbn = borrowHistory.BookIsbn,
            LibraryId = borrowHistory.LibraryId,
            BorrowDate = borrowHistory.BorrowDate,
        };

        LibraryContext.BorrowHistory.Add(borrowHistoryRecord);

        LibraryContext.SaveChanges();
    }
}