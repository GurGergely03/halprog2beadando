using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;

namespace CryptoService.Profiles;

public class WalletProfile : Profile
{
    public WalletProfile()
    {
        CreateMap<Wallet, WalletGetByIdDTO>();

        CreateMap<Wallet, PortfolioDTO>()
            .ForMember(dest => dest.CryptocurrencyTotalValues, opt => opt.MapFrom(
                src => src.WalletCryptocurrencies.Sum(x => x.Amount * x.Cryptocurrency.CurrentPrice)));

        CreateMap<WalletCryptocurrency, WalletCryptocurrencyDTO>()
            .ForMember(dest => dest.CryptocurrencyName, opt => opt.MapFrom(src => src.Cryptocurrency.Name))
            .ForMember(dest => dest.CryptocurrencyCurrentPrice, opt => opt.MapFrom(src => src.Cryptocurrency.CurrentPrice));
        
        CreateMap<WalletUpdateDTO, Wallet>();
        CreateMap<Wallet, WalletUpdateDTO>();
        
        CreateMap<WalletGetByIdDTO, WalletUpdateDTO>();
    }
}