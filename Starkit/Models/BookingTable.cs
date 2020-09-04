using System;

namespace Starkit.Models
{
    public class BookingTable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        public string BookingId { get; set; }
        public virtual Booking Booking { get; set; }

        public int TableId { get; set; }
        public virtual Table Table { get; set; }
        public bool IsDeleted { get; set; }
    }
}