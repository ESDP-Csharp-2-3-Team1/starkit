
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
        
        public string ImgPathSpecialOffers { get; set; } = "images/SiteCardDefaultPath/default-especial-dishes-back.jpg";
        public string SpecialOffersTitle { get; set; } = "Акции";
        public string SpecialOffersSubtitle { get; set; } = "Не пропустите";
        public string ImgPathMenu { get; set; } = "images/SiteCardDefaultPath/menu-back.jpg";
        public string MenuTitle { get; set; } = "Специальные меню";
        public string MenuSubtitle { get; set; } = "На любой вкус";
        public string ImgPathDishes { get; set; } = "images/SiteCardDefaultPath/default-foody_bg-1.jpg";
        public string DishesTitle { get; set; } = "Наши блюда";
        public string DishesSubtitle { get; set; } = "Выбор шеф-повара";
        public string ImgPathBooking { get; set; } = "images/SiteCardDefaultPath/table-reservation2.jpg";
        public string BookingTitle { get; set; } = "Забронировать столик";
        public string BookingSubtitle { get; set; } = "Мы вас ждем";
        public string RestaurantId { get; set; }
        [NotMapped]
        public Restaurant Restaurant { get; set; }


    }
}