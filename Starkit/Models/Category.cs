using System;
using System.ComponentModel.DataAnnotations;

namespace Starkit.Models
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Name { get; set; }
    }
}