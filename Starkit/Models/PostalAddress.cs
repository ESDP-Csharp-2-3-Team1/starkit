using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Starkit.Models
{
    public class PostalAddress
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        public int Index { get; set; }
        public string Country { get; set; } = "Казахстан";
        public string Region { get; set; }
        public string City { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        public string Address { get; set; }

        public string UserId { get; set; }
        
    }
}