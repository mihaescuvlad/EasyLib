namespace Application.Models;

public class BookAuthor
{
    public required string BookIsbn { get; set; }
    public required Guid AuthorId { get; set; }
}
