using Application.Models;
using Application.Pocos;

namespace Application.Services.Interfaces;

public interface IUserService
{
    UserPoco? GetUser(Guid Id);
    IEnumerable<UserPoco> GetUsersByPage(int pageNumber, int pageSize = 15);
    void UpdateUser(UserPoco user);
}
