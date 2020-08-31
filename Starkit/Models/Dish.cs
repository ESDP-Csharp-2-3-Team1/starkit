using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Starkit.Models
{
    public class Dish
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string CategoryId { get; set; }
        [JsonIgnore]
        public virtual Category Category { get; set; }
        public string SubCategoryId { get; set; }
        [JsonIgnore]
        public virtual SubCategory SubCategory { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Remote("CheckNameDish", "Validation", ErrorMessage = "Такое блюдо уже существует")]
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

        public bool Visibility { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Ingredients { get; set; }
        public DateTime AddTime { get; set; }
        public DateTime? EditTime { get; set; }
        public string CreatorId { get; set; }
        [JsonIgnore]
        public virtual User Creator { get; set; }
        public string EditorId { get; set; }
        [JsonIgnore]
        public virtual User Editor { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<Category> Categories { get; set; }
        [NotMapped]
        [JsonIgnore]
        public  List<SubCategory> SubCategories { get; set; }
        [JsonIgnore]
        public virtual List<MenuDish> MenuDish { get; set; }
        [NotMapped]
        [JsonIgnore]
        public List<Menu> Menu { get; set; }
        
        public string RestaurantId { get; set; }
        [JsonIgnore]
        public virtual Restaurant Restaurant { get; set; }
    }
}