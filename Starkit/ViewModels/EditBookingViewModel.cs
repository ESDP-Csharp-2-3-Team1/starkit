using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Starkit.Models;

namespace Starkit.ViewModels
{
    public class EditBookingViewModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Date { get; set; }
        [Required]
        public string BookFrom { get; set; }
        [Required]
        public string BookTo { get; set; }
        public virtual List<BookingTable> BookingTables { get; set; }
        public virtual List<Table> Tables { get; set; }
        [Required]
        public string ClientName { get; set; }
        [Required]
        public string ClientSurname { get; set; }
        [Required]
        public int Pax { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string EditorId { get; set; }
        public virtual User User { get; set; }
        public BookingStatus State { get; set; } = BookingStatus.Pending;
        public string Comment { get; set; }
        
    }
}