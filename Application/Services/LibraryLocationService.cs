using Application.Models;
using Application.Repositories.Interfaces;
using Application.Services.Interfaces;

namespace Application.Services;

public class LibraryLocationService : ILibraryLocationService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public LibraryLocationService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public List<LibraryLocation> GetLocationsByType(string locationType)
    {
        return _repositoryWrapper.LibraryLocationRepository.FindAll().ToList();
    }
}
