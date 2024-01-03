using Corely.Domain.Mappers;
using Corely.Domain.Repos;
using Corely.Domain.Services.Users;
using Corely.Domain.Validators;
using Corely.UnitTests.Collections;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.Domain.Services.Users
{
    [Collection(CollectionNames.LoggerCollection)]
    public class UserServiceTests : IDisposable
    {
        private readonly ServiceFactory _serviceFactory = new();
        private readonly ILogger<UserService> _logger;

        public UserServiceTests(LoggerFixture loggerFixture)
        {
            _logger = loggerFixture.CreateLogger<UserService>();
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenUserRepoIsNull()
        {
            UserService act() => new(
                null,
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _logger);

            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenValidationProviderIsNull()
        {
            UserService act() => new(
                Mock.Of<IUserRepo>(),
                null,
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _logger);

            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenMapProviderIsNull()
        {
            UserService act() => new(
                Mock.Of<IUserRepo>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                null,
                _logger);

            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            UserService act() => new(
                Mock.Of<IUserRepo>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                null);

            Assert.Throws<ArgumentNullException>(act);
        }

        public void Dispose()
        {
            _serviceFactory.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
