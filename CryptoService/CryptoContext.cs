using CryptoService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoService;

public class CryptoContext : DbContext
{
    public CryptoContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }
}