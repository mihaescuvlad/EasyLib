using Application.Models;
using Application.Pocos;

namespace Application.Repositories.Interfaces;

public interface IBorrowHistoryRepository : IRepositoryBase<BorrowHistory>
{
    void BorrowBook(BorrowHistoryPoco borrowHistory);
}