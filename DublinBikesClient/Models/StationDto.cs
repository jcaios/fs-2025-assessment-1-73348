namespace DublinBikesClient.Models
{
    public class StationDto
    {
        public int Number { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string Status { get; set; } = "";
        public int BikeStands { get; set; }
        public int AvailableBikes { get; set; }
        public int AvailableStands { get; set; }
        public string LastUpdate { get; set; } = "";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
