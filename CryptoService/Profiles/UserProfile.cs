using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;

namespace CryptoService.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserGetDTO>();
    }
}