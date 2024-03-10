using AutoFixture;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Entities.Users;
using Corely.Domain.Exceptions;
using Corely.Domain.Mappers;
using Corely.Domain.Models.Users;
using Corely.Domain.Repos;
using Corely.Domain.Services.Users;
using Corely.Domain.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.Domain.Services.Users
{
    public class UserServiceTests
    {
        private const string VALID_USERNAME = "username";
        private const string VALID_EMAIL = "email@x.y";

        private readonly Fixture _fixture = new();
        private readonly ServiceFactory _serviceFactory = new();
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userService = new UserService(
                _serviceFactory.GetRequiredService<IRepoExtendedGet<UserEntity>>(),
                _serviceFactory.GetRequiredService<IReadonlyRepo<AccountEntity>>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<UserService>>());
        }

        [Fact]
        public async Task CreateUserAsync_ShouldThrowUserExistsException_WhenUserExists()
        {
            var accountId = await CreateAccount();
            var createUserRequest = new CreateUserRequest(accountId, VALID_USERNAME, VALID_EMAIL);
            await _userService.CreateUserAsync(createUserRequest);

            Exception ex = await Record.ExceptionAsync(() => _userService.CreateUserAsync(createUserRequest));

            Assert.NotNull(ex);
            Assert.IsType<UserExistsException>(ex);
        }

        [Fact]
        public async Task CreateUser_ShouldReturnCreateUserResult()
        {
            var accountId = await CreateAccount();
            var createUserRequest = new CreateUserRequest(accountId, VALID_USERNAME, VALID_EMAIL);
            var res = await _userService.CreateUserAsync(createUserRequest);

            Assert.True(res.IsSuccess);
        }

        private async Task<int> CreateAccount()
        {
            var accountId = _fixture.Create<int>();
            var account = new AccountEntity { Id = accountId };
            var accountRepo = _serviceFactory.GetRequiredService<IRepoExtendedGet<AccountEntity>>();
            return await accountRepo.CreateAsync(account);
        }

        [Fact]
        public async Task CreateUser_ShouldThrowAccountDoesNotExistException_WhenAccountDNE()
        {
            var createUserRequest = new CreateUserRequest(_fixture.Create<int>(), VALID_USERNAME, VALID_EMAIL);

            Exception ex = await Record.ExceptionAsync(() => _userService.CreateUserAsync(createUserRequest));

            Assert.NotNull(ex);
            Assert.IsType<AccountDoesNotExistException>(ex);
        }

        [Fact]
        public async Task CreateUser_ShouldThrowArgumentNullException_WithNullRequest()
        {
            var ex = await Record.ExceptionAsync(() => _userService.CreateUserAsync(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenUserRepoIsNull()
        {
            UserService act() => new(
                null,
                _serviceFactory.GetRequiredService<IReadonlyRepo<AccountEntity>>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<UserService>>());

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenReadonlyAccountRepoIsNull()
        {
            UserService act() => new(
                Mock.Of<IRepoExtendedGet<UserEntity>>(),
                null,
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<UserService>>());

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenMapProviderIsNull()
        {
            UserService act() => new(
                Mock.Of<IRepoExtendedGet<UserEntity>>(),
                _serviceFactory.GetRequiredService<IReadonlyRepo<AccountEntity>>(),
                null,
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<UserService>>());

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenValidationProviderIsNull()
        {
            UserService act() => new(
                Mock.Of<IRepoExtendedGet<UserEntity>>(),
                _serviceFactory.GetRequiredService<IReadonlyRepo<AccountEntity>>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                null,
                _serviceFactory.GetRequiredService<ILogger<UserService>>());

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            UserService act() => new(
                Mock.Of<IRepoExtendedGet<UserEntity>>(),
                _serviceFactory.GetRequiredService<IReadonlyRepo<AccountEntity>>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                null);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}
