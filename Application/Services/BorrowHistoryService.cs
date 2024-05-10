using Application.Pocos;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;

namespace Application.Services;

public class BorrowHistoryService : IBorrowHistoryService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public BorrowHistoryService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public IEnumerable<HistoryPoco> GetHistoryForUser(Guid userId)
    {
        return _repositoryWrapper.BorrowHistoryRepository.GetHistoryForUser(userId);
    }

    public void BorrowBook(BorrowHistoryPoco borrowHistory)
    {
        _repositoryWrapper.BorrowHistoryRepository.BorrowBook(borrowHistory);
    }
}
