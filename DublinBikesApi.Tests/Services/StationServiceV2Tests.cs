using DublinBikesApi.Models;
using DublinBikesApi.Tests.Services; // usa o Fake
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DublinBikesApi.Tests.Services
{
    public class StationServiceV2Tests
    {
        private FakeStationServiceV2 CreateService() => new FakeStationServiceV2();

        [Fact]
        public async Task GetAllAsync_ShouldReturnList()
        {
            var service = CreateService();
            var result = service.GetAll();

            Assert.NotNull(result);
            Assert.IsType<List<Station>>(result);
        }

        [Fact]
        public async Task GetByNumberAsync_ReturnsStationIfExists()
        {
            var service = CreateService();
            var result = service.GetByNumber(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Number);
        }

        [Fact]
        public async Task GetByNumberAsync_ReturnsNullIfNotFound()
        {
            var service = CreateService();
            var result = service.GetByNumber(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddStation()
        {
            var service = CreateService();
            var newStation = new Station { Number = 2, Name = "New", Address = "Addr" };

            var result = await service.CreateAsync(newStation);
            var all = service.GetAll();

            Assert.Contains(newStation, all);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyStation()
        {
            var service = CreateService();
            var updatedStation = new Station { Number = 1, Name = "Updated", Address = "Addr" };

            var result = await service.UpdateAsync(updatedStation);
            var station = service.GetByNumber(1);

            Assert.Equal("Updated", station.Name);
        }

        [Fact]
        public async Task GetSummaryAsync_ShouldReturnObject()
        {
            var service = CreateService();
            var summary = await service.GetSummaryAsync();

            Assert.NotNull(summary);
        }
    }
}
