using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Cms;

namespace Starkit.Models
{
    public class Stock
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required(ErrorMessage = "Не все поля блюд заполнены")]
        [Remote("CheckNameStock", "Validation", ErrorMessage = "Акция с таким содержимом уже существует")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Type { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Validity { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public DateTime At { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public DateTime To { get; set; }
        
        public string Avatar { get; set; }
        
        [NotMapped]
        [System.Text.Json.Serialization.JsonIgnore]
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public IFormFile File { get; set; }
        
        public string CreatorId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual User Creator { get; set; }
        
        public string EditorId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual User Editor { get; set; }

        public string FirstDishId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Dish FirstDish { get; set; }
        
        public string SecondDishId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Dish SecondDish { get; set; }
        
        public string ThirdDishId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Dish ThirdDish { get; set; }
        
        public string RestaurantId { get; set; }
        [JsonIgnore]
        public virtual Restaurant Restaurant { get; set; }

        public decimal Cost { get; set; }
    }
}