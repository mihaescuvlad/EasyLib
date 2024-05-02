namespace Application.Repositories.Interfaces;

public interface IRepositoryWrapper
{
    IBookRepository BookRepository { get; }

    ILibraryLocationRepository LibraryLocationRepository { get; }
    IBorrowHistoryRepository BorrowHistoryRepository { get; }

    void Save();
}