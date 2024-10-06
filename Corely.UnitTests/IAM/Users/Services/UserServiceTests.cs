using AutoFixture;
using Corely.IAM.Mappers;
using Corely.IAM.Repos;
using Corely.IAM.Security.Services;
using Corely.IAM.Users.Entities;
using Corely.IAM.Users.Exceptions;
using Corely.IAM.Users.Models;
using Corely.IAM.Users.Services;
using Corely.IAM.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.Users.Services
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
                _serviceFactory.GetRequiredService<ISecurityService>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<UserService>>());
        }

        [Fact]
        public async Task CreateUserAsync_ThrowsUserExistsException_WhenUserExists()
        {
            var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
            await _userService.CreateUserAsync(createUserRequest);

            Exception ex = await Record.ExceptionAsync(() => _userService.CreateUserAsync(createUserRequest));

            Assert.NotNull(ex);
            Assert.IsType<UserExistsException>(ex);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreateUserResult()
        {
            var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
            var res = await _userService.CreateUserAsync(createUserRequest);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task CreateUser_ThrowsArgumentNullException_WithNullRequest()
        {
            var ex = await Record.ExceptionAsync(() => _userService.CreateUserAsync(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task GetUserByUseridAsync_ReturnsNull_WhenUserDNE()
        {
            var user = await _userService.GetUserAsync(_fixture.Create<int>());

            Assert.Null(user);
        }

        [Fact]
        public async Task GetUserByUseridAsync_ReturnsUser_WhenUserExists()
        {
            var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
            var createResult = await _userService.CreateUserAsync(createUserRequest);

            var user = await _userService.GetUserAsync(createResult.CreatedId);

            Assert.NotNull(user);
            Assert.Equal(createUserRequest.Username, user.Username);
            Assert.Equal(createUserRequest.Email, user.Email);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_ReturnsNull_WhenUserDNE()
        {
            var user = await _userService.GetUserAsync(_fixture.Create<string>());

            Assert.Null(user);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_ReturnsUser_WhenUserExists()
        {
            var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
            await _userService.CreateUserAsync(createUserRequest);

            var user = await _userService.GetUserAsync(createUserRequest.Username);

            Assert.NotNull(user);
            Assert.Equal(createUserRequest.Username, user.Username);
            Assert.Equal(createUserRequest.Email, user.Email);
        }

        [Fact]
        public async Task UpdateUserAsync_UpdatesUser()
        {
            var createUserRequest = new CreateUserRequest(VALID_USERNAME, VALID_EMAIL);
            await _userService.CreateUserAsync(createUserRequest);
            var user = await _userService.GetUserAsync(createUserRequest.Username);
            user!.Disabled = false;

            await _userService.UpdateUserAsync(user);
            var updatedUser = await _userService.GetUserAsync(createUserRequest.Username);

            Assert.False(updatedUser!.Disabled);
        }
    }
}
