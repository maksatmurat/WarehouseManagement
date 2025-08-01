using Application.DTOs;
using Domain.Entities;
using Domain.Responses;

namespace Infrastructure.Repositories.Contracts;

public interface IUserAccount
{
    Task<GeneralResponse> CreateAsync(Register user);
    Task<LoginResponse> SignInAsync(Login user);
    Task<LoginResponse> RefreshTokenAsync(RefreshToken token);
    Task<List<ManageUser>> GetUsers();
    Task<GeneralResponse> UpdateUser(ManageUser user);
    Task<List<SystemRole>> GetRoles();
    Task<GeneralResponse> DeleteUser(Guid id);
}
