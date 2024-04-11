namespace Application.Pocos;

public class RegisterPoco
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
    public required string PostalCode { get; set; }
    public required string Address_1 { get; set; }
    public string? Address_2 { get; set; }
}
