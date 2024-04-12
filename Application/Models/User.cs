namespace Application.Models;

public class User
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Guid AddressId { get; set; }
    public required string PostalCode { get; set; }
    public bool Blacklisted { get; set; }
    public Guid RoleId { get; set; }
}