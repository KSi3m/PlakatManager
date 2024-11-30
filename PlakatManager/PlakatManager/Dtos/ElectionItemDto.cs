namespace ElectionMaterialManager.Dtos
{
    public class ElectionItemDto
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string Area { get; set; }
        public string Status { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Priority { get; set; }

    }
}
