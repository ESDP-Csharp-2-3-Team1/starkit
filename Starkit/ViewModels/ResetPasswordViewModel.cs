using System.ComponentModel.DataAnnotations;

namespace Starkit.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
 
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
 
        public string Email { get; set; }
        public string Token { get; set; }
    }
}