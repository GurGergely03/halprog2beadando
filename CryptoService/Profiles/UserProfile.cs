using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;

namespace CryptoService.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // getall
        CreateMap<User, UserGetDTO>();
        
        // get by id0
        CreateMap<User, UserGetByIdDTO>();
        CreateMap<TransactionHistory, UserTransactionHistoryDTO>();
        
        // register
        CreateMap<UserCreateDTO, User>();
        
        // update
        CreateMap<UserUpdateDTO, User>()
            .ForMember(u => u.Name, opt => opt.PreCondition(dto => dto.Name != null))
            .ForMember(u => u.Password, opt => opt.PreCondition(dto => dto.Password != null))
            .ForMember(u => u.Email, opt => opt.PreCondition(dto => dto.Email != null));
    }
}