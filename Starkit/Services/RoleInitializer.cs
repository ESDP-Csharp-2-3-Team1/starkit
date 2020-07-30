using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Starkit.Models;

namespace Starkit.Services
{
    public static class RoleInitializer
    {
        public static async Task Initialize(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            
            var roles = new[] { "admin","restaurateur","employee"};
            foreach (var role in roles)
            {
                if (await roleManager.FindByNameAsync(role) is null)
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}