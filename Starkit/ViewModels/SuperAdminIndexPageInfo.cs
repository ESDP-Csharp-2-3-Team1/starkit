using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Starkit.Models;

namespace Starkit.ViewModels
{
    public class SuperAdminIndexPageInfo
    {
        public int PageNumber { get; set; } // номер текущей страницы
        public int PageSize { get; set; } // кол-во объектов на странице
        public int TotalItems { get; set; } // всего объектов
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize); // всего страниц
    }
    public class SuperAdminIndexViewModel
    {
        public List<User> Users { get; set; }
        public User User { get; set; }
        public SuperAdminIndexPageInfo PageInfo { get; set; }
    }
    public class EmployeeIndexViewModel
    {
        public List<User> Users { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public SuperAdminIndexPageInfo PageInfo { get; set; }
    }
}