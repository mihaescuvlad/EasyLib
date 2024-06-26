﻿namespace Application.Repositories.Interfaces;

public interface IRepositoryWrapper
{
    IBookRepository BookRepository { get; }
    IUserRepository UserRepository { get; }
    IAuthorRepository AuthorRepository { get; }
    IAddressRepository AddressRepository { get; }

    ILibraryLocationRepository LibraryLocationRepository { get; }
    IBorrowHistoryRepository BorrowHistoryRepository { get; }

    void Save();
}