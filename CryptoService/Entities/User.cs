using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Entities;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Email { get; set; }
    public string Password { get; set; }
    public float Balance { get; set; }
    public List<Wallet> Wallets { get; set; } = new List<Wallet>();
}