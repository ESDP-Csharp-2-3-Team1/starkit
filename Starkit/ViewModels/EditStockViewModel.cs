using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Starkit.Models;

namespace Starkit.ViewModels
{
    public class EditStockViewModel
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "Не все поля блюд заполнены")]
        [Remote("CheckNameStock", "Validation", ErrorMessage = "Акция с таким содержимом уже существует", 
            AdditionalFields = nameof(Id))]
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
        
        public IFormFile File { get; set; }
        
        public string FirstDishId { get; set; }

        public string SecondDishId { get; set; }

        public string ThirdDishId { get; set; }
    }
}