using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;
using CryptoService.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Services;


public interface ITransactionHistoryService
{
    Task BuyAsync(TransactionHistoryCreateDTO dto);
    Task SellAsync(TransactionHistoryCreateDTO dto);
    Task<IEnumerable<TransactionHistoryGetByUserIdDTO>> GetTransactionsByUserIdAsync(int userId);
    Task<TransactionHistoryGetByTransactionIdDTO> GetTransactionsByTransactionIdAsync(int transactionId);
}
public class TransactionHistoryService(UnitOfWork unitOfWork, IMapper mapper, CryptoContext cryptoContext) : ITransactionHistoryService
{
    public async Task BuyAsync(TransactionHistoryCreateDTO dto)
    {
        // Validation
        if (dto.CryptocurrencyId <= 0 || dto.WalletId <= 0 || dto.CryptocurrencyAmount <= 0) throw new ArgumentOutOfRangeException();
    
        var wallet = await unitOfWork.WalletRepository.GetByIdAsync(dto.WalletId, includedCollections: ["WalletCryptocurrencies"]);
        if (wallet == null) throw new KeyNotFoundException("Wallet not found.");
    
        var currentCrypto = await unitOfWork.CryptocurrencyRepository.GetByIdAsync(dto.CryptocurrencyId);
        if (currentCrypto == null) throw new KeyNotFoundException("Crypto not found.");

        var walletCrypto = wallet.WalletCryptocurrencies.FirstOrDefault(x => x.CryptocurrencyId == dto.CryptocurrencyId);
        
        // Buy logic
        decimal totalCost = dto.CryptocurrencyAmount * currentCrypto.CurrentPrice;
        
        if (wallet.Balance < totalCost) throw new ArgumentException("Wallet does not have enough balance for purchase.");
        if (currentCrypto.AvailableAmount < dto.CryptocurrencyAmount) throw new ArgumentException("Not enough cryptocurrency available for transaction.");

        if (walletCrypto == null)
        {
            wallet.WalletCryptocurrencies.Add(new WalletCryptocurrency
            {
                CryptocurrencyId = dto.CryptocurrencyId,
                WalletId = dto.WalletId,
                Amount = dto.CryptocurrencyAmount,
            });
        }
        else { walletCrypto.Amount += dto.CryptocurrencyAmount; }
        
        wallet.Balance -= totalCost;
        currentCrypto.AvailableAmount -= dto.CryptocurrencyAmount;

        // Create transaction record
        dto.CryptocurrencyPriceAtPurchase = currentCrypto.CurrentPrice;
        
        var transaction = mapper.Map<TransactionHistory>(dto);
        transaction.TransactionTime = DateTime.Now;
        
        await unitOfWork.TransactionHistoryRepository.InsertAsync(transaction);
        
        await unitOfWork.SaveAsync();
    }

    public async Task SellAsync(TransactionHistoryCreateDTO dto)
    {
        // Validation
        if (dto.CryptocurrencyId <= 0 || dto.WalletId <= 0 || dto.CryptocurrencyAmount <= 0) throw new ArgumentOutOfRangeException();
    
        var wallet = await unitOfWork.WalletRepository.GetByIdAsync(dto.WalletId, includedCollections: ["WalletCryptocurrencies"]);
        if (wallet == null) throw new KeyNotFoundException("Wallet not found.");
    
        var currentCrypto = await unitOfWork.CryptocurrencyRepository.GetByIdAsync(dto.CryptocurrencyId);
        if (currentCrypto == null) throw new KeyNotFoundException("Crypto not found.");

        var walletCrypto = wallet.WalletCryptocurrencies.FirstOrDefault(x => x.CryptocurrencyId == dto.CryptocurrencyId);
        if (walletCrypto == null) throw new KeyNotFoundException("Wallet cryptocurrency not found.");

        // Sell logic
        if (walletCrypto.Amount < dto.CryptocurrencyAmount) throw new ArgumentException("Wallet does not contain enough cryptocurrency for transaction.");
    
        walletCrypto.Amount -= dto.CryptocurrencyAmount;
        wallet.Balance += currentCrypto.CurrentPrice * Math.Abs(dto.CryptocurrencyAmount);

        if (walletCrypto.Amount == 0) wallet.WalletCryptocurrencies.Remove(walletCrypto);
        
        // Create transaction record
        dto.CryptocurrencyPriceAtPurchase = currentCrypto.CurrentPrice;
        
        var transaction = mapper.Map<TransactionHistory>(dto);
        await unitOfWork.TransactionHistoryRepository.InsertAsync(transaction);
        
        await unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<TransactionHistoryGetByUserIdDTO>> GetTransactionsByUserIdAsync(int userId)
    {
        if (userId <= 0) throw new ArgumentOutOfRangeException();
        
        var transactions = mapper.Map<List<TransactionHistoryGetByUserIdDTO>>(await cryptoContext.TransactionHistories.Include(th => th.Cryptocurrency).Where(th => th.WalletId == userId).ToListAsync());
        return transactions;
    }

    public async Task<TransactionHistoryGetByTransactionIdDTO> GetTransactionsByTransactionIdAsync(int transactionId)
    {
        if (transactionId <= 0) throw new ArgumentOutOfRangeException();
        
        return mapper.Map<TransactionHistoryGetByTransactionIdDTO>(await cryptoContext.TransactionHistories.Include(th => th.Cryptocurrency).Where(th => th.Id == transactionId).FirstOrDefaultAsync());
    }
}