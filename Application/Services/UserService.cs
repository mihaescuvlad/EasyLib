using System.Drawing.Printing;

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

    public IEnumerable<UserPoco> GetUsersByPage(int pageNumber, int pageSize = 15)
    {
        var itemsToSkip = (pageNumber - 1) * pageSize;

        var users = _repositoryWrapper.UserRepository.GetUsersWithAddress();

        var filteredUsers = users
            .Skip(itemsToSkip)
            .Take(pageSize)
            .ToList();

        return filteredUsers;
    }

    public void UpdateUser(UserPoco user)
    {
        _repositoryWrapper.UserRepository.UpdateUserById(user);
    }
}