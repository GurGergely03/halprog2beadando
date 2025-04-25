using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;

namespace CryptoService.Profiles;

public class CryptocurrencyProfile : Profile
{
    public CryptocurrencyProfile()
    {
        // getall
        CreateMap<Cryptocurrency, CryptocurrencyGetDTO>();
        
        // get by id
        CreateMap<Cryptocurrency, CryptocurrencyGetByIdDTO>();
        CreateMap<CryptocurrencyHistory, CryptocurrencyHistoryListDTO>();
        
        // create
        CreateMap<CryptocurrencyCreateDTO, Cryptocurrency>()
            .ForMember(dest => dest.CurrentPrice, opt => opt.MapFrom(src => src.StartingPrice))
            .ForMember(dest => dest.AvailableAmount, opt => opt.MapFrom(src => src.TotalAmount));
        
        // update
        CreateMap<CryptocurrencyUpdateDTO, Cryptocurrency>()
            .ForMember(cc => cc.Name, opt => opt.PreCondition(dto => dto.Name != null))
            .ForMember(cc => cc.ShortName, opt => opt.PreCondition(dto => dto.ShortName != null))
            .ForMember(cc => cc.CurrentPrice, opt => opt.PreCondition(dto => dto.CurrentPrice != null))
            .ForMember(cc => cc.TotalAmount, opt => opt.PreCondition(dto => dto.TotalAmount != null));
        
        // purchase
        CreateMap<CryptocurrencyBuyDTO, Cryptocurrency>();
        
        // crypto to update
        CreateMap<Cryptocurrency, CryptocurrencyUpdateDTO>();
    }
}