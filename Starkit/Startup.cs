using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.Services;

namespace Starkit
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IPasswordValidator<User>, CustomPasswordValidator>(
                server => new CustomPasswordValidator(8));
            
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddTransient<UploadService>();
            services.AddDbContext<StarkitContext>(options => options.UseNpgsql(connection)
                    .UseLazyLoadingProxies())
                .AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 8; // минимальная длина
                    options.Password.RequireNonAlphanumeric = false; // требуются ли не алфавитно-цифровые символы
                    options.Password.RequireLowercase = true; // требуются ли символы в нижнем регистре
                    options.Password.RequireUppercase = true; // требуются ли символы в верхнем регистре
                    options.Password.RequireDigit = true; // требуются ли цифры
                })
                .AddEntityFrameworkStores<StarkitContext>()
                .AddDefaultTokenProviders();
            services.AddControllersWithViews();
            services.Configure<AppOptions>(Configuration);
            services.AddSingleton<IRecaptchaService, GoogleRecaptchaService>();
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(1));
            services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromDays(1));
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Initial}/{id?}");
            });
        }
    }
}