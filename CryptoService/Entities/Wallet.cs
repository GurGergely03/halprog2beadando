using System.ComponentModel.DataAnnotations;

namespace CryptoService.Entities;

public class Wallet
{
    public int Id { get; set; }
    public Dictionary<Cryptocurrency, int> Cryptocurrencies { get; set; } = new Dictionary<Cryptocurrency, int>();
    public float Balance { get; set; } = 500;
    [Required]
    public int UserId { get; set; }
    public User User { get; set; }
}