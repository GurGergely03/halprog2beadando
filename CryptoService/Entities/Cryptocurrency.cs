using System.ComponentModel.DataAnnotations;

namespace CryptoService.Entities;

public class Cryptocurrency
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string ShortName { get; set; }
    [Required]
    public float StartingPrice { get; set; }
    public float CurrentPrice { get; set; }
    [Required]
    public int TotalAmount { get; set; }
    public int AvailableAmount { get; set; }
}