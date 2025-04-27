using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CryptoService.Entities;

namespace CryptoService.DTOs;

public class CryptocurrencyGetDTO
{
    public int Id { get; set; } 
    public string Name { get; set; }
    public decimal StartingPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal TotalAmount { get; set; }
}

public class CryptocurrencyGetByIdDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public decimal StartingPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AvailableAmount { get; set; }
    public List<CryptocurrencyHistoryListDTO> CryptocurrencyHistory { get; set; }
}

public class CryptocurrencyCreateDTO
{
    public string Name { get; set; }
    public string? ShortName { get; set; } 
    public decimal StartingPrice { get; set; }
    public decimal TotalAmount { get; set; }
}

public class CryptocurrencyUpdateDTO
{ 
    public string? Name { get; set; }
    public string? ShortName { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal? TotalAmount { get; set; }
}

public class CryptocurrencyHistoryListDTO
{
    public int Id { get; set; }
    public decimal PriceAt { get; set; }
    public decimal PriceChange { get; set; }
    public decimal PriceChangePercent { get; set; }
    public DateTime ChangeAt { get; set; } = DateTime.UtcNow;
}

public class CryptocurrencyBuyDTO
{
    public decimal AvailableAmount { get; set; }
}