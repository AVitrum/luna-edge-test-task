using System.ComponentModel.DataAnnotations;

namespace MyProject.Application.Payloads.Requests;

/// <summary>
/// Request payload for user registration.
/// </summary>
public sealed class RegisterRequest
{
    /// <summary>
    /// Desired username. Can contain letters, numbers, and _ . -
    /// </summary>
    [Required]
    [RegularExpression("^[a-zA-Z0-9_.-]*$",
        ErrorMessage = "Username can only contain letters, numbers, and the following characters: _ . -")]
    public required string Username { get; init; }

    /// <summary>
    /// User's email address.
    /// </summary>
    [Required]
    [EmailAddress]
    public required string Email { get; init; }

    /// <summary>
    /// User's password. Must be at least 8 characters, contain uppercase, lowercase, and a number.
    /// </summary>
    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
    public required string Password { get; init; }
}