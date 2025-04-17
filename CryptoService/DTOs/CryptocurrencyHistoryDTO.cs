namespace CryptoService.DTOs;


public class CryptocurrencyHistoryGetByCryptoIdDTO
{
    public int CryptocurrencyId { get; set; }
    public int Id { get; set; }
    public decimal PriceBefore { get; set; }
    public decimal PriceAfter { get; set; }
    public decimal PriceChange { get; set; }
    public decimal PriceChangePercent { get; set; }
    public DateTime ChangeAt { get; set; }
}

public class CryptocurrencyHistoryCreateDTO
{
    public int CryptocurrencyId { get; set; }
    public decimal PriceBefore { get; set; }
    public decimal PriceAfter { get; set; }
    public decimal PriceChange { get; set; }
    public decimal PriceChangePercent { get; set; }
    public DateTime ChangeAt { get; set; }
}