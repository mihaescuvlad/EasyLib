using Application.Data;
using Application.Models;
using Application.Pocos;
using Application.Repositories.Interfaces;

namespace Application.Repositories;

public class UserRepository : RepositoryBase<ApplicationUser>, IUserRepository
{
    private readonly LibraryContext _context;

    public UserRepository(LibraryContext context)
        : base(context)
    {
        _context = context;
    }

    public UserPoco? GetUserById(Guid id)
    {
        var user = _context.Users
            .Where(u => u.Id == id.ToString())
            .Join(
                _context.Addresses,
                user => user.AddressId,
                address => address.Id,
                (user, address) => new { User = user, Address = address })
            .Select(result => new UserPoco
            {
                Id = new Guid(result.User.Id),
                FirstName = result.User.FirstName,
                LastName = result.User.LastName,
                PostalCode = result.User.PostalCode,
                Blacklisted = result.User.Blacklisted,
                Email = result.User.Email,
                Address1 = result.Address.Address1,
                Address2 = result.Address.Address2,
            })
            .SingleOrDefault();

        return user;
    }
}
