namespace DublinBikesApi.Models;

public class GeoPosition
{
    public double Lat { get; set; }
    public double Lng { get; set; }
}

public class Station
{
    public int Number { get; set; }
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
    public GeoPosition Position { get; set; } = new();
    public int Bike_Stands { get; set; }
    public int Available_Bikes { get; set; }
    public int Available_Bike_Stands { get; set; }
    public string Status { get; set; } = "";
    public long Last_Update { get; set; }

    public double Occupancy =>
        Bike_Stands == 0 ? 0 : (double)Available_Bikes / Bike_Stands;

    public DateTimeOffset LastUpdateUtc =>
        DateTimeOffset.FromUnixTimeMilliseconds(Last_Update);
}
