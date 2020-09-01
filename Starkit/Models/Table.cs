using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Starkit.Models
{
    public enum TableState
    {
        Available,
        Booked
    }

    public enum Location
    {
        Window,
        Middle,
        Outdoor,
        Regular
    }
    
    public class Table
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public string IconUrl { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public IFormFile File { get; set; }
        public TableState State { get; set; } = TableState.Available;
        public string Desc { get; set; }
        public Location Location { get; set; } = Location.Regular;
        public bool IsSmoking { get; set; } = false;
        public bool IsQuiet { get; set; } = false;
        public int Floor { get; set; } = 1;
        public string RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public string CreatorId { get; set; }
        public virtual User Creator { get; set; }
        public string EditorId { get; set; }
        public virtual User Editor { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime EditedDate { get; set; }
        public virtual List<BookingTable> BookingTables { get; set; }
        public virtual List<Booking> Bookings { get; set; }
    }
}