using System.ComponentModel.DataAnnotations.Schema;
using Starkit.Models;

namespace Starkit.ViewModels
{
    public class EmployeeViewModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public EmployeePosition Position { get; set; }
       
        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        public string ConfirmPassword { get; set; }
        
    }
}