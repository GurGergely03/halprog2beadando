namespace CryptoService.Entities;

public class CryptocurrencyHistory
{
    public int Id { get; set; }
    public float PriceAtTime { get; set; }
    public float PriceChange { get; set; }
    public float PriceAfterChange { get; set; }
    public double PriceChangePercent { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CryptocurrencyId { get; set; }
    public Cryptocurrency Cryptocurrency { get; set; }
}