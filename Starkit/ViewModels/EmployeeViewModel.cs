using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Starkit.Models;

namespace Starkit.ViewModels
{
    public class EmployeeViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        [EmailAddress(ErrorMessage = "Пожалуйста, введите действительный адрес электронной почты.")]
        [Remote("CheckEmail","Validation",ErrorMessage = "Данный электронный адрес используется другим аккаунтом.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        public bool Position { get; set; }
       
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage = "Пароль должен содержать не менее 8 символов.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage = "Пароль должен содержать не менее 8 символов.")]
        [Compare("Password",ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
        
    }
}