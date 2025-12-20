using DublinBikesClient.Models;
using System.Net.Http.Json;

namespace DublinBikesClient.Services;

public class StationsApiClient
{
    private readonly HttpClient _http;

    public StationsApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<StationDto>> GetStationsAsync()
    {
        return await _http.GetFromJsonAsync<List<StationDto>>("api/stations")
               ?? new List<StationDto>();
    }

    public async Task CreateStationAsync(StationDto station)
    {
        await _http.PostAsJsonAsync("api/stations", station);
    }

    public async Task UpdateStationAsync(StationDto station)
    {
        await _http.PutAsJsonAsync($"api/stations/{station.Number}", station);
    }

    public async Task<StationDto?> GetStationAsync(int number)
    {
        return await _http.GetFromJsonAsync<StationDto>($"api/stations/{number}");
    }

}
