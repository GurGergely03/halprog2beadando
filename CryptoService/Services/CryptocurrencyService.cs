using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;
using CryptoService.Profiles;
using CryptoService.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CryptoService.Services;

public interface ICryptocurrencyService
{
    Task<IEnumerable<CryptocurrencyGetDTO>> GetCryptocurrencies();
    Task<CryptocurrencyGetByIdDTO> GetCryptocurrencyById(int cryptoId);
    Task AddCryptoAsync(CryptocurrencyCreateDTO cryptocurrency);
    Task UpdateCryptoAsync(int cryptoId, CryptocurrencyUpdateDTO cryptocurrencyUpdate);
    Task DeleteCryptoAsync(int cryptoId);
    Task<IEnumerable<CryptocurrencyHistoryListDTO>> GetCryptocurrencyHistories(int cryptoId);
}
public class CryptocurrencyService(UnitOfWork unitOfWork, IMapper mapper, CryptoContext cryptoContext) : ICryptocurrencyService
{
    public async Task<IEnumerable<CryptocurrencyGetDTO>> GetCryptocurrencies()
    {
        return mapper.Map<List<CryptocurrencyGetDTO>>(await unitOfWork.CryptocurrencyRepository.GetAllAsync());
    }

    public async Task<CryptocurrencyGetByIdDTO> GetCryptocurrencyById(int cryptoId)
    {
        if (cryptoId <= 0) throw new ArgumentOutOfRangeException();
        var cryptoCurrency = await unitOfWork.CryptocurrencyRepository.GetByIdAsync(cryptoId, includedCollections: ["CryptocurrencyHistory"]);
        
        if (cryptoCurrency == null) throw new KeyNotFoundException();
        return mapper.Map<CryptocurrencyGetByIdDTO>(cryptoCurrency);
    }

    public async Task AddCryptoAsync(CryptocurrencyCreateDTO cryptocurrency)
    {
        var crypto = mapper.Map<Cryptocurrency>(cryptocurrency);
        await unitOfWork.CryptocurrencyRepository.InsertAsync(crypto);
        await unitOfWork.SaveAsync();
        
        var sources = new CryptocurrencyHistoryProfile.CryptocurrencyHistorySources
            { Crypto = crypto, Update = mapper.Map<CryptocurrencyUpdateDTO>(crypto) };
        await unitOfWork.CryptocurrencyHistoryRepository.InsertAsync(mapper.Map<CryptocurrencyHistory>(sources));
        
        await unitOfWork.SaveAsync();
    }

    public async Task UpdateCryptoAsync(int cryptoId, CryptocurrencyUpdateDTO crypto)
    {
        if (cryptoId <= 0) throw new ArgumentOutOfRangeException();
        var existingCrypto = await unitOfWork.CryptocurrencyRepository.GetByIdAsync(cryptoId);
        if (existingCrypto == null) throw new KeyNotFoundException();
        
        if (crypto.CurrentPrice != null)
        {
            var sources = new CryptocurrencyHistoryProfile.CryptocurrencyHistorySources {Crypto = existingCrypto, Update = crypto};
            await unitOfWork.CryptocurrencyHistoryRepository.InsertAsync(mapper.Map<CryptocurrencyHistory>(sources));
        }
        
        mapper.Map(crypto, existingCrypto);
        await unitOfWork.SaveAsync();
    }

    public async Task DeleteCryptoAsync(int cryptoId)
    {
        if (cryptoId <= 0) throw new ArgumentOutOfRangeException();
        await unitOfWork.CryptocurrencyRepository.DeleteByIdAsync(cryptoId);
    }

    public async Task<IEnumerable<CryptocurrencyHistoryListDTO>> GetCryptocurrencyHistories(int cryptoId)
    {
        if (cryptoId <= 0) throw new ArgumentOutOfRangeException(); 
        
        var history = mapper.Map<List<CryptocurrencyHistoryListDTO>>(await cryptoContext.CryptocurrencyHistories.Where(x => x.CryptocurrencyId == cryptoId).ToListAsync());
        return history;
    }
}