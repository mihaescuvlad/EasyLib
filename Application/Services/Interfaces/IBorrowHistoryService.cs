using Application.Models;
using Application.Pocos;

namespace Application.Services.Interfaces;

public interface IBorrowHistoryService
{
    void GetBorrowBook(BorrowHistoryPoco borrowHistory);
}
