using AutoMapper;
using CryptoService.DTOs;
using CryptoService.Entities;
using CryptoService.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CryptoService.Services;

public interface IUserService
{
    Task<IEnumerable<UserGetDTO>> GetUsersAsync();
    Task<UserGetByIdDTO> GetUserByIdAsync(int id);
    Task AddUserAsync(UserCreateDTO user);
    Task UpdateUserAsync(int id, UserUpdateDTO user);
    Task DeleteUserAsync(int id);
}

public class UserService(UnitOfWork unitOfWork, IMapper mapper) : IUserService
{
    public async Task<IEnumerable<UserGetDTO>> GetUsersAsync()
    {
        return mapper.Map<List<UserGetDTO>>(await unitOfWork.UserRepository.GetAllAsync());
    }

    public async Task<UserGetByIdDTO> GetUserByIdAsync(int id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException();
        var user = await unitOfWork.UserRepository.GetByIdAsync(id);
        if (user is null) throw new KeyNotFoundException();
        
        return mapper.Map<UserGetByIdDTO>(user);
    }

    public async Task AddUserAsync(UserCreateDTO user)
    { 
        await unitOfWork.UserRepository.InsertAsync(mapper.Map<User>(user));
        await unitOfWork.SaveAsync();
    }

    public async Task UpdateUserAsync(int id, UserUpdateDTO user)
    { 
        if (id <= 0) throw new ArgumentOutOfRangeException();
        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(id);
        if (existingUser is null) throw new KeyNotFoundException();
        
        mapper.Map(user, existingUser);
        await unitOfWork.SaveAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException();
        
        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(id);
        if (existingUser is null) throw new KeyNotFoundException();
        
        await unitOfWork.UserRepository.DeleteByIdAsync(id);
        await unitOfWork.SaveAsync();
    }
}

