using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Starkit.Models
{
    public class SubCategory
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Remote("CheckNameSubCategory", "Validation", ErrorMessage = "Такая подкатегория уже существует")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public DateTime CreateTime { get; set; }
        
        public DateTime? EditedTime { get; set; }

        [NotMapped]
        public List<Category> Categories { get; set; }
        
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}