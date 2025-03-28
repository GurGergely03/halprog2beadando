namespace CryptoService.Entities;

public class Wallet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Value { get; set; }
    public int Amount { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}