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
        public string Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Description { get; set; }
        
        public IFormFile File { get; set; }
    }
}