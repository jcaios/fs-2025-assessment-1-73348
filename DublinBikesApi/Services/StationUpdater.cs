using DublinBikesApi.Models;

namespace DublinBikesApi.Services;

public class StationUpdater
{
    private readonly IStationService _stationService;
    private readonly Random _random = new();

    public StationUpdater(IStationService stationService)
    {
        _stationService = stationService;
    }

    public void StartUpdating(int intervalSeconds = 10)
    {
        // Cria uma task em background
        Task.Run(async () =>
        {
            while (true)
            {
                UpdateStationsRandomly();
                await Task.Delay(intervalSeconds * 1000);
            }
        });
    }

    private void UpdateStationsRandomly()
    {
        var stations = _stationService.GetAll();

        foreach (var station in stations)
        {
            // Randomiza available bikes e bike stands mantendo consistência
            var maxBikes = station.Bike_Stands;
            station.Available_Bikes = _random.Next(0, maxBikes + 1);
            station.Available_Bike_Stands = maxBikes - station.Available_Bikes;
            station.Last_Update = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}
