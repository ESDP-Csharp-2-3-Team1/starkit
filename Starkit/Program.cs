using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Starkit.Models;
using Starkit.Services;

namespace Starkit
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var userManage = services.GetRequiredService<UserManager<User>>();
                var roleManage = services.GetRequiredService<RoleManager<IdentityRole>>();
                await RoleInitializer.Initialize(userManage,roleManage);
            }
            catch (Exception e)
            {
                // var logger = services.GetRequiredService<ILogger<Program>();
                // logger.LogError(e, "Возникло исключеие при инициализации ролей");
                Console.WriteLine(e.Message);
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}