﻿namespace crud
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public float Price { get; set; }
        public int CreatedAt { get; set; }
    }
}
