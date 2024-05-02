using Application.Models;
using Application.Pocos;

namespace Application.Services.Interfaces
{
    public interface ILibraryLocationService
    {
        List<LibraryLocationWithAddressPoco> GetLibraryLocationsWithAddress(string isbn);
    }
}
