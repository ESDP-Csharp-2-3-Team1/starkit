using System;

namespace Starkit.Models
{
    public class Item
    {
        public string Id { get; set; }
        public Dish Dish { get; set; }
        public Menu Menu { get; set; }
        public Stock Stock { get; set; }
        public int Quantity { get; set; }
    }
}