using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Starkit.Models
{
    public enum DeliveryMethod
    {
        Самовывоз,
        Доставка
    }
    public enum Status    
    {
        Новая,
        Подтвержден,
        Отказ,
        Доставке,
        Доставлен
    }
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Address { get; set; }

        public string Comment { get; set; }

        public DateTime OrderTime { get; set; }
        
        public DeliveryMethod DeliveryMethod { get; set; }
        
        public Status Status { get; set; }

        public int OrderNum { get; set; }

        public string RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }

        public virtual List<OrdersDishes> OrdersDishes { get; set; }
        public virtual List<OrdersMenu> OrdersMenu { get; set; }
        public virtual List<OrdersStocks> OrdersStocks { get; set; }
    }
}