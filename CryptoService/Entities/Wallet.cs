using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoService.Entities;

[Table("Wallets", Schema = "dbo")]
public class Wallet
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    
    [Required(ErrorMessage = "Starting Balance must be provided to create a wallet.")]
    [Column(TypeName = "decimal(14, 6)")]
    public decimal Balance { get; set; } = 500;
    
    
    [Required]
    public int UserId { get; set; }
    
    
    public User User { get; set; }
    
    
    public List<WalletCryptocurrency> WalletCryptocurrencies { get; set; } = new List<WalletCryptocurrency>();
    
    public List<TransactionHistory> TransactionHistory { get; set; } = new List<TransactionHistory>();
}