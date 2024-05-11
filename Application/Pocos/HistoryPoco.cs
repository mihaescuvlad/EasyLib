namespace Application.Pocos;

public class HistoryPoco
{
    public required string BookIsbn { get; set; }
    public required string BookTitle { get; set; }
    public required string Address1 { get; set; }
    public string? Address2 { get; set; }
    public required DateTime BorrowDate { get; set; }
}
