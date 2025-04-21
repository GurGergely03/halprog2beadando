using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;
using CryptoService.Profiles;
using CryptoService.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CryptoService.Services;

public interface ICryptocurrencyService
{
    Task<IEnumerable<CryptocurrencyGetDTO>> GetCryptocurrencies();
    Task<CryptocurrencyGetByIdDTO> GetCryptocurrencyById(int id);
    Task AddCryptoAsync(CryptocurrencyCreateDTO cryptocurrency);
    Task UpdateCryptoAsync(int id, CryptocurrencyUpdateDTO cryptocurrencyUpdate);
    Task DeleteCryptoAsync(int id);
}
public class CryptocurrencyService(UnitOfWork unitOfWork, IMapper mapper) : ICryptocurrencyService
{
    public async Task<IEnumerable<CryptocurrencyGetDTO>> GetCryptocurrencies()
    {
        return mapper.Map<List<CryptocurrencyGetDTO>>(await unitOfWork.CryptocurrencyRepository.GetAllAsync());
    }

    public async Task<CryptocurrencyGetByIdDTO> GetCryptocurrencyById(int id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException();
        var cryptoCurrency = await unitOfWork.CryptocurrencyRepository.GetByIdAsync(id, includedCollections: ["CryptocurrencyHistory"]);
        
        if (cryptoCurrency == null) throw new KeyNotFoundException();
        return mapper.Map<CryptocurrencyGetByIdDTO>(cryptoCurrency);
    }

    public async Task AddCryptoAsync(CryptocurrencyCreateDTO cryptocurrency)
    {
        await unitOfWork.CryptocurrencyRepository.InsertAsync(mapper.Map<Cryptocurrency>(cryptocurrency));
        await unitOfWork.SaveAsync();
    }

    public async Task UpdateCryptoAsync(int id, CryptocurrencyUpdateDTO crypto)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException();
        var existingCrypto = await unitOfWork.CryptocurrencyRepository.GetByIdAsync(id);
        if (existingCrypto == null) throw new KeyNotFoundException();
        
        if (crypto.CurrentPrice != null)
        {
            var sources = new CryptocurrencyHistoryProfile.CryptocurrencyHistorySources {Crypto = existingCrypto, Update = crypto};
            existingCrypto.CryptocurrencyHistory.Add(mapper.Map<CryptocurrencyHistory>(sources));
            await unitOfWork.CryptocurrencyHistoryRepository.InsertAsync(mapper.Map<CryptocurrencyHistory>(sources));
        }
        
        mapper.Map(crypto, existingCrypto);
        await unitOfWork.SaveAsync();
    }

    public async Task DeleteCryptoAsync(int id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException();
        await unitOfWork.CryptocurrencyRepository.DeleteByIdAsync(id);
    }
}