using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoService.Entities;

[Table("TransactionHistories", Schema = "dbo")]
public class TransactionHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [ForeignKey(nameof(Wallet))]
    public int WalletId { get; set; }
    public Wallet Wallet { get; set; }
    
    [ForeignKey(nameof(Cryptocurrency))]
    public int CryptocurrencyId { get; set; }
    public Cryptocurrency Cryptocurrency { get; set; }
    
    [Column(TypeName = "decimal(14, 6)")]
    public decimal CryptocurrencyPriceAtPurchase { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18, 8)")]
    public decimal CryptocurrencyAmount { get; set; } // amount will indicate whether it was a sell or a buy

    [Column(TypeName = "decimal(14, 6)")]
    public decimal TransactionTotal => CryptocurrencyPriceAtPurchase * CryptocurrencyAmount;
    public DateTime TransactionTime { get; set; }
}