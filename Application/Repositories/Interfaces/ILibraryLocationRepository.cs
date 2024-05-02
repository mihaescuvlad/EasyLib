using Application.Models;
using Application.Pocos;

namespace Application.Repositories.Interfaces;

public interface ILibraryLocationRepository : IRepositoryBase<LibraryLocation>
{
    public IEnumerable<LibraryLocationWithAddressPoco> GetLibraryLocationWithAddress(string isbn);
}