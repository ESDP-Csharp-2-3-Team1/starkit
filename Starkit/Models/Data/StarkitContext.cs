using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Starkit.Models.Data
{
    public class StarkitContext : IdentityDbContext<User>
    {
        
    }
}