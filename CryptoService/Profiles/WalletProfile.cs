using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;

namespace CryptoService.Profiles;

public class WalletProfile : Profile
{
    public WalletProfile()
    {
        // TODO
        CreateMap<Wallet, WalletGetByIdDTO>()
            .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.TransactionHistory.GroupBy(x => x.CryptoCurrencyId)));
        
        CreateMap<WalletUpdateDTO, Wallet>()
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance));
    }
}