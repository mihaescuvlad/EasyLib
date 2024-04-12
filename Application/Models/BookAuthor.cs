using Microsoft.EntityFrameworkCore;

namespace Application.Models;

[PrimaryKey(nameof(BookIsbn), nameof(AuthorId))]
public class BookAuthor
{
    public required string BookIsbn { get; set; }
    public required Guid AuthorId { get; set; }
}
