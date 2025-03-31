using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Entities;

[Index(nameof(Email), IsUnique = true)]
public sealed class User
{
    public int Id { get; set; }
    [MaxLength(30)]
    [MinLength(3)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(50)]
    [EmailAddress]
    public string Email { get; set; }
    
    [MaxLength(20)]
    [MinLength(5)]
    [Required]
    public string Password { get; set; }

    public Wallet Wallet { get; set; }
    public Role Role { get; set; } = Role.User;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public enum Role
{
    Admin,
    User
}