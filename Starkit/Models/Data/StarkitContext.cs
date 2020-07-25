using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Starkit.Models.Data
{
    public class StarkitContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<PostalAddress> PostalAddresses { get; set; }
        public DbSet<LegalAddress> LegalAddresses { get; set; }
        public StarkitContext(DbContextOptions options) : base(options)
        {
        }
    }
}