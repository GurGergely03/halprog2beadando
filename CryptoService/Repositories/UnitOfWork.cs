using CryptoService.Entities;

namespace CryptoService.Repositories;

public class UnitOfWork
{
    private CryptoContext _context;

    public UnitOfWork(CryptoContext context)
    {
        _context = context;
        UserRepository = new GenericRepository<User>(_context);
        CryptocurrencyRepository = new GenericRepository<Cryptocurrency>(_context);
        WalletRepository = new GenericRepository<Wallet>(_context);
        TransactionHistoryRepository = new GenericRepository<TransactionHistory>(_context);
        CryptocurrencyHistoryRepository = new GenericRepository<CryptocurrencyHistory>(_context);
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