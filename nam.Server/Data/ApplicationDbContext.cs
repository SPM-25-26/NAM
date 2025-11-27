using Microsoft.EntityFrameworkCore;
using nam.Server.Models.Entities;

namespace nam.Server.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        public DbSet<User> Users{ get; set; }

        public DbSet<PasswordResetCode> ResetPasswordAuth{ get; set; }

        public DbSet<RevokedToken> RevokedTokens { get; set; } = null!;

    }
}
