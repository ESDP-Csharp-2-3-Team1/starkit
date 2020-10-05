using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Starkit.Models.Data
{
    public class StarkitContext : IdentityDbContext<User>
    {
        public override DbSet<User> Users { get; set; }
        public DbSet<PostalAddress> PostalAddresses { get; set; }
        public DbSet<LegalAddress> LegalAddresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<MenuDish> MenuDishes  { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingTable> BookingTables { get; set; }
        public DbSet<DataSiteCard> DataSiteCards { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrdersProducts { get; set; }
        public StarkitContext(DbContextOptions<StarkitContext> options) : base(options) {}
    }
}