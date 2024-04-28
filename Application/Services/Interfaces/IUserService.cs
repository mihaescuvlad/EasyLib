using Application.Models;
using Application.Pocos;

namespace Application.Services.Interfaces;

public interface IUserService
{
    UserPoco? GetUser(Guid Id);
}
