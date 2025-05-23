using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;
using CryptoService.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Services;


public interface IWalletService
{
    Task<WalletGetByIdDTO> GetWalletByUserIdAsync(int id);
    Task UpdateWalletBalanceAsync(int id, WalletUpdateDTO wallet);
    Task DeleteWalletAsync(int id);
    Task<PortfolioDTO> GetPortfolioAsync(int id);
}
public class WalletService(IMapper mapper, UnitOfWork unitOfWork, CryptoContext cryptoContext) : IWalletService
{
    public async Task<WalletGetByIdDTO> GetWalletByUserIdAsync(int id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException();
        var wallet = mapper.Map<WalletGetByIdDTO>(await cryptoContext.Wallets.Include(w => w.WalletCryptocurrencies).ThenInclude(wcc => wcc.Cryptocurrency).FirstOrDefaultAsync(w => w.Id == id));
        if (wallet is null) throw new KeyNotFoundException();
        
        return wallet;
    }

    public async Task UpdateWalletBalanceAsync(int id, WalletUpdateDTO wallet)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException();
        var existingWallet = await unitOfWork.WalletRepository.GetByIdAsync(id);
        if (existingWallet is null) throw new KeyNotFoundException();
        
        mapper.Map(wallet, existingWallet);
        await unitOfWork.SaveAsync();
    }

    public async Task DeleteWalletAsync(int id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException();
        var existingWallet = unitOfWork.WalletRepository.GetByIdAsync(id);
        if (existingWallet is null) throw new KeyNotFoundException();

        await unitOfWork.WalletRepository.DeleteByIdAsync(id);
        await unitOfWork.SaveAsync();
    }
    
    public async Task<PortfolioDTO> GetPortfolioAsync(int id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException();
        var wallet = await unitOfWork.WalletRepository.GetByIdAsync(id);
        if (wallet == null) throw new KeyNotFoundException("Wallet with given ID not found.");

        return mapper.Map<PortfolioDTO>(wallet);
    }
}