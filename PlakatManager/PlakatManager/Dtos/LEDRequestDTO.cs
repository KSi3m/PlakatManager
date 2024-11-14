using System.ComponentModel;

namespace PlakatManager.Dtos
{
    public class LEDRequestDTO
    {
        public string? Area { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? Priority { get; set; }
        public string? Size { get; set; }

        public decimal? Cost { get; set; }

        [DefaultValue(1)]
        public int StatusId { get; set; } 

        public int AuthorId { get; set; }

        public int? RefreshRate { get; set; }
        public string? Resolution { get; set; }
    
    }
}
