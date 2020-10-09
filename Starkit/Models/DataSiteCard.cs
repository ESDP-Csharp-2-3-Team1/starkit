
using System.ComponentModel.DataAnnotations.Schema;

namespace Starkit.Models
{
    public class DataSiteCard 
    {
        public int Id { get; set; }
        public string ImgPathCarousel1 { get; set; } = "images/SiteCardDefaultPath/slide1-1-ok.jpg";
        public string ImgPathCarousel2 { get; set; } = "images/SiteCardDefaultPath/slide1-2-ok.jpg";
        public string ImgPathCarousel3 { get; set; } = "images/SiteCardDefaultPath/slide1-3-ok.jpg";

        public string DishNameCarousel1 { get; set; } = "СЭНДВИЧ НА ГРИЛЕ ШАВАРМА";
        public string DishNameCarousel2 { get; set; } = "СЭНДВИЧ НА ГРИЛЕ ШАВАРМА";
        public string DishNameCarousel3 { get; set; } = "РЕСТОРАН LEEDS RED CHILI";
        
        public string DishTextCarousel1 { get; set; } = "Praesent commodo cursus magna, vel scelerisque nisl consectetur.";
        public string DishTextCarousel2 { get; set; } = "Praesent commodo cursus magna, vel scelerisque nisl consectetur.";
        public string DishTextCarousel3 { get; set; } = "Praesent commodo cursus magna, vel scelerisque nisl consectetur.";
        
        
        public string RestaurantId { get; set; }
        [NotMapped]
        public Restaurant Restaurant { get; set; }


    }
}