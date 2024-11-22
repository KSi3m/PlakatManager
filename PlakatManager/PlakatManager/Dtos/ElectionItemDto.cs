namespace ElectionMaterialManager.Dtos
{
    public class ElectionItemDto
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
    }
}
