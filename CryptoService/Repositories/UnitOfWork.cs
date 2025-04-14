using CryptoService.Entities;

namespace CryptoService.Repositories;

public class UnitOfWork
{
    private CryptoContext _context;

    public UnitOfWork(CryptoContext context)
    {
        _context = context;
        UserRepository = new GenericRepository<User>(context);
        CryptocurrencyRepository = new GenericRepository<Cryptocurrency>(context);
        WalletRepository = new GenericRepository<Wallet>(context);
        TransactionHistoryRepository = new GenericRepository<TransactionHistory>(context);
        CryptocurrencyHistoryRepository = new GenericRepository<CryptocurrencyHistory>(context);
    }
    
    public GenericRepository<User> UserRepository { get; set; }
    public GenericRepository<Cryptocurrency> CryptocurrencyRepository { get; set; }
    public GenericRepository<Wallet> WalletRepository { get; set; }
    public GenericRepository<TransactionHistory> TransactionHistoryRepository { get; set; }
    public GenericRepository<CryptocurrencyHistory> CryptocurrencyHistoryRepository { get; set; }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}