namespace Application.Models;

public class BorrowHistory
{
    public required Guid UserId { get; set; }
    public required string BookIsbn { get; set; }
    public required DateTime BorrowDate { get; set; }
}
