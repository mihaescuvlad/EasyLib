namespace Application.Pocos;

public class BorrowHistoryPoco
{
    public required Guid UserId { get; set; }
    public required string BookIsbn { get; set; }
    public required Guid LibraryId { get; set; }
    public required DateTime BorrowDate { get; set; }
}
