using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Starkit.Models;

namespace Starkit.ViewModels
{
    public class EditMenuViewModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public decimal Cost { get; set; }
        
        [NotMapped]
        public IFormFile File { get; set; }

        public string Avatar { get; set; }

        [NotMapped] 
        public IEnumerable<IGrouping<Category, Dish>> Dishes { get; set; }

        [NotMapped]
        public List<string> DishesId { get; set; }
    }
}