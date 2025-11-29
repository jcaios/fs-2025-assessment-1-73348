using DublinBikesApi.Models;

namespace DublinBikesApi.Services;

public interface IStationService
{
    Task LoadAsync();
    List<Station> GetAll();
    Station GetByNumber(int number);

    // Adicione estes métodos
    Task<Station> CreateAsync(Station station);
    Task<Station> UpdateAsync(Station station);
    Task<object> GetSummaryAsync();
}

