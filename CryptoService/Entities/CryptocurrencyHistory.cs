namespace CryptoService.Entities;

public class CryptocurrencyHistory
{
    public int Id { get; set; }
    public float PriceBefore { get; set; }
    public float PriceAfter { get; set; }
    public float PriceChange { get; set; }
    public double PriceChangePercent { get; set; }
    public DateTime ChangeAt { get; set; }
    public int CryptocurrencyId { get; set; }
    public Cryptocurrency Cryptocurrency { get; set; }
}