using System.ComponentModel.DataAnnotations;

namespace Starkit.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage = "Пароль должен содержать не менее 8 символов.")]
        public string Password { get; set; }
 
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Пароли не совпадают.")]
        [MinLength(6,ErrorMessage = "Пароль должен содержать не менее 8 символов.")]
        public string ConfirmPassword { get; set; }
 
        public string Email { get; set; }
        public string Token { get; set; }
    }
}