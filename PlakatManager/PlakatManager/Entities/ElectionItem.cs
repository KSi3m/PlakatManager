﻿namespace ElectionMaterialManager.Entities
{
    public class Billboard : ElectionItem
    {
        public int Height { get; set; }
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

        public Location Location { get; set; }

    
        public int Priority { get; set; }
        public string Size { get; set; }

        public decimal Cost { get; set; }

        public Status Status { get; set; } 
        public int StatusId { get; set; }

        public List<Comment> Comments { get; set; } = [];

        public User Author { get; set; }
        public string AuthorId { get; set; }

        public List<Tag> Tags { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


    }

    public class Location
    {

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string? District { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? Description { get; set; }

    }

}
