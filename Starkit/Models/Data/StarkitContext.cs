using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Starkit.Models.Data
{
    public class StarkitContext : IdentityDbContext<User>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        
        public StarkitContext(DbContextOptions<StarkitContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Category>().HasData(
                new Category[]
                {
                    new Category{Id = Guid.NewGuid().ToString(), Name = "Первые блюда"}, 
                    new Category{Id = Guid.NewGuid().ToString(), Name = "Вторые блюда"}, 
                    new Category{Id = Guid.NewGuid().ToString(), Name = "Десерты"}, 
                    new Category{Id = Guid.NewGuid().ToString(), Name = "Напитки"} 
                });
        }
    }
}