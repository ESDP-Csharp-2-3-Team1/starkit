using System;

namespace Starkit.Models
{
    public class OrdersMenu
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string MenuId { get; set; }
        public virtual Menu Menu { get; set; }
        public int Quantity { get; set; }
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}