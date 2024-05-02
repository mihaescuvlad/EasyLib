using Application.Data;
using Application.Models;
using Application.Pocos;
using Application.Repositories.Interfaces;

namespace Application.Repositories;

public class LibraryLocationRepository : RepositoryBase<LibraryLocation>, ILibraryLocationRepository
{
    public LibraryLocationRepository(LibraryContext context)
        : base(context)
    {
    }
}