using System.ComponentModel.DataAnnotations;
using CryptoService.Entities;

namespace CryptoService.DTOs;

public class UserGetDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int WalletId { get; set; }
    public float WalletBalance { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UserGetByIdDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public int WalletId { get; set; }
    public float WalletBalance { get; set; }
    public List<TransactionHistory> WalletTransactionHistories { get; set; } // convert into dto
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