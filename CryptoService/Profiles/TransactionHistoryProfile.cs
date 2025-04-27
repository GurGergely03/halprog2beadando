using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;

namespace CryptoService.Profiles;

public class TransactionHistoryProfile : Profile
{
    public TransactionHistoryProfile()
    {
        CreateMap<TransactionHistory, TransactionHistoryGetByUserIdDTO>();
        CreateMap<TransactionHistory, TransactionHistoryGetByTransactionIdDTO>()
            .ForMember(dest => dest.CryptocurrencyPriceChangeSinceTransaction,
                opt => opt.MapFrom(src => src.Cryptocurrency.CurrentPrice - src.CryptocurrencyPriceAtPurchase));
        CreateMap<TransactionHistoryCreateDTO, TransactionHistory>();
    }
}