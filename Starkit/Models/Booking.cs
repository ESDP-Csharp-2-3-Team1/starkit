using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Starkit.Models
{
    public enum BookingStatus
    {
        Pending,
        Approved,
        Done,
        Cancelled,
        NoShow,
        Late
    }
    public class Booking
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required] 
        public DateTime DateTime { get; set; }
        public virtual List<BookingTable> BookingTables { get; set; }
        [Required]
        public string ClientName { get; set; }
        [Required]
        public int Pax { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public string CreatorId { get; set; }
        public virtual User Creator { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string EditorId { get; set; }
        public virtual User Editor { get; set; }
        public DateTime EditedDate { get; set; }
        public BookingStatus State { get; set; } = BookingStatus.Pending;
        public string Comment { get; set; }
    }
}