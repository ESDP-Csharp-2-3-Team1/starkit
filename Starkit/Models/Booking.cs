using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

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
        public string Date { get; set; }
        [Required(ErrorMessage = "Выберите время")]
        public string BookFrom { get; set; }
        [Required(ErrorMessage = "Выберите время")]
        [Remote("CheckTime", "Validation", ErrorMessage = "Ошибка ввода", AdditionalFields = "BookFrom")]
        public string BookTo { get; set; }
        public virtual List<BookingTable> BookingTables { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string ClientName { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Remote("CheckTableCapacity", "Validation", ErrorMessage = "Недостаточно мест, выберите другой столик или забронируйте еще один", AdditionalFields = "TableId")]
        public int Pax { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
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