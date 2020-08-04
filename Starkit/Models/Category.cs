using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Starkit.Models
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public virtual List<Dish> Dishes { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Remote("CheckNameCategory", "Validation", ErrorMessage = "Такая категория уже существует")]
        public string Name { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? EditedTime { get; set; }
    }
}