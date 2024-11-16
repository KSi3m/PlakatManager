namespace ElectionMaterialManager.Entities
{
    public class ElectionItemTag
    {
        public ElectionItem ElectionItem { get; set; }  
        public int ElectionItemId { get; set; } 

        public Tag Tag { get; set; }
        public int TagId { get; set; }

        public DateTime DateOfPublication { get; set; } 
    }
}
