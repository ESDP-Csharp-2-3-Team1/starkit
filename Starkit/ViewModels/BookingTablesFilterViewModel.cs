using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Starkit.Models;
using Starkit.Models.Data;

namespace Starkit.ViewModels
{
    public class BookingTablesFilterViewModel
    {
        public BookingTablesFilterViewModel(string item, int name)
        {
            SelectedItem = item;
            SelectedName = name;
        }
        public string SelectedItem { get; set; }
        public int SelectedName { get; set; }
    }
}