using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Starkit.ViewModels
{
    public class EditSubCategoryViewModel
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Remote("CheckNameSubCategory", "Validation", ErrorMessage = "Такая подкатегория уже существует", 
            AdditionalFields = nameof(Id))]
        public string Name { get; set; }
    }
}