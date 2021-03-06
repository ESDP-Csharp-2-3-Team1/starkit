﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string RestaurantInformation { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public bool DomainAvailability { get; set; }
        [RegularExpression("^((?!-)[A-Za-z0-9-]{1,63}(?<!-)\\.)+[A-Za-z]{2,6}$", ErrorMessage = "Неверное доменное имя. Попробуйте еще раз.")]
        [Remote("CheckDomain", "Validation", ErrorMessage = "Этот домен уже зарегестрирован в приложении")]
        public string DomainName { get; set; }
        public string DomainRegistrar { get; set; }
        public string LogoPath { get; set; }
        public string WorkSchedule { get; set; }
        public int TotalNumberSeats { get; set; }
        public int AvailableNumberSeats { get; set; }
        public string OrderConditions { get; set; }
        public string BookingTerms { get; set; }
        public string GoogleMapsApi { get; set; }
        public string UserId { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }

        public virtual List<Category> Categories { get; set; }
        public virtual List<SubCategory> SubCategories { get; set; }
        public virtual List<Dish> Dishes { get; set; }
        public virtual List<Menu> Menu { get; set; }
        public virtual List<Stock> Stocks { get; set; }
        public virtual List<Table> Tables { get; set; }

        [NotMapped]
        public IEnumerable<IGrouping<Category,Dish>> DishesGroup { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}