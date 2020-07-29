using System;

namespace Starkit.Models
{
    public class MenuDish
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        public string MenuId { get; set; }
        public virtual Menu Menu { get; set; }

        public string DishId { get; set; }
        public virtual Dish Dish { get; set; }
    }
}