using System;

namespace Starkit.Models
{
    public class OrderProduct
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string DishId { get; set; }
        public virtual Dish Dish { get; set; }
        public string MenuId { get; set; }
        public virtual Menu Menu { get; set; }
        public string StockId { get; set; }
        public virtual Stock Stock { get; set; }
        public int Quantity { get; set; }
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}