using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Starkit.Models;

namespace Starkit.ViewModels
{
    public class CreateBookingViewModel
    {
        public int TableId { get; set; }
        [Required(ErrorMessage = "Выберите дату")]
        public string Date { get; set; }
        [Required(ErrorMessage = "Выберите дату")]
        public string BookFrom { get; set; }
        [Required(ErrorMessage = "Выберите дату")]
        public string BookTo { get; set; }
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
        public string Comment { get; set; }
        
    }
}