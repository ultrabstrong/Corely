using Corely.Domain.Entities.Auth;
using Corely.Domain.Enums;
using Corely.Domain.Mappers;
using Corely.Domain.Models.Auth;
using Corely.Domain.Repos;
using Corely.Domain.Services.Auth;
using Corely.Domain.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.Domain.Services.Auth
{
    public class AuthServiceTests
    {
        private readonly ServiceFactory _serviceFactory = new();
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _authService = new AuthService(
                _serviceFactory.GetRequiredService<IRepoExtendedGet<BasicAuthEntity>>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<AuthService>>());
        }

        [Fact]
        public async Task UpsertBasicAuthAsync_ShouldReturnCreateResult_WhenBasicAuthDoesNotExist()
        {
            var request = new UpsertBasicAuthRequest(1, "username", "password");
            var result = await _authService.UpsertBasicAuthAsync(request);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(UpsertType.Create, result.UpsertType);
        }

        [Fact]
        public async Task UpsertBasicAuthAsync_ShouldReturnUpdateResult_WhenBasicAuthExists()
        {
            var request = new UpsertBasicAuthRequest(1, "username", "password");
            await _authService.UpsertBasicAuthAsync(request);
            var result = await _authService.UpsertBasicAuthAsync(request);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(UpsertType.Update, result.UpsertType);
        }

        [Fact]
        public async Task UpsertBasicAuthAsync_ShouldThrowArgumentNullException_WithNullRequest()
        {
            var ex = await Record.ExceptionAsync(() => _authService.UpsertBasicAuthAsync(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenBasicAuthRepoIsNull()
        {
            AuthService act() => new(
                null,
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<AuthService>>());

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenMapProviderIsNull()
        {
            AuthService act() => new(
                Mock.Of<IRepoExtendedGet<BasicAuthEntity>>(),
                null,
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<AuthService>>());

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenValidationProviderIsNull()
        {
            AuthService act() => new(
                Mock.Of<IRepoExtendedGet<BasicAuthEntity>>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                null,
                _serviceFactory.GetRequiredService<ILogger<AuthService>>());

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            AuthService act() => new(
                Mock.Of<IRepoExtendedGet<BasicAuthEntity>>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                null);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}
