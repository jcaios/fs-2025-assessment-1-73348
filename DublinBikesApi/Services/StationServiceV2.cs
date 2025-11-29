using DublinBikesApi.Models;
using Microsoft.Azure.Cosmos;

namespace DublinBikesApi.Services
{
    public class StationServiceV2
    {
        private readonly Container _container;

        public StationServiceV2(CosmosClient client, string databaseName, string containerName)
        {
            _container = client.GetContainer(databaseName, containerName);
        }

        // Pegar todas as estações
        public async Task<List<Station>> GetAllAsync()
        {
            var iterator = _container.GetItemQueryIterator<Station>(new QueryDefinition("SELECT * FROM c"));
            var results = new List<Station>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }

        // Pegar estação por número
        public async Task<Station?> GetByNumberAsync(int number)
        {
            var queryDef = new QueryDefinition("SELECT * FROM c WHERE c.number = @number")
                .WithParameter("@number", number);

            var iterator = _container.GetItemQueryIterator<Station>(queryDef);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                var station = response.FirstOrDefault();
                if (station != null) return station;
            }

            return null;
        }

        // Criar nova estação
        public async Task<Station> CreateAsync(Station newStation)
        {
            var response = await _container.CreateItemAsync(newStation, new PartitionKey(newStation.Number));
            return response.Resource;
        }

        // Atualizar estação existente
        public async Task<Station> UpdateAsync(Station updatedStation)
        {
            var response = await _container.UpsertItemAsync(updatedStation, new PartitionKey(updatedStation.Number));
            return response.Resource;
        }

        // Resumo das estações
        public async Task<object> GetSummaryAsync()
        {
            var stations = await GetAllAsync();
            return new
            {
                totalStations = stations.Count,
                totalBikeStands = stations.Sum(s => s.Bike_Stands),
                totalAvailableBikes = stations.Sum(s => s.Available_Bikes),
                statusCounts = stations
                    .GroupBy(s => s.Status.ToUpper())
                    .ToDictionary(g => g.Key, g => g.Count())
            };
        }
    }
}
