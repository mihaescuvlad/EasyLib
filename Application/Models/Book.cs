using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public class Book
{
    [Key]
    [RegularExpression(@"^\d{10,13}$", ErrorMessage = "Invalid ISBN format")]
    public required string Isbn { get; set; }

    public required string Title { get; set; }
    public required string Description { get; set; }
    public byte[]? CoverPicture { get; set; }
}
