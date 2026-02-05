using Domain.Entities;
using Domain.Entities.MunicipalityEntities;
using Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace nam.ServerTests.Integration.Database
{
    [TestFixture]
    public sealed class PersistenceTests
    {
        
        private DbContextOptions<ApplicationDbContext> GetOptions(SqliteConnection connection)
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
        }

        [Test]
        public async Task User_is_persisted_across_contexts_async()
        {
            
            await using var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = GetOptions(connection);

            
            await using (var context = new ApplicationDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();

                context.Users.Add(new User
                {
                    Email = "persisted@example.com",
                    PasswordHash = "hash",
                    IsEmailVerified = true 
                });

                await context.SaveChangesAsync();
            }

            
            await using (var context = new ApplicationDbContext(options))
            {
                var user = await context.Users.SingleOrDefaultAsync(u => u.Email == "persisted@example.com");

                Assert.That(user, Is.Not.Null, "Expected user to be persisted across contexts.");
                Assert.That(user!.Email, Is.EqualTo("persisted@example.com"));
            }
        }

        [Test]
        public async Task Map_marker_is_persisted_across_contexts_async()
        {
            await using var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = GetOptions(connection);

            await using (var context = new ApplicationDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();

                context.MapMarkers.Add(new MapMarker
                {
                    Name = "Marker One",
                    ImagePath = "marker.png",
                    Typology = "Test"
                });

                await context.SaveChangesAsync();
            }

            await using (var context = new ApplicationDbContext(options))
            {
                var marker = await context.MapMarkers.SingleOrDefaultAsync(m => m.Name == "Marker One");

                Assert.That(marker, Is.Not.Null, "Expected map marker to be persisted across contexts.");
                Assert.That(marker!.ImagePath, Is.EqualTo("marker.png"));
            }
        }

        [Test]
        public async Task Map_data_is_persisted_across_contexts_async()
        {
            await using var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = GetOptions(connection);

            await using (var context = new ApplicationDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();

                context.MapData.Add(new MapData
                {
                    Name = "Test Map",
                    CenterLatitude = 45.123,
                    CenterLongitude = 9.456
                });

                await context.SaveChangesAsync();
            }

            await using (var context = new ApplicationDbContext(options))
            {
                var mapData = await context.MapData.SingleOrDefaultAsync(m => m.Name == "Test Map");

                Assert.That(mapData, Is.Not.Null, "Expected map data to be persisted across contexts.");
                Assert.That(mapData!.CenterLatitude, Is.EqualTo(45.123));
            }
        }

        [Test]
        public async Task Municipality_card_is_persisted_across_contexts_async()
        {
            await using var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = GetOptions(connection);

            await using (var context = new ApplicationDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();

                context.MunicipalityCards.Add(new MunicipalityCard
                {
                    LegalName = "Test Municipality",
                    ImagePath = "municipality.png"
                });

                await context.SaveChangesAsync();
            }

            await using (var context = new ApplicationDbContext(options))
            {
                var card = await context.MunicipalityCards.SingleOrDefaultAsync(m => m.LegalName == "Test Municipality");

                Assert.That(card, Is.Not.Null, "Expected municipality card to be persisted across contexts.");
                Assert.That(card!.ImagePath, Is.EqualTo("municipality.png"));
            }
        }
    }
}