using System.ComponentModel.DataAnnotations;
using Starkit.Models;

namespace Starkit.ViewModels
{
    public class Register
    {
        public string Login { get; set; }
        [Required(ErrorMessage = "Поля обязательные для заполнения")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Поля обязательные для заполнения")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Поля обязательные для заполнения")]
        public string SurName { get; set; }
        [Required(ErrorMessage = "Поля обязательные для заполнения")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Поля обязательные для заполнения")]
        public int IIN { get; set; }
        [Required(ErrorMessage = "Поля обязательные для заполнения")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Поля обязательные для заполнения")]
        public string CityPhone { get; set; }

        [Required(ErrorMessage = "Поля обязательные для заполнения")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Поля обязательные для заполнения")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
        public LegalAddress LegalAddress { get; set; }
        public PostalAddress PostalAddress { get; set; }
    }
}