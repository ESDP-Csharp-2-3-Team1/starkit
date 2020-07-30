using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Starkit.Models;

namespace Starkit.ViewModels
{
    public class FilterViewModel
    {
        public FilterViewModel(List<Category> categories, string category, string name)
        {
            categories.Insert(0, new Category{Name = "Все", Id = null});
            Categories = new SelectList(categories, "Id", "Name", category);
            SelectedCategory = category;
            SelectedName = name;
        }
        
        public SelectList Categories { get; set; }
        public string SelectedCategory { get; set; }
        public string SelectedName { get; set; }
    }
}