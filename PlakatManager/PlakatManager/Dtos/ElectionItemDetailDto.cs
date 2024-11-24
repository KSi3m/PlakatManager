using System.ComponentModel;

namespace ElectionMaterialManager.Dtos
{
    public class ElectionItemDetailDto: ElectionItemDto
    { 
       
        public string? Size { get; set; }

        public decimal? Cost { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }

        public AuthorDto Author { get; set; }

        public string? PaperType { get; set; }
        public int? RefreshRate { get; set; }
        public string? Resolution { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
