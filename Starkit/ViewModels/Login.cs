using System.ComponentModel.DataAnnotations;

namespace Starkit.ViewModels
{
    public class Login
    {
        [EmailAddress(ErrorMessage = "Пожалуйста, введите действительный адрес электронной почты.")]
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        [MinLength(3,ErrorMessage = "Минимальная длина пароля 3 символа")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}