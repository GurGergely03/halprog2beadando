using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;

namespace CryptoService.Profiles;

public class WalletProfile : Profile
{
    public WalletProfile()
    {
        CreateMap<Wallet, WalletGetByIdDTO>()
            .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.TransactionHistory
                .GroupBy(x => x.CryptocurrencyId)
                .Select(g => new WalletTransactionHistoryGetDTO
                {
                    Id = g.Key,
                    CryptocurrencyAmount = g.Sum(t => t.CryptocurrencyAmount)
                })
                .ToList()
            ))
            .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.TransactionHistory));

        CreateMap<WalletUpdateDTO, Wallet>();
    }
}