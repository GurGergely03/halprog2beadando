using CryptoService.Entities;

namespace CryptoService.DTOs;

public class CryptocurrencyGetDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float StartingPrice { get; set; }
    public float CurrentPrice { get; set; }
    public int TotalAmount { get; set; }
}

public class CryptocurrencyGetByIdDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public float StartingPrice { get; set; }
    public float CurrentPrice { get; set; }
    public int TotalAmount { get; set; }
    public int AvailableAmount { get; set; }
    public List<CryptocurrencyHistory> CryptocurrencyHistory { get; set; }
}

public class CryptocurrencyCreateDTO
{
    public string Name { get; set; }
    public string? ShortName { get; set; }
    public float StartingPrice { get; set; }
    public int TotalAmount { get; set; }
}

public class CryptocurrencyUpdateDTO
{
    public string? Name { get; set; }
    public string? ShortName { get; set; }
    public float? CurrentPrice { get; set; }
    public int? TotalAmount { get; set; }
}