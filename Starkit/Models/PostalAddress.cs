using System.ComponentModel.DataAnnotations;

namespace Starkit.Models
{
    public class PostalAddress
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string Country { get; set; } = "Казахстан";
        public string Region { get; set; }
        public string City { get; set; }
        [Required(ErrorMessage = "Это поле необходимо заполнить.")]
        public string Address { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}