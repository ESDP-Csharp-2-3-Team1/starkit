
using System.ComponentModel.DataAnnotations.Schema;

namespace Starkit.Models
{
    public class DataSiteCard 
    {
        public int Id { get; set; }
        public string ImgPathCarousel1 { get; set; }
        public string ImgPathCarousel2 { get; set; }
        public string ImgPathCarousel3 { get; set; }

        public string DishNameCarousel1 { get; set; }
        public string DishNameCarousel2 { get; set; }
        public string DishNameCarousel3 { get; set; }
        
        public string DishTextCarousel1 { get; set; }
        public string DishTextCarousel2 { get; set; }
        public string DishTextCarousel3 { get; set; }
        
        
        public string RestaurantId { get; set; }
        [NotMapped]
        public Restaurant Restaurant { get; set; }


    }
}