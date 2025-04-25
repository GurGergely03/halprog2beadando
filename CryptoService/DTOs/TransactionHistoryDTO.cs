using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CryptoService.DTOs;

public class TransactionHistoryGetByUserIdDTO
{
    public int Id { get; set; }
    public int CryptocurrencyId { get; set; }
    public string CryptocurrencyShortName { get; set; }
    public decimal CryptocurrencyAmount { get; set; } // amount will indicate whether it was a sell or a buy
    public decimal TransactionTotal { get; set; }
    public DateTime TransactionTime { get; set; }
}

public class TransactionHistoryGetByTransactionIdDTO
{
    public int Id { get; set; }
    public int CryptocurrencyId { get; set; }
    public string CryptocurrencyShortName { get; set; }
    public string CryptocurrencyName { get; set; }
    public decimal CryptocurrencyPriceAtPurchase  { get; set; }
    public decimal CryptocurrencyAmount  { get; set; }
    public decimal TransactionTotal  { get; set; }
    public decimal CryptocurrencyPriceChangeSinceTransaction { get; set; }
    public DateTime TransactionTime { get; set; }
}

public class TransactionHistoryCreateDTO
{
    public int WalletId { get; set; }
    public int CryptocurrencyId { get; set; }
    public decimal CryptocurrencyAmount { get; set; } // amount will indicate whether it was a sell or a buy
    [JsonIgnore]
    public decimal CryptocurrencyPriceAtPurchase { get; set; }
}
