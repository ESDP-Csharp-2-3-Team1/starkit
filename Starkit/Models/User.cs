using Microsoft.AspNetCore.Identity;

namespace Starkit.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string CompanyName { get; set; }
        public string IIN { get; set; }
        public string CityPhone { get; set; }
        public string AvatarPath { get; set; }
    }
}