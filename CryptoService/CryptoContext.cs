using CryptoService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CryptoService;

public class CryptoContext : DbContext
{
    public CryptoContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }
    public DbSet<CryptocurrencyHistory> CryptocurrencyHistories { get; set; }
    public DbSet<TransactionHistory> TransactionHistories { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User -> Wallet connection
        modelBuilder.Entity<User>()
            .HasOne(u => u.Wallet)
            .WithOne(w => w.User)
            .HasForeignKey<Wallet>(w => w.Id)
            .OnDelete(DeleteBehavior.Cascade);
        
        // User's WalletId field
        modelBuilder.Entity<User>()
            .HasIndex(u => u.WalletId)
            .IsUnique();
        
        // Wallet -> TransactionHistory connection
        modelBuilder.Entity<Wallet>()
            .HasMany(w => w.TransactionHistory)
            .WithOne(th => th.Wallet)
            .HasForeignKey(th => th.WalletId)
            .OnDelete(DeleteBehavior.NoAction);
        
        // Cryptocurrency -> CryptocurrencyHistory connection
        modelBuilder.Entity<Cryptocurrency>()
            .HasMany(cc => cc.CryptocurrencyHistory)
            .WithOne(cch => cch.Cryptocurrency)
            .HasForeignKey(cch => cch.CryptocurrencyId)
            .OnDelete(DeleteBehavior.NoAction);

        // Cryptocurrency CreatedAt field value generation
        modelBuilder.Entity<Cryptocurrency>()
            .Property(cc => cc.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd();
        
        // CryptocurrencyHistory -> Cryptocurrency connection
        modelBuilder.Entity<CryptocurrencyHistory>()
            .HasOne(cch => cch.Cryptocurrency)
            .WithMany(cc => cc.CryptocurrencyHistory)
            .OnDelete(DeleteBehavior.NoAction);
        
        // TransactionHistory -> CryptocurrencyHistory connection
        modelBuilder.Entity<TransactionHistory>()
            .HasOne(th => th.CryptocurrencyHistory)
            .WithMany()
            .HasForeignKey(th => th.CryptocurrencyHistoryId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}