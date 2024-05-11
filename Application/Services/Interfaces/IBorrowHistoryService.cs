using Application.Models;
using Application.Pocos;

namespace Application.Services.Interfaces;

public interface IBorrowHistoryService
{
    IEnumerable<HistoryPoco> GetHistoryForUser(Guid userId);
    void BorrowBook(BorrowHistoryPoco borrowHistory);
}
