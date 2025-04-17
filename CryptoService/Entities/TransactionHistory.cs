using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoService.Entities;

public class TransactionHistory
{
    public int Id { get; set; }
    [Required]
    public int WalletId { get; set; }
    public Wallet Wallet { get; set; }
    [Required]
    public int CryptoCurrencyId { get; set; }
    public Cryptocurrency Cryptocurrency { get; set; }
    [Required]
    public int CryptocurrencyHistoryId { get; set; }
    public CryptocurrencyHistory CryptocurrencyHistory { get; set; }
    [Required]
    [Column(TypeName = "decimal(18, 8)")]
    public decimal CryptocurrencyAmount { get; set; } // amount will indicate whether it was a sell or a buy
    [Column(TypeName = "decimal(14, 6)")]
    public decimal TransactionTotal { get; set; }
    public DateTime TransactionTime { get; set; }
}