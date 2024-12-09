namespace ElectionMaterialManager.Dtos
{
    public class ElectionItemDto
    {
        public int Id { get; set; }

        public string Type { get; set; }

    
        public string Status { get; set; }

        public LocationDto Location { get; set; }
        public int Priority { get; set; }

    }
}
