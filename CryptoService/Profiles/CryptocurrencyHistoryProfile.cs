using System.Runtime.InteropServices.JavaScript;
using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;

namespace CryptoService.Profiles;

public class CryptocurrencyHistoryProfile : Profile
{
    public class CryptocurrencyHistorySources
    {
        public Cryptocurrency Crypto { get; set; }
        public CryptocurrencyUpdateDTO Update { get; set; }
    }
    public CryptocurrencyHistoryProfile()
    {
        // get by crypto id
        CreateMap<CryptocurrencyHistory, CryptocurrencyHistoryGetByCryptoIdDTO>();
        
        // create new history
        CreateMap<CryptocurrencyHistorySources, CryptocurrencyHistory>()
            .ForMember(dest => dest.PriceBefore, opt => opt.MapFrom(src => src.Crypto.CurrentPrice))
            .ForMember(dest => dest.PriceAfter, opt => opt.MapFrom(src => src.Update.CurrentPrice))
            .ForMember(dest => dest.CryptocurrencyId, opt => opt.MapFrom(src => src.Crypto.Id))
            .AfterMap((src, dest) =>
            {
                dest.PriceChange = dest.PriceAfter - dest.PriceBefore;
                dest.PriceChangePercent = (-100) + ((dest.PriceAfter / dest.PriceBefore) * 100);
                dest.ChangeAt = DateTime.Now;
            });
    }
}