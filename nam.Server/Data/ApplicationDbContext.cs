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

        // Public events entities
        public DbSet<PublicEventCard> PublicEventCards { get; set; } = null!;
        public DbSet<PublicEventMobileDetail> PublicEventMobileDetails { get; set; } = null!;
        public DbSet<Offer> Offers { get; set; } = null!;
        public DbSet<Organizer> Organizers { get; set; } = null!;
        public DbSet<MunicipalityForLocalStorageSetting> MunicipalityForLocalStorageSettings { get; set; } = null!;

        // Article entities
        public DbSet<ArticleCard> ArticleCards { get; set; } = null!;
        public DbSet<ArticleDetail> ArticleDetails { get; set; } = null!;
        public DbSet<Paragraph> Paragraphs { get; set; } = null!;
        public DbSet<Nature> Natures { get; set; } = null!;

        // Municipality entities
        //public DbSet<MunicipalityCard> MunicipalityCards { get; set; } = null!;
        //public DbSet<MunicipalityHomeInfo> MunicipalityHomeInfos { get; set; } = null!;
        //public DbSet<MunicipalityHomeContactInfo> MunicipalityHomeContactInfos { get; set; } = null!;

        // Organization entities
        public DbSet<OrganizationCard> OrganizationCards { get; set; } = null!;
        public DbSet<OrganizationMobileDetail> OrganizationMobileDetails { get; set; } = null!;
        public DbSet<OwnedPoi> OwnedPois { get; set; } = null!;

    }
}
