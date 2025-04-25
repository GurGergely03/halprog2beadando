using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Entities;

[Table("WalletCryptocurrencies", Schema = "dbo")]
[Index(nameof(WalletId), nameof(CryptocurrencyId), IsUnique = true)]
public class WalletCryptocurrency
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    
    [Required]
    [ForeignKey(nameof(Wallet))]
    public int WalletId { get; set; }
    
    
    public Wallet Wallet { get; set; }
    
    
    [Required]
    [ForeignKey(nameof(Cryptocurrency))]
    public int CryptocurrencyId { get; set; }
    
    
    public Cryptocurrency Cryptocurrency { get; set; }
    
    
    [Required]
    public decimal Amount { get; set; }
}