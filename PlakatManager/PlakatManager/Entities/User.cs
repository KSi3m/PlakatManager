namespace PlakatManager.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName {  get; set; }
        public string LastName {  get; set; }
        public string FullName { get; set; } 
        public string Email {  get; set; }
        public Address Address { get; set; }

        public List<ElectionItem> ElectionItems { get; set; } = [];
        public List<Comment> Comments { get; set; } = [];

    }
}
