using Microsoft.EntityFrameworkCore;
using nam.Server.Models.Entities;
using nam.Server.Models.Entities.Auth;
using nam.Server.Models.Entities.MunicipalityEntities;

namespace nam.Server.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<PasswordResetCode> ResetPasswordAuth { get; set; }

        public DbSet<RevokedToken> RevokedTokens { get; set; } = null!;

        public DbSet<ArtCultureNatureCard> ArtCultureNatureCards { get; set; } = null!;
        public DbSet<ArtCultureNatureDetail> ArtCultureNatureDetails { get; set; } = null!;
        public DbSet<CulturalSiteService> CulturalSiteServices { get; set; } = null!;
        public DbSet<CulturalProject> CulturalProjects { get; set; } = null!;
        public DbSet<Catalogue> Catalogues { get; set; } = null!;
        public DbSet<CreativeWorkMobile> CreativeWorkMobiles { get; set; } = null!;
        public DbSet<FeatureCard> FeatureCards { get; set; } = null!;
        public DbSet<AssociatedService> AssociatedServices { get; set; } = null!;
        public DbSet<NearestCarPark> NearestCarParks { get; set; } = null!;
        public DbSet<SiteCard> SiteCards { get; set; } = null!;
        public DbSet<MobileCategoryDetail> MobileCategoryDetails { get; set; } = null!;
    }
}
