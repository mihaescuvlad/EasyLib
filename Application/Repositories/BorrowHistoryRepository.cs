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

    public IEnumerable<HistoryPoco> GetHistoryForUser(Guid userId)
    {
        var history = LibraryContext.BorrowHistory
            .Where(bh => bh.UserId == userId)
            .Join(
                LibraryContext.LibraryLocations,
                bh => bh.LibraryId,
                ll => ll.Id,
                (bh, ll) => new { BorrowHistory = bh, LibraryLocation = ll })
            .Join(
                LibraryContext.Addresses,
                combined => combined.LibraryLocation.AddressId,
                a => a.Id,
                (combined, a) => new { combined.BorrowHistory, combined.LibraryLocation, Address = a })
            .Join(
                LibraryContext.Books,
                combined => combined.BorrowHistory.BookIsbn,
                b => b.Isbn,
                (combined, b) => new HistoryPoco
                {
                    BookIsbn = combined.BorrowHistory.BookIsbn,
                    BookTitle = b.Title,
                    Address1 = combined.Address.Address1,
                    Address2 = combined.Address.Address2,
                    BorrowDate = combined.BorrowHistory.BorrowDate,
                })
            .OrderByDescending(h => h.BorrowDate);

        return history;
    }

    public void BorrowBook(BorrowHistoryPoco borrowHistory)
    {
        var libraryLocation = LibraryContext.LibraryLocations
            .FirstOrDefault(l => l.Id == borrowHistory.LibraryId);

        if (libraryLocation == null)
        {
            throw new ArgumentException("Invalid Library Id");
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