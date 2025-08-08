using Application.Interfaces;
using System.Net.Http.Json;

namespace Application.Services;

public class GenericApiService<TEntity> : IGenericService<TEntity> where TEntity : class
{
    private readonly HttpClient _httpClient;
    private readonly string _endpoint;

    public GenericApiService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _endpoint = $"api/{typeof(TEntity).Name.ToLower()}";
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<TEntity>>(_endpoint)
               ?? new List<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<TEntity>($"{_endpoint}/{id}");
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        var response = await _httpClient.PostAsJsonAsync(_endpoint, entity);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TEntity>();
        if (result is null)
            throw new InvalidOperationException("The API response did not contain a valid entity.");
        return result;
    }

    public async Task<TEntity> UpdateAsync(Guid id, TEntity entity)
    {
        var response = await _httpClient.PutAsJsonAsync($"{_endpoint}/{id}", entity);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TEntity>();
        if (result is null)
            throw new InvalidOperationException("The API response did not contain a valid entity.");
        return result;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{_endpoint}/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        var items = await _httpClient.GetFromJsonAsync<List<TEntity>>(_endpoint)
               ?? new List<TEntity>();
        var nameProp = typeof(TEntity).GetProperty("Name");
        if (nameProp == null)
            throw new InvalidOperationException("TEntity does not have a 'Name' property.");

        return items.Any(x =>
            nameProp.GetValue(x)?.ToString()?.Equals(name, StringComparison.OrdinalIgnoreCase) == true
        );
    }
}
