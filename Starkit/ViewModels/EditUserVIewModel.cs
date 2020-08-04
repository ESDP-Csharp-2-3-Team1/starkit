using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Starkit.Models;

namespace Starkit.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string SurName { get; set; }
        public string CompanyName { get; set; }
        public string IIN { get; set; }
        public string PhoneNumber { get; set; }
        public string CityPhone { get; set; }
        public LegalAddress LegalAddress { get; set; }
        public PostalAddress PostalAddress { get; set; }

        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        [DataType(DataType.Password)]
        [Remote("CheckOldPassword", "Validation", ErrorMessage = "Введите старый пароль")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        [DataType(DataType.Password)]
        [Remote("ComparePasswords", "Validation", ErrorMessage = "Новый пароль должен отличаться от старого.", AdditionalFields = "OldPassword")]
        [MinLength(8,ErrorMessage = "Пароль должен содержать не менее 8 символов.")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают.")]
        [MinLength(8,ErrorMessage = "Пароль должен содержать не менее 8 символов.")]
        public string ConfirmPassword { get; set; }
    }
}