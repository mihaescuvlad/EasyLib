using Microsoft.AspNetCore.Identity;

namespace Application.Models;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Guid AddressId { get; set; }
    public required string PostalCode { get; set; }
    public bool Blacklisted { get; set; }
}