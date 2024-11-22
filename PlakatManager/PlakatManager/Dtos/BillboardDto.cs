namespace ElectionMaterialManager.Dtos
{
    public class BillboardDto
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public List<TagDto> Tags { get; set; }
    }
}
