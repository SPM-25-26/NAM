using Microsoft.EntityFrameworkCore;

namespace nam.Server.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        public DbSet<Entities.User> Users{ get; set; }
    }

}
