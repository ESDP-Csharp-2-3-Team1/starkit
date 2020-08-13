using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Starkit.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string NameRestaurant { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string ContactPerson { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Address { get; set; }
        public string RestaurantInformation { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public bool DomainAvailability { get; set; }
        public string DomainName { get; set; }
        public string LogoPath { get; set; }
        public string UserId { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }

        
    }
}