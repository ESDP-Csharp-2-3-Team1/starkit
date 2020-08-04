using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Starkit.ViewModels
{
    public class EditCategoryViewModel
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Remote("CheckNameCategory", "Validation", ErrorMessage = "Такая категория уже существует", 
            AdditionalFields = nameof(Id))]
        public string Name { get; set; }
    }
}