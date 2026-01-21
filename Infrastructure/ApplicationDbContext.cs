using Domain.Entities;
using Domain.Entities.Auth;
using Domain.Entities.MunicipalityEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Configure MunicipalityHomeInfo relationships with FeatureCard
            builder.Entity<MunicipalityHomeInfo>()
                .HasMany(m => m.Events)
                .WithOne()
                .HasForeignKey("MunicipalityHomeInfoLegalName")
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MunicipalityHomeInfo>()
                .HasMany(m => m.ArticlesAndPaths)
                .WithOne()
                .HasForeignKey("MunicipalityHomeInfoLegalName1")
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ServiceDetail>(b =>
            {
                b.OwnsOne(x => x.OpeningHours, oh =>
                {
                    oh.OwnsOne(x => x.AdmissionType);
                    oh.OwnsOne(x => x.TimeInterval);
                });

                b.OwnsOne(x => x.TemporaryClosure, tc =>
                {
                    tc.OwnsOne(x => x.TimeInterval);
                });

                b.OwnsOne(x => x.Booking, bk =>
                {
                    bk.OwnsOne(x => x.TimeIntervalDto);
                });
            });
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
        public DbSet<MunicipalityCard> MunicipalityCards { get; set; } = null!;
        public DbSet<MunicipalityHomeInfo> MunicipalityHomeInfos { get; set; } = null!;
        public DbSet<MunicipalityHomeContactInfo> MunicipalityHomeContactInfos { get; set; } = null!;

        // Entertainment and leisure entities
        public DbSet<EntertainmentLeisureCard> EntertainmentLeisureCards { get; set; } = null!;
        public DbSet<EntertainmentLeisureDetail> EntertainmentLeisureDetails { get; set; } = null!;

        // Organization entities
        public DbSet<OrganizationCard> OrganizationCards { get; set; } = null!;
        public DbSet<OrganizationMobileDetail> OrganizationMobileDetails { get; set; } = null!;
        public DbSet<OwnedPoi> OwnedPois { get; set; } = null!;
        public DbSet<FeatureCardRelationship<ArtCultureNatureDetail>> FeatureCardArtCultureRelationships { get; set; } = null!;

        // Route entities
        public DbSet<RouteCard> RouteCards { get; set; } = null!;
        public DbSet<RouteDetail> RouteDetails { get; set; } = null!;
        public DbSet<Point> RoutePoints { get; set; } = null!;
        public DbSet<FeatureCardRelationship<RouteDetail>> RouteFeatureCardRelationships { get; set; } = null!;
        public DbSet<StageMobileRelationship<RouteDetail>> RouteStageMobileRelationships { get; set; } = null!;
        public DbSet<StageMobile> StageMobiles { get; set; } = null!;

        // Service entities
        public DbSet<ServiceCard> ServiceCards { get; set; } = null!;
        public DbSet<ServiceDetail> ServiceDetails { get; set; } = null!;
        public DbSet<ServiceLocationRelationship<ServiceDetail>> ServiceLocations { get; set; } = null!;



    }
}