using Domain.Entities;
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
    }
}
