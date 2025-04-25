using System.Runtime.InteropServices.JavaScript;
using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;

namespace CryptoService.Profiles;

public class CryptocurrencyHistoryProfile : Profile
{
    public class CryptocurrencyHistorySources
    {
        public required Cryptocurrency Crypto { get; set; }
        public required CryptocurrencyUpdateDTO Update { get; set; }
    }
    public CryptocurrencyHistoryProfile()
    {
        // create new history
        CreateMap<CryptocurrencyHistorySources, CryptocurrencyHistory>()
            .ForMember(dest => dest.PriceAt, opt => opt.MapFrom(src => src.Update.CurrentPrice))
            .ForMember(dest => dest.CryptocurrencyId, opt => opt.MapFrom(src => src.Crypto.Id))
            .AfterMap((src, dest) =>
            {
                dest.PriceChange = dest.PriceAt - src.Crypto.CurrentPrice;
                dest.PriceChangePercent = (-100) + ((dest.PriceAt / src.Crypto.CurrentPrice) * 100);
                dest.ChangeAt = DateTime.Now;
            });
    }
}