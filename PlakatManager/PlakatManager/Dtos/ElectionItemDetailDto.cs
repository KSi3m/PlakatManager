using System.ComponentModel;

namespace ElectionMaterialManager.Dtos
{
    public class ElectionItemDetailDto: ElectionItemDto
    { 
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? Priority { get; set; }
        public string? Size { get; set; }

        public decimal? Cost { get; set; }

        public int StatusId { get; set; }

        public int AuthorId { get; set; }

        public string? PaperType { get; set; }
        public int? RefreshRate { get; set; }
        public string? Resolution { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
