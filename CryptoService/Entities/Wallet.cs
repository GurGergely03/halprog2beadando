using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoService.Entities;

public class Wallet
{
    public int Id { get; set; }
    [Column(TypeName = "decimal(14, 6)")]
    public decimal Balance { get; set; } = 500;
    [Required]
    public int UserId { get; set; }
    public User User { get; set; }
    public List<TransactionHistory> TransactionHistory { get; set; } = new List<TransactionHistory>();
}