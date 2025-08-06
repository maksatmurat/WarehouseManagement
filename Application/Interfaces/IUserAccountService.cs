using Domain.DTOs;
using Domain.Entities;
using Domain.Responses;

namespace Application.Interfaces;

public interface IUserAccountService
{
    Task<GeneralResponse> CreateAsync(Register user);
    Task<LoginResponse> SignInAsync(Login user);
    Task<LoginResponse> RefreshTokenAsync(RefreshToken Token);
    Task<List<ManageUser>> GetUsers();
    Task<GeneralResponse> UpdateUser(ManageUser user);
    Task<List<SystemRole>> GetRoles();
    Task<GeneralResponse> DeleteUser(Guid id);
}
