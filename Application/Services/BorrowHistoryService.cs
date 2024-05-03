using Application.Models;
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

    public void BorrowBook(BorrowHistoryPoco borrowHistory)
    {
        _repositoryWrapper.BorrowHistoryRepository.BorrowBook(borrowHistory);
    }
}
