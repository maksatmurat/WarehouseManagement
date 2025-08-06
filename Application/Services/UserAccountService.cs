using Application.Helpers;
using Application.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Domain.Responses;
using System.Net.Http.Json;

namespace Application.Services;

public class UserAccountService(GetHttpClient getHttpClient) : IUserAccountService
{
    public const string AuthUrl = "api/authentication";
    public async Task<GeneralResponse> CreateAsync(Register user)
    {
        var httpClient = getHttpClient.GetPublicHttpClient();
        var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/register", user);
        if (!result.IsSuccessStatusCode)
        {
            return new GeneralResponse(false, "Registration failed. Please try again.");
        }
        return await result.Content.ReadFromJsonAsync<GeneralResponse>() ?? new GeneralResponse(false, "Registration failed. Please try again.");

    }
    public async Task<LoginResponse> SignInAsync(Login user)
    {
        var httpClient = getHttpClient.GetPublicHttpClient();
        var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/login", user);
        if (!result.IsSuccessStatusCode) return new LoginResponse(false, "Login failed. Please try again.");
        return await result.Content.ReadFromJsonAsync<LoginResponse>() ?? new LoginResponse(false, "Login failed. Please try again.");

    }
    public async Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
    {
        var httpClient = getHttpClient.GetPublicHttpClient();

        HttpResponseMessage result;
        try
        {
            result = await httpClient.PostAsJsonAsync($"{AuthUrl}/refresh-token", token);
        }
        catch (Exception ex)
        {
            // Hata loglanabilir
            return new LoginResponse(false, "İstek başarısız: " + ex.Message);
        }

        if (!result.IsSuccessStatusCode)
        {
            return new LoginResponse(false, $"Sunucu hatası: {result.StatusCode}");
        }

        var response = await result.Content.ReadFromJsonAsync<LoginResponse>();

        if (response == null)
        {
            return new LoginResponse(false, "Yanıt çözümlenemedi.");
        }

        return response;
    }
    public async Task<List<ManageUser>> GetUsers()
    {
        var httpclient = await getHttpClient.GetPrivateHttpClient();
        var result = await httpclient.GetFromJsonAsync<List<ManageUser>>($"{AuthUrl}/users");
        return result!;
    }
    public async Task<GeneralResponse> UpdateUser(ManageUser user)
    {
        var httpClient = getHttpClient.GetPublicHttpClient();
        var result = await httpClient.PutAsJsonAsync($"{AuthUrl}/update-user", user);
        if (!result.IsSuccessStatusCode) return new GeneralResponse(false, " Error occured");
        return await result.Content.ReadFromJsonAsync<GeneralResponse>() ?? new GeneralResponse(false, " Error occured");
    }
    public async Task<List<SystemRole>> GetRoles()
    {
        var httpClient = await getHttpClient.GetPrivateHttpClient();
        var result = await httpClient.GetFromJsonAsync<List<SystemRole>>($"{AuthUrl}/roles");
        return result!;
    }
    public async Task<GeneralResponse> DeleteUser(Guid id)
    {
        var httpClient = await getHttpClient.GetPrivateHttpClient();
        var result = await httpClient.DeleteAsync($"{AuthUrl}/delete-user/{id}");
        if (!result.IsSuccessStatusCode) return new GeneralResponse(false, "Error occured");

        return await result.Content.ReadFromJsonAsync<GeneralResponse>() ?? new GeneralResponse(false, " Error occured");
    }
}
