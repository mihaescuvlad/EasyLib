using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public class Login
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Email is required")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }
}
