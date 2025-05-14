using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CryptoService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.DTOs;

public class UserGetDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int WalletId { get; set; }
    public decimal WalletBalance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class UserGetByIdDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int WalletId { get; set; }
    public decimal WalletBalance { get; set; }
    public List<UserTransactionHistoryDTO> WalletTransactionHistory { get; set; } // convert into dto
}

public class UserCreateDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserUpdateDTO
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class UserTransactionHistoryDTO
{
    public int Id { get; set; }
    public string CryptocurrencyName { get; set; }
    public decimal CryptocurrencyPriceAtPurchase { get; set; }
    public decimal CryptocurrencyAmount { get; set; } 
    public decimal TransactionTotal { get; set; }
    public DateTime TransactionTime { get; set; }
}

public class UserGetProfitByIdDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal ProfitTotal { get; set; }
}

public class UserGetProfitDetailsByIdDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal ProfitTotal { get; set; }
    public List<WalletCryptocurrencyProfitDTO> WalletCryptocurrencyProfitDTO { get; set; }
}

public class WalletCryptocurrencyProfitDTO
{
    public int Id { get; set; }
    public int CryptocurrencyId { get; set; }
    public string CryptocurrencyName { get; set; }
    public string CryptocurrencyShortName { get; set; }
    public decimal Amount { get; set; }
    public decimal CryptocurrencyProfit { get; set; }
}