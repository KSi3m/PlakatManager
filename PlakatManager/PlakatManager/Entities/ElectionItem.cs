namespace ElectionMaterialManager.Entities
{
    public class Billboard : ElectionItem
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class Poster : ElectionItem { 
        public string PaperType { get; set; }
    }

    public class LED: ElectionItem
    {
        public int RefreshRate { get; set; }
        public string Resolution { get; set; }
    }


    public abstract class ElectionItem
    {
        public int Id { get; set; }

        public string Area { get; set; }
        public double Latitude   { get; set; }
        public double Longitude { get; set; }
        public int Priority { get; set; }
        public string Size { get; set; }

        public decimal Cost { get; set; }

        public Status Status { get; set; } 
        public int StatusId { get; set; }

        public List<Comment> Comments { get; set; } = [];

        public User Author { get; set; }
        public int AuthorId { get; set; }

        public List<Tag> Tags { get; set; }

       
        

    }
}
