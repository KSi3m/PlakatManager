﻿namespace PlakatManager.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Value {  get; set; }

        public List<ElectionItem> ElectionItems { get; set; } 
    }
}
