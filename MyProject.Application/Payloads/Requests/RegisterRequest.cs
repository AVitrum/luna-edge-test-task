using System.ComponentModel.DataAnnotations;

namespace MyProject.Application.Payloads.Requests;

public sealed class RegisterRequest
{
    [Required]
    [RegularExpression("^[a-zA-Z0-9_.-]*$",
        ErrorMessage = "Username can only contain letters, numbers, and the following characters: _ . -")]
    public required string Username { get; init; }

    [Required]
    [EmailAddress]
    public required string Email { get; init; }

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
    public required string Password { get; init; }
}