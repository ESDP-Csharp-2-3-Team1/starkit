using System;

namespace Starkit.Models
{
    public class OrdersDishes
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string DishId { get; set; }
        public virtual Dish Dish { get; set; }
        public int Quantity { get; set; }
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}