namespace Application.Models;

public class Address
{
    public required Guid Id { get; set; }
    public required string Address1 { get; set; }
    public string? Address2 { get; set; }
}
