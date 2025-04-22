using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoService.DTOs;

public class WalletGetByIdDTO
{
    [Key]
    public int Id { get; set; }
    
    
    [Column(TypeName = "decimal(14, 6)")]
    public decimal Balance { get; set; }
    public List<WalletTransactionHistoryGetDTO> Transactions { get; set; }
}

public class WalletUpdateDTO
{
    [Column(TypeName = "decimal(14, 6)")]
    public decimal Balance { get; set; }
}

public class WalletTransactionHistoryGetDTO
{
    [Key]
    public int Id { get; set; }
    
    
    [Display(Name = "Total Supply")]
    [Column(TypeName = "decimal(28, 8)")]
    [Range(0, 99999999999999999999.99999999, ErrorMessage = "Total Supply must be between 0 and 999999999999.999999")]
    public string CryptocurrencyName { get; set; }
    
    
    [Column(TypeName = "decimal(28, 8)")]
    [Range(0, 99999999999999999999.99999999)]
    public decimal CryptocurrencyAmount { get; set; }
}