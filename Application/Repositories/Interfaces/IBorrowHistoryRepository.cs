using Application.Models;
using Application.Pocos;

namespace Application.Repositories.Interfaces;

public interface IBorrowHistoryRepository : IRepositoryBase<BorrowHistory>
{
    IEnumerable<HistoryPoco> GetHistoryForUser(Guid userId);
    void BorrowBook(BorrowHistoryPoco borrowHistory);
}