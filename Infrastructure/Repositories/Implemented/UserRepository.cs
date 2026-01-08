using Domain.Entities;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implemented
{
    public class UserRepository(ApplicationDbContext context) : Repository<User, Guid>(context), IUserRepository
    {
        private ApplicationDbContext AppContext => (ApplicationDbContext)_context;

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await AppContext.Users
                .Include(u => u.Questionaire)
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            return await AppContext.Users
                .AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<bool> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await AppContext.Set<User>().AddAsync(user, cancellationToken);
            var result = await AppContext.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            AppContext.Set<User>().Update(user);
            var result = await AppContext.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> UpdateQuestionaireByEmailAsync(Questionaire questionaire, string email, CancellationToken cancellationToken = default)
        {
            var user = await context.Users
                .Include(u => u.Questionaire)
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (user == null)
                return false;

            user.Questionaire ??= new Questionaire();
            user.Questionaire.Interest = questionaire.Interest;
            user.Questionaire.TravelStyle = questionaire.TravelStyle;
            user.Questionaire.AgeRange = questionaire.AgeRange;
            user.Questionaire.TravelRange = questionaire.TravelRange;
            user.Questionaire.TravelCompanions = questionaire.TravelCompanions;
            user.Questionaire.DiscoveryMode = questionaire.DiscoveryMode;

            try
            {
                var result = await UpdateAsync(user!, cancellationToken);
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                    if (entry.Entity is Questionaire)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues![property];
                            // Resolve conflicts by preferring the database values
                            proposedValues[property] = databaseValue;
                        }
                        // Refresh original values to bypass next concurrency check
                        entry.OriginalValues.SetValues(databaseValues);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "Don't know how to handle concurrency conflicts for "
                            + entry.Metadata.Name);
                    }
                return false;
            }
        }
    }
}
