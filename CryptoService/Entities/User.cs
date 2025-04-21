using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Entities;
[Table("Users")]
[Index(nameof(Email), IsUnique = true, Name = "IX_Users_Email")]
public sealed class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    
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
    
    
    [ForeignKey(nameof(Wallet))]
    public int WalletId { get; set; }
    
    
    public Wallet Wallet { get; set; } = new Wallet();
    
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}