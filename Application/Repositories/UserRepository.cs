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

    public IEnumerable<UserPoco> GetUsersWithAddress()
    {
        var usersWithAddress = _context.Users
            .Join(
                _context.Addresses,
                user => user.AddressId,
                address => address.Id,
                (user, address) => new UserPoco
                {
                    Id = new Guid(user.Id),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PostalCode = user.PostalCode,
                    Blacklisted = user.Blacklisted,
                    Email = user.Email,
                    Address1 = address.Address1,
                    Address2 = address.Address2,
                })
            .ToList();

        return usersWithAddress;
    }

    public void UpdateUserById(UserPoco userPoco)
    {
        var existingUser = _context.Users.FirstOrDefault(u => u.Id == userPoco.Id.ToString());

        if (existingUser == null)
        {
            throw new InvalidOperationException("User not found");
        }

        existingUser.FirstName = userPoco.FirstName;
        existingUser.LastName = userPoco.LastName;
        existingUser.PostalCode = userPoco.PostalCode;
        existingUser.Blacklisted = userPoco.Blacklisted;
        existingUser.Email = userPoco.Email;

        var userAddress = _context.Addresses.FirstOrDefault(a => a.Id == existingUser.AddressId);
        if (userAddress == null)
        {
            throw new InvalidOperationException("Address not found");
        }

        userAddress.Address1 = userPoco.Address1;
        userAddress.Address2 = userPoco.Address2;

        _context.Addresses.Update(userAddress);
        _context.Users.Update(existingUser);
        _context.SaveChanges();
    }
}
