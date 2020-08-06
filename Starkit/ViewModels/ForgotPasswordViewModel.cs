using System.ComponentModel.DataAnnotations;

namespace Starkit.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}