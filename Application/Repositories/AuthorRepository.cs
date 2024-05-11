using Application.Data;
using Application.Models;
using Application.Repositories.Interfaces;

namespace Application.Repositories;

public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
{
    public AuthorRepository(LibraryContext context)
        : base(context)
    {
    }
}