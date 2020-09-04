using Starkit.Models;

namespace Starkit.ViewModels
{
    public class BookTableViewModel
    {
        public Booking Booking { get; set; }
        public Table Table { get; set; }
        public string RestaurantId { get; set; }
    }
}