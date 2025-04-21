using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CryptoService.Entities;

namespace CryptoService.DTOs;

public class CryptocurrencyGetDTO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Cryptocurrency name must be between 2-100 characters")]
    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; }
    
    
    [Display(Name = "Initial Price")]
    [Range(0.00000001, 99999999.999999, ErrorMessage = "Price must be between 0 and 99999999.999999")]
    [Column(TypeName = "decimal(14, 6)")]
    public decimal StartingPrice { get; set; }
    
    
    [Display(Name = "Current Price")]
    [Range(0.00000001, 99999999.999999)]
    [Column(TypeName = "decimal(14, 6)")]
    public decimal CurrentPrice { get; set; }
    
    
    [Display(Name = "Total Supply")]
    [Column(TypeName = "decimal(28, 8)")]
    [Range(0, 99999999999999999999.99999999, ErrorMessage = "Total Supply must be between 0 and 999999999999.999999")]
    public decimal TotalAmount { get; set; }
}

public class CryptocurrencyGetByIdDTO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Cryptocurrency name must be between 2-100 characters")]
    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; }
    
    
    [StringLength(10, MinimumLength = 2, ErrorMessage = "Cryptocurrency shortname must be between 2 and 10 characters")]
    [Column(TypeName = "nvarchar(10)")]
    public string ShortName { get; set; }
    
    
    [Display(Name = "Initial Price")]
    [Range(0.00000001, 99999999.999999, ErrorMessage = "Price must be between 0 and 99999999.999999")]
    [Column(TypeName = "decimal(14, 6)")]
    public decimal StartingPrice { get; set; }
    
    
    [Display(Name = "Current Price")]
    [Range(0.00000001, 99999999.999999)]
    [Column(TypeName = "decimal(14, 6)")]
    public decimal CurrentPrice { get; set; }
    
    
    [Display(Name = "Total Supply")]
    [Column(TypeName = "decimal(28, 8)")]
    [Range(0, 99999999999999999999.99999999, ErrorMessage = "Total Supply must be between 0 and 999999999999.999999")]
    public decimal TotalAmount { get; set; }
    
    
    [Display(Name = "Available Supply")]
    [Column(TypeName = "decimal(28, 8)")]
    [Range(0, 99999999999999999999.99999999)]
    public decimal AvailableAmount { get; set; }
    
    
    public List<CryptocurrencyHistoryListDTO> CryptocurrencyHistory { get; set; }
}

public class CryptocurrencyCreateDTO
{
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Cryptocurrency name must be between 2-100 characters")]
    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; }
    
    
    [StringLength(10, MinimumLength = 2, ErrorMessage = "Cryptocurrency shortname must be between 2 and 10 characters")]
    [Column(TypeName = "nvarchar(10)")]
    public string? ShortName { get; set; }
    
    
    [Display(Name = "Initial Price")]
    [Range(0.00000001, 99999999.999999, ErrorMessage = "Price must be between 0 and 99999999.999999")]
    [Column(TypeName = "decimal(14, 6)")]
    public decimal StartingPrice { get; set; }
    
    
    [Display(Name = "Total Supply")]
    [Column(TypeName = "decimal(28, 8)")]
    [Range(0, 99999999999999999999.99999999, ErrorMessage = "Total Supply must be between 0 and 999999999999.999999")]
    public decimal TotalAmount { get; set; }
}

public class CryptocurrencyUpdateDTO
{
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Cryptocurrency name must be between 2-100 characters")]
    [Column(TypeName = "nvarchar(100)")]
    public string? Name { get; set; }
    
    
    [StringLength(10, MinimumLength = 2, ErrorMessage = "Cryptocurrency shortname must be between 2 and 10 characters")]
    [Column(TypeName = "nvarchar(10)")]
    public string? ShortName { get; set; }
    
    
    [Display(Name = "Current Price")]
    [Range(0.00000001, 99999999.999999)]
    [Column(TypeName = "decimal(14, 6)")]
    public decimal? CurrentPrice { get; set; }
    
    
    [Display(Name = "Total Supply")]
    [Column(TypeName = "decimal(28, 8)")]
    [Range(0, 99999999999999999999.99999999, ErrorMessage = "Total Supply must be between 0 and 999999999999.999999")]
    public decimal? TotalAmount { get; set; }
}

public class CryptocurrencyHistoryListDTO
{
    [Key]
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
    
    
    [DataType(DataType.Date)]
    [Display(Name = "Change Timestamp")]
    public DateTime ChangeAt { get; set; } = DateTime.UtcNow;
    
}