using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Starkit.Models;

namespace Starkit.ViewModels
{
    public class Register
    {
        
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        [EmailAddress(ErrorMessage = "Пожалуйста, введите действительный адрес электронной почты.")]
        [Remote("CheckEmail","Validation",ErrorMessage = "Данный электронный адрес используется другим аккаунтом.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        public string SurName { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        [Remote("CheckIIN","Validation",ErrorMessage = "ИИН должен состоять только из цифр.")]
        [MinLength(12,ErrorMessage = "Минимальная длина 12 символов.")]
        [MaxLength(12,ErrorMessage = "Максимальная длина 12 символов.")]
        public string IIN { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        public string PhoneNumber { get; set; }
        public string CityPhone { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage = "Пароль должен содержать не менее 8 символов.")]
        
        public string Password { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Пароли не совпадают.")]
        [MinLength(6,ErrorMessage = "Пароль должен содержать не менее 8 символов.")]
        public string ConfirmPassword { get; set; }
        public LegalAddress LegalAddress { get; set; }
        public PostalAddress PostalAddress { get; set; }
    }
}