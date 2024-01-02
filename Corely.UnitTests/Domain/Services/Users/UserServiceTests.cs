using Corely.Domain.Exceptions;
using Corely.Domain.Mappers;
using Corely.Domain.Models.Auth;
using Corely.Domain.Models.Users;
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
        private readonly UserService _userService;
        public UserServiceTests(LoggerFixture loggerFixture)
        {
            _logger = loggerFixture.CreateLogger<UserService>();
            _userService = new UserService(
                Mock.Of<IUserRepo>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _logger);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenUserRepoIsNull()
        {
            UserService act() => new(
                null,
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _logger);

            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenValidationProviderIsNull()
        {
            UserService act() => new(
                Mock.Of<IUserRepo>(),
                null,
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _logger);

            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenMapProviderIsNull()
        {
            UserService act() => new(
                Mock.Of<IUserRepo>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                null,
                _logger);

            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenLoggerIsNull()
        {
            UserService act() => new(
                Mock.Of<IUserRepo>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                null);

            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void Create_ShouldThrow_WhenUserIsNull()
        {
            void act() => _userService.Create(null, new BasicAuth());
            Assert.Throws<UserServiceException>(act);
        }

        [Fact]
        public void Create_ShouldThrow_WhenBasicAuthIsNull()
        {
            void act() => _userService.Create(new User(), null);
            Assert.Throws<UserServiceException>(act);
        }

        public void Dispose()
        {
            _serviceFactory.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
