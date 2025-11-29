using DublinBikesApi.Models;
using DublinBikesApi.Services;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Xunit;

namespace DublinBikesApi.Tests;

public class StationServiceTests
{
    private readonly StationService _service;

    public StationServiceTests()
    {
        // Mock do IWebHostEnvironment
        var envMock = new Mock<IWebHostEnvironment>();
        envMock.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());

        _service = new StationService("Data/dublinbike.json");
        _service.LoadAsync().Wait(); // Carrega JSON
    }

    [Fact]
    public void GetAll_ShouldReturnStations()
    {
        var stations = _service.GetAll();
        Assert.NotEmpty(stations);
    }

    [Fact]
    public void GetByNumber_ShouldReturnCorrectStation()
    {
        var first = _service.GetAll().First();
        var station = _service.GetByNumber(first.Number);
        Assert.NotNull(station);
        Assert.Equal(first.Number, station.Number);
    }

    [Fact]
    public void Occupancy_ShouldBeCorrect()
    {
        var station = _service.GetAll().First();
        if (station.Bike_Stands > 0)
            Assert.Equal((double)station.Available_Bikes / station.Bike_Stands, station.Occupancy);
        else
            Assert.Equal(0, station.Occupancy);
    }
}
