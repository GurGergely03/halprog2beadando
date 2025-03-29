using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Entities;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    [MaxLength(30)]
    [MinLength(3)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(50)]
    [MinLength(10)]
    public string Email { get; set; }
    
    [MaxLength(20)]
    [MinLength(5)]
    [Required]
    public string Password { get; set; }
    
    public float Balance { get; set; }
    
    public List<Wallet> Wallets { get; set; } = new List<Wallet>();
}