namespace Application.Repositories.Interfaces;

public interface IRepositoryWrapper
{
    IBookRepository BookRepository { get; }

    void Save();
}