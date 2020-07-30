using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Starkit.Models;

namespace Starkit.ViewModels
{
    public class EditDishViewModel
    {
        public string Id { get; set; }
        public virtual Category Category { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public decimal Cost { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Description { get; set; }

        public IFormFile File { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public double Calorie { get; set; }

        public bool ProperNutrition { get; set; }
        
        public bool Vegetarian { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Ingredients { get; set; }
    }
}