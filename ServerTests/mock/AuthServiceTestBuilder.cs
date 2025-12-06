using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using nam.Server.Data;
using nam.Server.Models.Entities;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces;
using nam.Server.Models.Services.Infrastructure.Services.Implemented.Auth;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.Auth;
using System.Linq.Expressions;
using System.Security.Claims;

namespace nam.ServerTests.mock
{
    public class AuthServiceTestBuilder : IDisposable
    {
        public ApplicationDbContext Context { get; }

        // Espongo i servizi fake per asserzioni nei test (es. controllo mail inviate)
        public FakeEmailService EmailService { get; } = new();
        public FakeCodeService CodeService { get; } = new();

        public AuthServiceTestBuilder()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"AuthDb_{Guid.NewGuid()}")
                .Options;

            Context = new ApplicationDbContext(options);
        }

        public AuthService Build()
        {
            // 1. Fake UnitOfWork (implementa la nuova interfaccia)
            var unitOfWork = new FakeUnitOfWork(Context);

            // 2. Fake Token Generation
            var tokenGen = new FakeTokenGeneration();

            // 3. Fake Configuration
            var myConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"Frontend:BaseUrl", "http://localhost:test"}
                })
                .Build();

            return new AuthService(
                unitOfWork,
                tokenGen,
                myConfiguration,
                Context,
                EmailService,
                CodeService
            );
        }

        public async Task SeedUserAsync(string email, string clearPassword, bool isVerified = true)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(clearPassword),
                IsEmailVerified = isVerified
            };
            Context.Users.Add(user);
            await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }

    // --- IMPLEMENTAZIONI FAKE AGGIORNATE ---

    public class FakeUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public FakeUnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new FakeUserRepository(_context);
        }

        public IUserRepository Users { get; }

        public Task CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }
    }

    public class FakeUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<User> _dbSet;

        public FakeUserRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<User>();
        }

        // --- Metodi di IRepository<User, Guid> ---

        public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public IEnumerable<User> Find(Expression<Func<User, bool>> predicate)
        {
            // Nota: Find su IEnumerable è sincrono, quindi eseguiamo la query in memoria
            return _dbSet.Where(predicate).ToList();
        }

        public void Add(User entity)
        {
            _dbSet.Add(entity);
        }

        public void Remove(User entity)
        {
            _dbSet.Remove(entity);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        // --- Metodi specifici di IUserRepository ---

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return _dbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            return _dbSet.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<bool> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(user, cancellationToken);
            // L'interfaccia restituisce un bool, presumibilmente se il salvataggio ha avuto successo
            var changes = await _context.SaveChangesAsync(cancellationToken);
            return changes > 0;
        }

        public async Task<bool> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(user);
            var changes = await _context.SaveChangesAsync(cancellationToken);
            return changes >= 0; // Update potrebbe non cambiare nulla se i dati sono uguali, ma non è un errore
        }
    }

    // --- ALTRE DIPENDENZE FAKE (Invariate) ---

    public class FakeTokenGeneration : ITokenGeneration
    {
        public Task<string> GenerateTokenAsync(string userId, string email)
        {
            return Task.FromResult("fake-jwt-token-generated");
        }

        public ClaimsPrincipal? ValidateEmailVerificationToken(string token)
        {
            if (token == "valid-token")
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Email, "user@example.com") };
                var identity = new ClaimsIdentity(claims, "TestAuth");
                return new ClaimsPrincipal(identity);
            }
            return null;
        }
    }

    public class FakeEmailService : IEmailService
    {
        public List<(string to, string subject, string body)> SentEmails { get; } = new();

        public Task SendEmailAsync(string to, string subject, string body)
        {
            SentEmails.Add((to, subject, body));
            return Task.CompletedTask;
        }
    }

    public class FakeCodeService : ICodeService
    {
        public int TimeToLiveMinutes => 15;
        public string GenerateAuthCode() => "123456";
    }
}