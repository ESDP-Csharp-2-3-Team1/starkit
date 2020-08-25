using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Starkit.Models
{
    public class Restaurant
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string NameRestaurant { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string ContactPerson { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Address { get; set; }
        public string RestaurantInformation { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public bool DomainAvailability { get; set; }
        public string DomainName { get; set; }
        public string DomainRegistrar { get; set; }
        public string LogoPath { get; set; }
        public string WorkSchedule { get; set; }
        public int TotalNumberSeats { get; set; }
        public int AvailableNumberSeats { get; set; }
        public string OrderConditions { get; set; }
        public string UserId { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }

        public virtual List<Category> Categories { get; set; }
        public virtual List<SubCategory> SubCategories { get; set; }
        public virtual List<Dish> Dishes { get; set; }
        public virtual List<Menu> Menu { get; set; }
        public virtual List<Stock> Stocks { get; set; }
    }
}