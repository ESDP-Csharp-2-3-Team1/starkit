using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Starkit.Models;

namespace Starkit.ViewModels
{
    public class EditMenuViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Remote("CheckNameMenu", "Validation", ErrorMessage = "Такое меню уже существует", 
            AdditionalFields = nameof(Id))]
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