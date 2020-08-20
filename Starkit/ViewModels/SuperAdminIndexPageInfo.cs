using System;
using System.Collections.Generic;
using Starkit.Models;

namespace Starkit.ViewModels
{
    public class SuperAdminIndexPageInfo
    {
        public int PageNumber { get; set; } // номер текущей страницы
        public int PageSize { get; set; } // кол-во объектов на странице
        public int TotalItems { get; set; } // всего объектов
        public int TotalPages  // всего страниц
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
    public class SuperAdminIndexViewModel
    {
        public List<User> Users { get; set; }
        public User User { get; set; }
        public SuperAdminIndexPageInfo PageInfo { get; set; }
    }
}