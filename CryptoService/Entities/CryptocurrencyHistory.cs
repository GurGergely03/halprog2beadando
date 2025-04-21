using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Entities;
[Table("CryptocurrencyHistories", Schema = "dbo")]
[Index(nameof(ChangeAt))]
public class CryptocurrencyHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    
    [Column(TypeName = "decimal(14, 6)")]
    [Range(0, 99999999.999999, ErrorMessage = "Price must be between 0 and 99,999,999.999999.")]
    public decimal PriceBefore { get; set; }
    
    
    [Column(TypeName = "decimal(14, 6)")]
    [Range(0, 99999999.999999)]
    public decimal PriceAfter { get; set; }
    
    
    [Column(TypeName = "decimal(14, 6)")]
    [Range(-99999999.999999, 99999999.999999)]
    public decimal PriceChange { get; set; }
    
    
    [Column(TypeName = "decimal(10, 4)")]
    [Range(-999999.9999, 999999.9999, ErrorMessage = "Percentage must be between -999,999.9999 and 999,999.9999.")]
    public decimal PriceChangePercent { get; set; }
    
    
    [Required]
    [ForeignKey(nameof(Cryptocurrency))]
    public int CryptocurrencyId { get; set; }
    
    
    [DeleteBehavior(DeleteBehavior.NoAction)]    
    public Cryptocurrency Cryptocurrency { get; set; }
    
    
    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Change Timestamp")]
    public DateTime ChangeAt { get; set; } = DateTime.UtcNow;

    
    [NotMapped]
    [Display(Name = "Price Change Direction")]
    public string ChangeDirection => PriceChange >= 0 ? "Up" : "Down";
}