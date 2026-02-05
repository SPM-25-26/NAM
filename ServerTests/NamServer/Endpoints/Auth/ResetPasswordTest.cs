using Domain.Entities;
using Domain.Entities.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using nam.Server.DTOs;
using nam.Server.Endpoints.Auth;
using nam.Server.Services.Interfaces.Auth;
using nam.ServerTests.NamServer.Endpoints.Auth.mock;
using NSubstitute;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace nam.ServerTests.NamServer.Endpoints.Auth
{
    [TestFixture]
    public sealed class PasswordResetTests
    {
        private AuthServiceTestBuilder _builder = null!;
        private IAuthService _authService = null!;
        private const string StaticAuthCode = "123456";

        [SetUp]
        public void Setup()
        {
            _builder = new AuthServiceTestBuilder();
            _authService = _builder.Build();
        }

        [TearDown]
        public void Cleanup()
        {
            _builder.Dispose();
        }

        private PasswordResetResponseDto GetDtoFromAnonymousResult(object result)
        {
            
            Assert.That(result.GetType().Name, Does.StartWith("Ok"), "Il risultato non è un Ok Result");

            
            var valueProp = result.GetType().GetProperty("Value");
            var anonymousValue = valueProp?.GetValue(result);
            Assert.That(anonymousValue, Is.Not.Null, "Il valore della risposta è nullo");

            
            var dataProp = anonymousValue!.GetType().GetProperty("data");
            Assert.That(dataProp, Is.Not.Null, "La risposta non contiene la proprietà 'data'");

            return (PasswordResetResponseDto)dataProp!.GetValue(anonymousValue)!;
        }

        [Test]
        public async Task RequestPasswordReset_EmailNotExists_ReturnsNotFound()
        {
            var request = new PasswordResetRequestDto { Email = "nonexistent@example.com" };

            var result = await AuthEndpoints.RequestPasswordReset(request, _authService);

            
            Assert.That(result, Is.InstanceOf<ProblemHttpResult>());
            var problem = (ProblemHttpResult)result;

            Assert.That(problem.StatusCode, Is.EqualTo(404));
            Assert.That(problem.ProblemDetails.Detail, Is.EqualTo("The email not found"));

            var codesCount = await _builder.Context.ResetPasswordAuth.CountAsync();
            Assert.That(codesCount, Is.EqualTo(0));
        }

        [Test]
        public async Task RequestPasswordReset_EmailExists_CodeIsCreatedAndSaved()
        {
            const string testEmail = "test@example.com";
            Guid testUserId = Guid.NewGuid();

            _builder.Context.Users.Add(new User
            {
                Id = testUserId,
                Email = testEmail,
                PasswordHash = "dummyhash",
                IsEmailVerified = true
            });
            await _builder.Context.SaveChangesAsync();

            var request = new PasswordResetRequestDto { Email = testEmail };
            var beforeRequest = DateTime.UtcNow;

            var result = await AuthEndpoints.RequestPasswordReset(request, _authService);

            
            if (result is ProblemHttpResult problem) Assert.Fail($"Server Error: {problem.ProblemDetails.Detail}");

            
            var responseDto = GetDtoFromAnonymousResult(result);
            Assert.That(responseDto.Success, Is.True);

            
            var savedCode = await _builder.Context.ResetPasswordAuth
                                    .FirstOrDefaultAsync(c => c.UserId == testUserId.ToString());

            Assert.That(savedCode, Is.Not.Null);
            Assert.That(savedCode!.AuthCode, Is.EqualTo(StaticAuthCode));
            Assert.That(savedCode.ExpiresAt, Is.GreaterThan(beforeRequest.AddMinutes(14)));
            Assert.That(savedCode.UserId, Is.EqualTo(testUserId.ToString()));

            await _builder.EmailService.Received(1).SendEmailAsync(testEmail, Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public async Task RequestPasswordReset_ExistingCodeIsOverwritten()
        {
            const string testEmail = "overwrite@example.com";
            Guid testUserId = Guid.NewGuid();

            _builder.Context.Users.Add(new User
            {
                Id = testUserId,
                Email = testEmail,
                PasswordHash = "dummyhash",
                IsEmailVerified = true
            });

            var oldAuthCode = "999999";
            var oldExpiration = DateTime.UtcNow.AddMinutes(5);

            _builder.Context.ResetPasswordAuth.Add(new PasswordResetCode
            {
                UserId = testUserId.ToString(),
                AuthCode = oldAuthCode,
                ExpiresAt = oldExpiration,
                CreatedAt = DateTime.UtcNow.AddMinutes(-10)
            });
            await _builder.Context.SaveChangesAsync();

            var request = new PasswordResetRequestDto { Email = testEmail };
            var beforeSecondRequest = DateTime.UtcNow;

            var result = await AuthEndpoints.RequestPasswordReset(request, _authService);

            
            var responseDto = GetDtoFromAnonymousResult(result);
            Assert.That(responseDto.Success, Is.True);

            var codesCount = await _builder.Context.ResetPasswordAuth.CountAsync();
            Assert.That(codesCount, Is.EqualTo(1));

            var updatedCode = await _builder.Context.ResetPasswordAuth.SingleAsync();
            Assert.That(updatedCode.AuthCode, Is.EqualTo(StaticAuthCode));
            Assert.That(updatedCode.ExpiresAt, Is.GreaterThan(beforeSecondRequest.AddMinutes(14)));
        }

        [Test]
        public async Task ResetPassword_ExpiredCode_ReturnsBadRequest()
        {
            const string testEmail = "expired@example.com";
            Guid testUserId = Guid.NewGuid();

            _builder.Context.Users.Add(new User
            {
                Id = testUserId,
                Email = testEmail,
                PasswordHash = "dummyhash",
                IsEmailVerified = true
            });

            var expiredTime = DateTime.UtcNow.AddMinutes(-5);
            _builder.Context.ResetPasswordAuth.Add(new PasswordResetCode
            {
                UserId = testUserId.ToString(),
                AuthCode = StaticAuthCode,
                CreatedAt = expiredTime.AddMinutes(-15),
                ExpiresAt = expiredTime
            });
            await _builder.Context.SaveChangesAsync();

            var request = new PasswordResetConfirmDto
            {
                AuthCode = StaticAuthCode,
                NewPassword = "mock_password_1",
                ConfirmPassword = "mock_password_1",
            };

            var result = await AuthEndpoints.ResetPassword(request, _authService);

            
            Assert.That(result, Is.InstanceOf<ProblemHttpResult>());
            var problem = (ProblemHttpResult)result;

            Assert.That(problem.StatusCode, Is.EqualTo(400));
            Assert.That(problem.ProblemDetails.Detail.ToLower(), Does.Contain("expired"));
        }

        [Test]
        public async Task ResetPassword_IncorrectCode_ReturnsBadRequest()
        {
            const string testEmail = "wrongcode@example.com";
            Guid testUserId = Guid.NewGuid();

            _builder.Context.Users.Add(new User
            {
                Id = testUserId,
                Email = testEmail,
                PasswordHash = "dummyhash",
                IsEmailVerified = true
            });

            var correctCode = "555555";
            var validTime = DateTime.UtcNow.AddMinutes(5);
            _builder.Context.ResetPasswordAuth.Add(new PasswordResetCode
            {
                UserId = testUserId.ToString(),
                AuthCode = correctCode,
                ExpiresAt = validTime,
                CreatedAt = DateTime.UtcNow
            });
            await _builder.Context.SaveChangesAsync();

            var request = new PasswordResetConfirmDto
            {
                AuthCode = "999999",
                NewPassword = "new",
                ConfirmPassword = "new"
            };

            var result = await AuthEndpoints.ResetPassword(request, _authService);

            // L'endpoint usa TypedResults.Problem(400)
            Assert.That(result, Is.InstanceOf<ProblemHttpResult>());
            var problem = (ProblemHttpResult)result;

            Assert.That(problem.StatusCode, Is.EqualTo(400));
            Assert.That(problem.ProblemDetails.Detail.ToLower(), Does.Match(".*(invalid|not found).*"));

            var codeExists = await _builder.Context.ResetPasswordAuth.AnyAsync();
            Assert.That(codeExists, Is.True);
        }
    }
}