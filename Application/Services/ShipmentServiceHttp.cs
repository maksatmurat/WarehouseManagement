using Application.DTOs;
using Application.Interfaces;
using System.Net.Http.Json;

namespace Application.Services;

public class ShipmentServiceHttp : IShipmentService
{
    private readonly HttpClient _http;

    public ShipmentServiceHttp(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ShipmentDocumentDto>> FilterAsync(ShipmentFilterDto filter)
    {
        var response = await _http.PostAsJsonAsync("api/shipments/filter", filter);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<ShipmentDocumentDto>>();
    }
}
