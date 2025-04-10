namespace CryptoService.Entities;

public class Cryptocurrency
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float StartingPrice { get; set; }
    public float Price { get; set; }
    public int Amount { get; set; }
}