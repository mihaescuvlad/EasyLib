using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace Application.Models;

[PrimaryKey(nameof(BookIsbn), nameof(LibraryId))]
public class BookStock
{
    public required string BookIsbn { get; set; }
    public required Guid LibraryId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock must not be lower than 0")]
    public int Stock { get; set; }
}
