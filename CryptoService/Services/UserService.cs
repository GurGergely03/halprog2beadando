using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;
using CryptoService.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Services;

public interface IUserService
{
    Task<IEnumerable<UserGetDTO>> GetUsersAsync();
    Task<UserGetByIdDTO> GetUserByIdAsync(int id);
    Task AddUserAsync(UserCreateDTO user);
    Task UpdateUserAsync(int id, UserUpdateDTO user);
    Task DeleteUserAsync(int id);
    Task<UserGetProfitByIdDTO> GetProfitByUserIdAsync(int id);
    Task<UserGetProfitDetailsByIdDTO> GetProfitDetailsByIdAsync(int id);
}

public class UserService(UnitOfWork unitOfWork, IMapper mapper, CryptoContext cryptoContext) : IUserService
{
    public async Task<IEnumerable<UserGetDTO>> GetUsersAsync()
    {
        return mapper.Map<List<UserGetDTO>>(await unitOfWork.UserRepository.GetAllAsync(includedProperties: ["Wallet"]));
    }

    public async Task<UserGetByIdDTO> GetUserByIdAsync(int id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException();
        var user = await cryptoContext.Users.Include(u => u.Wallet).ThenInclude(w => w.TransactionHistory).ThenInclude(th => th.Cryptocurrency).FirstOrDefaultAsync(u => u.Id == id);
        if (user is null) throw new KeyNotFoundException();
        
        return mapper.Map<UserGetByIdDTO>(user);
    }

    public async Task AddUserAsync(UserCreateDTO dto)
    {
        var user = mapper.Map<User>(dto);
        user.CreatedAt = DateTime.UtcNow;
        
        await unitOfWork.UserRepository.InsertAsync(mapper.Map<User>(user));
        await unitOfWork.SaveAsync();
    }

    public async Task UpdateUserAsync(int id, UserUpdateDTO user)
    { 
        if (id <= 0) throw new ArgumentOutOfRangeException();
        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(id);
        if (existingUser is null) throw new KeyNotFoundException();
        
        mapper.Map(user, existingUser);
        await unitOfWork.SaveAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException();
        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(id);
        if (existingUser is null) throw new KeyNotFoundException();
        
        await unitOfWork.UserRepository.DeleteByIdAsync(id);
        await unitOfWork.SaveAsync();
    }

    public async Task<UserGetProfitByIdDTO> GetProfitByUserIdAsync(int userId)
    {
        if (userId <= 0) throw new ArgumentOutOfRangeException();
        var user = await cryptoContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) throw new KeyNotFoundException();
        
        var dto =  mapper.Map<UserGetProfitByIdDTO>(user);
        
        dto.ProfitTotal = await cryptoContext.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Wallet.TransactionHistory
                .Sum(th => th.TransactionTotal) + u.Wallet.WalletCryptocurrencies
                .Sum(wcc => wcc.Amount * wcc.Cryptocurrency.CurrentPrice))
            .FirstOrDefaultAsync();

        return dto;
    }

    public async Task<UserGetProfitDetailsByIdDTO> GetProfitDetailsByIdAsync(int userId)
    {
       var dto = mapper.Map<UserGetProfitDetailsByIdDTO>(await GetProfitByUserIdAsync(userId));
       
       var user = await cryptoContext.Users
           .Where(u => u.Id == userId)
           .Select(u => new
           {
               Transactions = u.Wallet.TransactionHistory
                   .Select(th => new 
                   {
                       th.Cryptocurrency.Name,
                       th.Cryptocurrency.ShortName,
                       th.CryptocurrencyAmount,
                       th.TransactionTotal
                   }),
               Cryptocurrencies = u.Wallet.WalletCryptocurrencies
                   .Select(wc => new 
                   {
                       wc.Cryptocurrency.Id,
                       wc.Cryptocurrency.Name,
                       wc.Cryptocurrency.ShortName,
                       wc.Amount,
                       CurrentValue = wc.Amount * wc.Cryptocurrency.CurrentPrice
                   })
           })
           .FirstOrDefaultAsync();

       dto.WalletCryptocurrencyProfitDTO = user.Transactions
           .GroupBy(t => new { t.Name, t.ShortName })
           .Select(g => new WalletCryptocurrencyProfitDTO
           {
               CryptocurrencyName = g.Key.Name,
               CryptocurrencyShortName = g.Key.ShortName,
               Amount = g.Sum(t => t.CryptocurrencyAmount),
               CryptocurrencyProfit = g.Sum(t => t.TransactionTotal)
           })
           .ToList();

       foreach (var crypto in user.Cryptocurrencies)
       {
           dto.WalletCryptocurrencyProfitDTO.Where(wcc => wcc.CryptocurrencyId == crypto.Id)
               .Select(wcc => wcc.CryptocurrencyProfit += crypto.CurrentValue);
       }
       
       return dto;
    }
}

