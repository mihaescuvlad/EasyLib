namespace Application.Pocos;
public class UserPoco
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PostalCode { get; set; }
    public bool Blacklisted { get; set; }
    public string? Email { get; set; }
    public required string Address1 { get; set; }
    public string? Address2 { get; set; }
}
