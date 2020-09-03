using System;

namespace Starkit.Models
{
    public class OrdersStocks
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string StockId { get; set; }
        public virtual Stock Stock { get; set; }
        public int Quantity { get; set; }
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}