using System.ComponentModel.DataAnnotations;

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
    public int CryptocurrencyAmount { get; set; } // amount will indicate whether it was a sell or a buy
    public float CryptocurrencyExchangeRate { get; set; }
    public float TransactionTotal { get; set; }
    public DateTime TransactionTime { get; set; }
}