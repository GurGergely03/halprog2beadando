using CryptoService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CryptoService;

public class CryptoContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }
    public DbSet<CryptocurrencyHistory> CryptocurrencyHistories { get; set; }
    public DbSet<TransactionHistory> TransactionHistories { get; set; }
    public DbSet<WalletCryptocurrency> WalletCryptocurrencies { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /* USER ENTITY */
        // User -> Wallet (One-to-One)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Wallet)
            .WithOne(w => w.User)
            .HasForeignKey<User>(u => u.WalletId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // User WalletId uniqueness
        modelBuilder.Entity<User>()
            .HasIndex(u => u.WalletId)
            .IsUnique();
        
        
        /* WALLET ENTITY */
        // Wallet -> TransactionHistory connection
        modelBuilder.Entity<Wallet>()
            .HasMany(w => w.TransactionHistory)
            .WithOne(th => th.Wallet)
            .HasForeignKey(th => th.WalletId)
            .OnDelete(DeleteBehavior.NoAction);
        
        
        /* CRYPTOCURRENCY ENTITY */
        // Cryptocurrency -> CryptocurrencyHistory connection
        modelBuilder.Entity<Cryptocurrency>()
            .HasMany(cc => cc.CryptocurrencyHistory)
            .WithOne(cch => cch.Cryptocurrency)
            .HasForeignKey(cch => cch.CryptocurrencyId)
            .OnDelete(DeleteBehavior.NoAction);
        
        
        /* CRYPTOCURRENCY HISTORY ENTITY */
        // CryptocurrencyHistory -> Cryptocurrency connection
        modelBuilder.Entity<CryptocurrencyHistory>()
            .HasOne(cch => cch.Cryptocurrency)
            .WithMany(cc => cc.CryptocurrencyHistory)
            .OnDelete(DeleteBehavior.NoAction);
        
        
        /* WALLET CRYPTOCURRENCY ENTITY */
        // WalletCryptocurrency -> Wallet (Many-to-One)
        modelBuilder.Entity<WalletCryptocurrency>()
            .HasOne(wcc => wcc.Wallet)
            .WithMany(w => w.WalletCryptocurrencies)
            .HasForeignKey(wcc => wcc.WalletId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // WalletCryptocurrency -> Cryptocurrency (Many-to-One)
        modelBuilder.Entity<WalletCryptocurrency>()
            .HasOne(wcc => wcc.Cryptocurrency)
            .WithMany()
            .HasForeignKey(wcc => wcc.CryptocurrencyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}