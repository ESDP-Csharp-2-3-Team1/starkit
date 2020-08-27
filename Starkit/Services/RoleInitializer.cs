using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Starkit.Models;

namespace Starkit.Services
{
    public enum Roles
    {
        SuperAdmin,
        Registrant,
        ContentManager,
        AdministratorRestaurant
    }
    public static class RoleInitializer
    {
        public static async Task Initialize(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            
            var roles = new[] { Roles.SuperAdmin,Roles.Registrant,Roles.ContentManager,Roles.AdministratorRestaurant};
            foreach (var value in roles)
            {
                string role = Convert.ToString(value);
                if (await roleManager.FindByNameAsync(role) is null)
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            string adminEmail = "admin@admin.com";
            string adminPassword = "Admin123";
            if (await userManager.FindByEmailAsync(adminEmail) is null)
            {
                User admin = new User
                {
                    Email = adminEmail,
                    EmailConfirmed = true,
                    UserName = "Admin",
                    Name = "SuperAdmin"
                };
                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin,Convert.ToString(Roles.SuperAdmin));
            }

        }
    }
}