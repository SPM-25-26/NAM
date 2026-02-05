using Domain.Entities;
using Infrastructure;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.UnitOfWork;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using nam.Server.Services.Implemented.Auth;
using nam.Server.Services.Interfaces.Auth;
using NSubstitute;
using System.Linq.Expressions;

namespace nam.ServerTests.NamServer.Endpoints.Auth.mock
{
    public class AuthServiceTestBuilder : IDisposable
    {
        private readonly SqliteConnection _connection;
        public ApplicationDbContext Context { get; }
        public IEmailService EmailService { get; }
        public ICodeService CodeService { get; }
        public ITokenGeneration TokenService { get; }
        public IUnitOfWork UnitOfWork { get; }

        public AuthServiceTestBuilder()
        {
            
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection)
                .Options;

            Context = new ApplicationDbContext(options);
            Context.Database.EnsureCreated();

            
            EmailService = Substitute.For<IEmailService>();
            CodeService = Substitute.For<ICodeService>();
            TokenService = Substitute.For<ITokenGeneration>();

            
            CodeService.GenerateAuthCode().Returns("123456");
            CodeService.TimeToLiveMinutes.Returns(15);
            EmailService.SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                        .Returns(Task.CompletedTask);
            TokenService.GenerateTokenAsync(Arg.Any<string>(), Arg.Any<string>())
                        .Returns("fake-jwt-token-generated");

            
            var fakePrincipal = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(new[] {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, "valid@example.com")
                }));
            TokenService.ValidateEmailVerificationToken(Arg.Is("valid-token")).Returns(fakePrincipal);


            UnitOfWork = Substitute.For<IUnitOfWork>();

            
            var userRepository = new TestUserRepository(Context);
            UnitOfWork.Users.Returns(userRepository);

            UnitOfWork.CompleteAsync().Returns(async x => await Context.SaveChangesAsync());
        }

        public AuthService Build()
        {
            var myConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"Frontend:BaseUrl", "http://localhost:test"}
                })
                .Build();

            return new AuthService(
                UnitOfWork,
                TokenService,
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
            Context.Dispose();
            _connection.Close();
            _connection.Dispose();
        }
    }

    // Repository Concreto per i test: evita problemi con Mock asincroni
    public class TestUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<User> _dbSet;

        public TestUserRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<User>();
        }

        // Metodi usati da AuthService
        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(u => u.Email == email, ct);

        public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
            => await _dbSet.AnyAsync(u => u.Email == email, ct);

        public async Task<bool> AddAsync(User user, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(user, ct);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> UpdateAsync(User user, CancellationToken ct = default)
        {
            _dbSet.Update(user);
            return await _context.SaveChangesAsync(ct) >= 0;
        }


        public async Task<User?> GetAsync(Guid id, CancellationToken ct = default) => await _dbSet.FindAsync(new object[] { id }, ct);
        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default) => await _dbSet.ToListAsync(ct);
        public IEnumerable<User> Find(Expression<Func<User, bool>> predicate) => _dbSet.Where(predicate).ToList();
        public void Add(User entity) => _dbSet.Add(entity);
        public void Remove(User entity) => _dbSet.Remove(entity);
        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _context.SaveChangesAsync(ct);

        public Task<bool> UpdateQuestionaireByEmailAsync(Questionaire questionaire, string email, CancellationToken ct = default)
            => throw new NotImplementedException("Not needed for Auth tests");
    }
}