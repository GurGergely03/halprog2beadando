using System.ComponentModel.DataAnnotations;

namespace CryptoService.Entities;

public class Wallet
{
    public int Id { get; set; }
    public float Balance { get; set; } = 500;
    [Required]
    public int UserId { get; set; }
    public User User { get; set; }
    public List<TransactionHistory> TransactionHistory { get; set; } = new List<TransactionHistory>();
}