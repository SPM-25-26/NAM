namespace DataInjection.DTOs
{
    public class NearestCarParkDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? Address { get; set; }
        public double Distance { get; set; }
    }
}