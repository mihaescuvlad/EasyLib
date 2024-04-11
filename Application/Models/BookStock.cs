using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public class BookStock
{
    public required string BookIsbn { get; set; }
    public required Guid LibraryId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock must not be lower than 0")]
    public int Stock { get; set; }
}
