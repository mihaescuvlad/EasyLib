using Microsoft.EntityFrameworkCore;

namespace Application.Models;

[PrimaryKey(nameof(UserId), nameof(BookIsbn), nameof(BorrowDate))]
public class BorrowHistory
{
    public required Guid UserId { get; set; }
    public required string BookIsbn { get; set; }
    public required DateTime BorrowDate { get; set; }
}
