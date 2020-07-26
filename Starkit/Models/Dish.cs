using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Starkit.Models
{
    public class Dish
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public decimal Cost { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Description { get; set; }
        
        public string Avatar { get; set; }
        
        [NotMapped]
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public IFormFile File { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public double Calorie { get; set; }

        public bool ProperNutrition { get; set; }

        public bool Vegetarian { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        [NotMapped]
        public List<Category> Categories { get; set; }
    }
}