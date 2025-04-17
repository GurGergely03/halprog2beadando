using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Entities;

[Table("Cryptocurrencies", Schema = "dbo")] // linux needs schema specified
[Index(nameof(ShortName), IsUnique = true)]
public class Cryptocurrency
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    
    [Required(ErrorMessage = "Cryptocurrency name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Cryptocurrency name must be between 2-100 characters")]
    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; }
    
    
    [Required]
    [StringLength(10, MinimumLength = 2, ErrorMessage = "Cryptocurrency shortname must be between 2 and 10 characters")]
    [Column(TypeName = "nvarchar(10)")]
    public string ShortName { get; set; }
    
    
    [Required]
    [Display(Name = "Initial Price")]
    [Range(0.00000001, 99999999.999999, ErrorMessage = "Price must be between 0 and 99999999.999999")]
    [Column(TypeName = "decimal(14, 6)")]
    public decimal StartingPrice { get; set; }


    [Required]
    [Display(Name = "Current Price")]
    [Range(0.00000001, 99999999.999999)]
    [Column(TypeName = "decimal(14, 6)")]
    public decimal CurrentPrice { get; set; }
    
    
    [Required]
    [Display(Name = "Total Supply")]
    [Column(TypeName = "decimal(28, 8)")]
    [Range(0, 99999999999999999999.99999999, ErrorMessage = "Total Supply must be between 0 and 999999999999.999999")]
    public decimal TotalAmount { get; set; }
    
    
    [Display(Name = "Available Supply")]
    [Column(TypeName = "decimal(28, 8)")]
    [Range(0, 99999999999999999999.99999999)]
    public decimal AvailableAmount { get; set; }
    
    
    public List<CryptocurrencyHistory> CryptocurrencyHistory { get; set; } = new List<CryptocurrencyHistory>();
    
    
    [NotMapped]
    [Display(Name = "Circulating Supply")]
    public decimal CirculatingSupply => TotalAmount - AvailableAmount;
    
    
    [NotMapped]
    [Display(Name = "Price Change Since Launch")]
    public decimal PriceChangeSinceLaunch => CurrentPrice - StartingPrice;
    
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}