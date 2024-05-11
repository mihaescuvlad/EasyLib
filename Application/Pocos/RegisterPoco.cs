using System.ComponentModel.DataAnnotations;

namespace Application.Pocos;

public class RegisterPoco
{
    [Required(ErrorMessage = "First Name is required")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [DataType(DataType.Password)]
    public required string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Postal Code is required")]
    public required string PostalCode { get; set; }

    [Required(ErrorMessage = "Address Line 1 is required")]
    public required string Address1 { get; set; }

    public string? Address2 { get; set; }
}
