using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Models
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        //public AppIdentityDbContext()
        //{
        //}

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) {
        
        
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

    }
}

