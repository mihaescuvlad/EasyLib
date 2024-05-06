using Application.Models;

namespace Application.Services.Interfaces;

public interface IAuthorService
{
    IEnumerable<Author> GetAllAuthors();
}
