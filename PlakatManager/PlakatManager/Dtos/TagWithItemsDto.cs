namespace ElectionMaterialManager.Dtos
{
    public class TagWithItemsDto: TagDto
    {
        public IEnumerable<ElectionItemDto> Items { get; set; }
    }
}
