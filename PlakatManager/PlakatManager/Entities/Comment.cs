namespace PlakatManager.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message {  get; set; }
        public User Author { get; set; }
        public int AuthorId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ElectionItem ElectionItem { get; set; }
        public int ElectionItemId {  get; set; }
           
    }
}
