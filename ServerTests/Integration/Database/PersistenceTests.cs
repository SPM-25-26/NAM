using Domain.Entities;
using Domain.Entities.MunicipalityEntities;
using Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace nam.ServerTests.Integration.Database
{
    [TestClass]
    public sealed class PersistenceTests
    {
        [TestMethod]
        public async Task User_is_persisted_across_contexts_async()
        {
            await using var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            await using (var context = new ApplicationDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();

                context.Users.Add(new User
                {
                    Email = "persisted@example.com",
                    PasswordHash = "hash"
                });

                await context.SaveChangesAsync();
            }

            await using (var context = new ApplicationDbContext(options))
            {
                var user = await context.Users.SingleOrDefaultAsync(u => u.Email == "persisted@example.com");

                Assert.IsNotNull(user, "Expected user to be persisted across contexts.");
                Assert.AreEqual("persisted@example.com", user.Email);
            }
        }

        [TestMethod]
        public async Task Map_marker_is_persisted_across_contexts_async()
        {
            await using var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

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

                Assert.IsNotNull(marker, "Expected map marker to be persisted across contexts.");
                Assert.AreEqual("marker.png", marker.ImagePath);
            }
        }

        [TestMethod]
        public async Task Map_data_is_persisted_across_contexts_async()
        {
            await using var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

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

                Assert.IsNotNull(mapData, "Expected map data to be persisted across contexts.");
                Assert.AreEqual(45.123, mapData.CenterLatitude);
            }
        }

        [TestMethod]
        public async Task Municipality_card_is_persisted_across_contexts_async()
        {
            await using var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

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

                Assert.IsNotNull(card, "Expected municipality card to be persisted across contexts.");
                Assert.AreEqual("municipality.png", card.ImagePath);
            }
        }
    }
}
