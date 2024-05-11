using Application.Models;
using Application.Pocos;
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

    public List<LibraryLocationWithAddressPoco> GetLibraryLocationsWithAddress(string isbn)
    {
        return _repositoryWrapper.LibraryLocationRepository.GetLibraryLocationWithAddress(isbn).ToList();
    }
}
