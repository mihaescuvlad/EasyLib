using Application.Models;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;

namespace Application.Services;

public class AuthorService : IAuthorService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public AuthorService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public IEnumerable<Author> GetAllAuthors()
    {
        return _repositoryWrapper.AuthorRepository.FindAll();
    }
}
