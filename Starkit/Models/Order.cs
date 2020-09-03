using System;
using System.ComponentModel.DataAnnotations;

namespace Starkit.Models
{
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Address { get; set; }

        public string Comment { get; set; }

        public DateTime OrderTime { get; set; }

        public string RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}