using Microsoft.AspNetCore.Identity;

namespace Starkit.Models
{
    public class User : IdentityUser
    {
        public string Login { get; set; }
        public string SurName { get; set; }
        public string CompanyName { get; set; }
        public int IIN { get; set; }
        public string CityPhone { get; set; }
    }
}