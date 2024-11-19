using System.ComponentModel;

namespace ElectionMaterialManager.Dtos
{
    public class BillboardRequestDTO
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

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
