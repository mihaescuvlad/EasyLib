using Application.Data;
using Application.Models;
using Application.Repositories.Interfaces;

namespace Application.Repositories;

public class AddressRepository : RepositoryBase<Address>, IAddressRepository
{
    public AddressRepository(LibraryContext context)
        : base(context)
    {
    }
}