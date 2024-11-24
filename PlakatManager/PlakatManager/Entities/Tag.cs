using System.Text.Json.Serialization;

namespace ElectionMaterialManager.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Value {  get; set; }
        //[JsonIgnore]
        public List<ElectionItem> ElectionItems { get; set; } 
    }
}
