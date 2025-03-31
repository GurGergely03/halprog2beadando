namespace CryptoService.Entities;

public class TransactionHistory
{
    public int Id { get; set; }
    public Type Type { get; set; }
    public int CryptoCurrencyId { get; set; }
    public Cryptocurrency Cryptocurrency { get; set; }
    public int CryptocurrencyAmount { get; set; }
    public float TransactionPrice { get; set; }
}

public enum Type
{
    Buy,
    Sell
}