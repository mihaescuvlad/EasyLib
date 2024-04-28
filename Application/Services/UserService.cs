using Application.Pocos;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public UserService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public UserPoco? GetUser(Guid Id)
    {
        return _repositoryWrapper.UserRepository.GetUserById(Id);
    }
}