using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Starkit.ViewModels
{
    public class Login
    {
        [EmailAddress(ErrorMessage = "Пожалуйста, введите действительный адрес электронной почты.")]
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage = "Пароль должен содержать не менее 8 символов.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        [StringLength(4)]
        public string CaptchaCode { get; set; }
    }
}