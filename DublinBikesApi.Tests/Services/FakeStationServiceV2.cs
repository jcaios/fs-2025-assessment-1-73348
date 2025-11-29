using DublinBikesApi.Models;
using DublinBikesApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DublinBikesApi.Tests.Services
{
    public class FakeStationServiceV2 : IStationService
    {
        private List<Station> _stations = new List<Station>();

        public FakeStationServiceV2()
        {
            _stations.Add(new Station
            {
                Number = 1,
                Name = "Fake Station",
                Address = "123 Dublin St",
                Position = new GeoPosition { Lat = 53.345, Lng = -6.260 },
                Bike_Stands = 20,
                Available_Bikes = 10,
                Available_Bike_Stands = 10,
                Status = "OPEN",
                Last_Update = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });
        }

        public Task LoadAsync()
        {
            return Task.CompletedTask;
        }

        public List<Station> GetAll()
        {
            return _stations;
        }

        public Station GetByNumber(int number)
        {
            return _stations.FirstOrDefault(s => s.Number == number);
        }

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
