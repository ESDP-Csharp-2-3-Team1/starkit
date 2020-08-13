using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Starkit.Models
{
    public class Menu
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        public string CreatorId { get; set; }
        public virtual User Creator { get; set; }
        
        public string EditorId { get; set; }
        public virtual User Editor { get; set; }

        public DateTime AddTime { get; set; }
        public DateTime? EditTime { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public decimal? Cost { get; set; }
        
        [NotMapped]
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public IFormFile File { get; set; }

        public string Avatar { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Type { get; set; }
        
        public virtual List<MenuDish> MenuDishes  { get; set; }
    }
}