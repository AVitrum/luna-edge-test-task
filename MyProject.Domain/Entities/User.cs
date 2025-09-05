using System.ComponentModel.DataAnnotations;

namespace MyProject.Domain.Entities;

public sealed class User : BaseEntity
{
    [MaxLength(100)]
    public required string Username { get; set; }
    
    [EmailAddress]
    public required string Email { get; set; }
    
    public required string PasswordHash { get; set; }
}