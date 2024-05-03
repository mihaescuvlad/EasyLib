using Application.Data;
using Application.Models;
using Application.Pocos;
using Application.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Application.Repositories;

public class LibraryLocationRepository : RepositoryBase<LibraryLocation>, ILibraryLocationRepository
{
    public LibraryLocationRepository(LibraryContext context)
        : base(context)
    {
    }

    public IEnumerable<LibraryLocationWithAddressPoco> GetLibraryLocationWithAddress(string isbn)
    {
        return LibraryContext.LibraryLocations
                 .Join(
                     LibraryContext.Addresses,
                     location => location.AddressId,
                     address => address.Id,
                     (location, address) => new { Location = location, Address = address })
                 .Join(
                     LibraryContext.BookStocks.Where(bs => bs.BookIsbn == isbn && bs.Stock > 0),
                     combined => combined.Location.Id,
                     bookStock => bookStock.LibraryId,
                     (combined, bookStock) => new LibraryLocationWithAddressPoco
                     {
                         Id = combined.Location.Id,
                         Address1 = combined.Address.Address1,
                         Address2 = combined.Address.Address2,
                         OpenTime = combined.Location.OpenTime,
                         CloseTime = combined.Location.CloseTime,
                     })
                 .ToList();
    }
}