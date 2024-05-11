using Application.Models;
using Application.Pocos;

namespace Application.Repositories.Interfaces;

public interface IUserRepository : IRepositoryBase<ApplicationUser>
{
    UserPoco? GetUserById(Guid Id);

    IEnumerable<UserPoco> GetUsersWithAddress();

    void UpdateUserById(UserPoco userPoco);
}