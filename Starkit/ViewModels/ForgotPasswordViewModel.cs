using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Starkit.ViewModels
{
    public class ForgotPasswordViewModel
    {
        
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        [EmailAddress(ErrorMessage = "Пожалуйста, введите действительный адрес электронной почты.")]
        [Remote("VerifyingEmailAuthenticity","Validation",ErrorMessage = "Электронный адрес не зарегистрирован.")]
        public string Email { get; set; }
    }
}