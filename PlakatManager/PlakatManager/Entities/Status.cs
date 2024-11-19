namespace ElectionMaterialManager.Entities
{
    public class Status
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public List<ElectionItem> ElectionItems { get; set; } = [];
    }
}
