namespace ElectionMaterialManager.Dtos
{
    public class LocationDto
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    

        public string? District { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? Description { get; set; }
    }
}
