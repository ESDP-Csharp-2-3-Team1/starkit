using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Starkit.Models;

namespace Starkit.ViewModels
{
    public class CreateBookingViewModel
    {
        public int TableId { get; set; }
        public string Date { get; set; }
        [Required(ErrorMessage = "Выберите время")]
        public string BookFrom { get; set; }
        [Required(ErrorMessage = "Выберите время")]
        public string BookTo { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [MinLength(2, ErrorMessage = "Минимальное количество символов 2")]
        public string ClientName { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Range(1, 10, ErrorMessage = "Введите количество от 1 до 10")]
        [Remote("CheckTableCapacity", "Validation", ErrorMessage = "Недостаточно мест, выберите другой столик или забронируйте еще один", AdditionalFields = "TableId")]
        public int Pax { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Введите корректный номер телефона")]
        public string PhoneNumber { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "Введите корректный email")]
        public string Email { get; set; }
        public string Comment { get; set; }
        [Required(ErrorMessage = "Выберите дату")]
        public string CustomDate { get; set; }
        
    }
}