using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CryptoService.Entities;

namespace CryptoService.DTOs;


// GET BY ID
public class WalletGetByIdDTO
{
    public int Id { get; set; }
    public decimal Balance { get; set; }
    public List<WalletCryptocurrencyDTO> WalletCryptocurrencies { get; set; }
}


// UPDATE BALANCE
public class WalletUpdateDTO
{
    public decimal Balance { get; set; }
}


// PORTFOLIO
public class PortfolioDTO
{
    public int Id { get; set; }
    public decimal Balance { get; set; }
    public List<WalletCryptocurrencyDTO> WalletCryptocurrencies { get; set; }
    public decimal CryptocurrencyTotalValues { get; set; }
    public decimal WalletValueTotal => CryptocurrencyTotalValues + Balance;
}


// WALLET CRYPTOCURRENCY
public class WalletCryptocurrencyDTO
{
    public int WalletId { get; set; }
    public int CryptocurrencyId { get; set; }
    public string CryptocurrencyName { get; set; }
    public decimal CryptocurrencyCurrentPrice { get; set; }
    public decimal Amount { get; set; }
    public decimal CryptocurrencyTotalValue => Amount * CryptocurrencyCurrentPrice;
}
