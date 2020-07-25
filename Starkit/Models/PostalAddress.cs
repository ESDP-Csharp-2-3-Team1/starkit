using System.ComponentModel.DataAnnotations;

namespace Starkit.Models
{
    public class PostalAddress
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поля обязательные для заполнения")]
        public int Index { get; set; }
        public string Country { get; set; } = "Казахстан";
        [Required(ErrorMessage = "Поля обязательные для заполнения")]
        public string Region { get; set; }
        [Required(ErrorMessage = "Поля обязательные для заполнения")]
        public string City { get; set; }
        [Required(ErrorMessage = "Поля обязательные для заполнения")]
        public string Address { get; set; }

        public string UserId { get; set; }
    }
}