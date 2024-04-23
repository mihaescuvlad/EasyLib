using Application.Data;
using Application.Repositories.Interfaces;

namespace Application.Repositories;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly LibraryContext _libraryContext;
    private IBookRepository? _bookRepository;
    private IAuthorRepository? _authorRepository;

    public IBookRepository BookRepository
    {
        get { return _bookRepository ??= new BookRepository(_libraryContext); }
    }

    public IAuthorRepository AuthorRepository
    {
        get { return _authorRepository ??= new AuthorRepository(_libraryContext); }
    }

    public RepositoryWrapper(LibraryContext libraryContext)
    {
        _libraryContext = libraryContext;
    }

    public void Save()
    {
        _libraryContext.SaveChanges();
    }
}
