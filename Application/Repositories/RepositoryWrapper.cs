using Application.Data;
using Application.Repositories.Interfaces;

namespace Application.Repositories;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly LibraryContext _libraryContext;
    private IBookRepository? _bookRepository;
    private IAuthorRepository? _authorRepository;
    private ILibraryLocationRepository? _libraryLocationRepository;
    private IBorrowHistoryRepository? _borrowHistoryRepository;
    private IUserRepository? _userRepository;
    private IAddressRepository? _addressRepository;

    public IBookRepository BookRepository
    {
        get { return _bookRepository ??= new BookRepository(_libraryContext); }
    }

    public IUserRepository UserRepository
    {
        get { return _userRepository ??= new UserRepository(_libraryContext); }
    }

    public IAuthorRepository AuthorRepository
    {
        get { return _authorRepository ??= new AuthorRepository(_libraryContext); }
    }

    public ILibraryLocationRepository LibraryLocationRepository
    {
        get { return _libraryLocationRepository ??= new LibraryLocationRepository(_libraryContext); }
    }

    public IBorrowHistoryRepository BorrowHistoryRepository
    {
        get { return _borrowHistoryRepository ??= new BorrowHistoryRepository(_libraryContext); }
    }

    public IAddressRepository AddressRepository
    {
        get { return _addressRepository ??= new AddressRepository(_libraryContext); }
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
