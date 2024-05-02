using Application.Models;

namespace Application.Services.Interfaces
{
    public interface ILibraryLocationService
    {
        List<LibraryLocation> GetLocationsByType(string locationType);
    }
}
