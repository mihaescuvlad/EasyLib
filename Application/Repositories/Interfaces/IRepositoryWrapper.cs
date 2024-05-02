namespace Application.Repositories.Interfaces;

public interface IRepositoryWrapper
{
    IBookRepository BookRepository { get; }
    IUserRepository UserRepository { get; }
    IAddressRepository AddressRepository { get; }
    void Save();
}