using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Starkit.Models
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public virtual List<Dish> Dishes { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Name { get; set; }
    }
}