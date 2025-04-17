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
    public List<CryptocurrencyHistory> CryptocurrencyHistory { get; set; }
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
    public decimal? CurrentPrice { get; set; }
    public decimal? TotalAmount { get; set; }
}