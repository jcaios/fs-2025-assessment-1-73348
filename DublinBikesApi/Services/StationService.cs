using DublinBikesApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DublinBikesApi.Services
{
    public class StationService : IStationService
    {
        private List<Station> _stations;
        private string _jsonFilePath;

        public StationService(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath;
            _stations = new List<Station>();

            if (File.Exists(_jsonFilePath))
            {
                var json = File.ReadAllText(_jsonFilePath);
                _stations = JsonConvert.DeserializeObject<List<Station>>(json);
            }
        }

        public Task LoadAsync()
        {
            return Task.CompletedTask;
        }

        public List<Station> GetAll() => _stations;

        public Station GetByNumber(int number) => _stations.FirstOrDefault(s => s.Number == number);

        public Task<Station> CreateAsync(Station station)
        {
            _stations.Add(station);
            return Task.FromResult(station);
        }

        public Task<Station> UpdateAsync(Station station)
        {
            var existing = _stations.FirstOrDefault(s => s.Number == station.Number);
            if (existing != null)
            {
                _stations.Remove(existing);
                _stations.Add(station);
            }
            return Task.FromResult(station);
        }

        public Task<object> GetSummaryAsync()
        {
            var summary = new
            {
                totalStations = _stations.Count,
                totalBikeStands = _stations.Sum(s => s.Bike_Stands),
                totalAvailableBikes = _stations.Sum(s => s.Available_Bikes),
                countsByStatus = _stations.GroupBy(s => s.Status)
                                          .ToDictionary(g => g.Key, g => g.Count())
            };
            return Task.FromResult((object)summary);
        }
    }
}
