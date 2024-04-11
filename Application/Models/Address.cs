using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public class Address
{
    public required Guid Id { get; set; }
    public required string Address_1 { get; set; }
    public string? Address_2 { get; set; }
}
