using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CryptoService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.DTOs;

public class UserGetDTO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name cannot be between 3-50 characters")]
    [Column(TypeName = "nvarchar(50)")]
    [RegularExpression($@"[a-zA-Z\s-']+$", ErrorMessage = "Name contains invalid characters")]
    public string Name { get; set; }
    
    
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
    [Column(TypeName = "nvarchar(100)")]
    [Unicode(false)]
    public string Email { get; set; }
    
    
    public int WalletId { get; set; }
    
    
    [Column(TypeName = "decimal(14, 6)")]
    public decimal WalletBalance { get; set; }
    
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class UserGetByIdDTO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name cannot be between 3-50 characters")]
    [Column(TypeName = "nvarchar(50)")]
    [RegularExpression($@"[a-zA-Z\s-']+$", ErrorMessage = "Name contains invalid characters")]
    public string Name { get; set; }
    
    
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
    [Column(TypeName = "nvarchar(100)")]
    [Unicode(false)]
    public string Email { get; set; }
    
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    
    public int WalletId { get; set; }
    
    
    [Column(TypeName = "decimal(14, 6)")]
    public decimal WalletBalance { get; set; }
    
    
    public List<UserTransactionHistoryDTO> WalletTransactionHistories { get; set; } // convert into dto
}

public class UserCreateDTO
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name cannot be between 3-50 characters")]
    [Column(TypeName = "nvarchar(50)")]
    [RegularExpression($@"[a-zA-Z\s-']+$", ErrorMessage = "Name contains invalid characters")]
    public string Name { get; set; }
    
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
    [Column(TypeName = "nvarchar(100)")]
    [Unicode(false)]
    public string Email { get; set; }
    
    
    [Required]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "Password must be between 4-100 characters")]
    public string Password { get; set; }
}

public class UserUpdateDTO
{
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name cannot be between 3-50 characters")]
    [Column(TypeName = "nvarchar(50)")]
    [RegularExpression($@"[a-zA-Z\s-']+$", ErrorMessage = "Name contains invalid characters")]
    public string? Name { get; set; }
    
    
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters")]
    [Column(TypeName = "nvarchar(100)")]
    [Unicode(false)]
    public string? Email { get; set; }
    
    
    [StringLength(100, MinimumLength = 4, ErrorMessage = "Password must be between 4-100 characters")]
    public string? Password { get; set; }
}

public class UserTransactionHistoryDTO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    
    [Column(TypeName = "nvarchar(100)")]
    public string CryptocurrencyName { get; set; }

    
    [Column(TypeName = "decimal(14, 6)")]
    public decimal CryptocurrencyHistoryPriceAfter { get; set; }


    [Column(TypeName = "decimal(18, 8)")]
    public decimal CryptocurrencyAmount { get; set; } // amount will indicate whether it was a sell or a buy
    
    
    [Column(TypeName = "decimal(14, 6)")]
    public decimal TransactionTotal { get; set; }
    
    
    public DateTime TransactionTime { get; set; }
}