﻿using AutoFixture;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Accounts.Exceptions;
using Corely.IAM.Mappers;
using Corely.IAM.Repos;
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
                _serviceFactory.GetRequiredService<IReadonlyRepo<AccountEntity>>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<UserService>>());
        }

        [Fact]
        public async Task CreateUserAsync_ThrowsUserExistsException_WhenUserExists()
        {
            var accountId = await CreateAccount();
            var createUserRequest = new CreateUserRequest(accountId, VALID_USERNAME, VALID_EMAIL);
            await _userService.CreateUserAsync(createUserRequest);

            Exception ex = await Record.ExceptionAsync(() => _userService.CreateUserAsync(createUserRequest));

            Assert.NotNull(ex);
            Assert.IsType<UserExistsException>(ex);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreateUserResult()
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
        public async Task CreateUser_ThrowsAccountDoesNotExistException_WhenAccountDNE()
        {
            var createUserRequest = new CreateUserRequest(_fixture.Create<int>(), VALID_USERNAME, VALID_EMAIL);

            Exception ex = await Record.ExceptionAsync(() => _userService.CreateUserAsync(createUserRequest));

            Assert.NotNull(ex);
            Assert.IsType<AccountDoesNotExistException>(ex);
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
            var accountId = await CreateAccount();
            var createUserRequest = new CreateUserRequest(accountId, VALID_USERNAME, VALID_EMAIL);
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
            var accountId = await CreateAccount();
            var createUserRequest = new CreateUserRequest(accountId, VALID_USERNAME, VALID_EMAIL);
            await _userService.CreateUserAsync(createUserRequest);

            var user = await _userService.GetUserAsync(createUserRequest.Username);

            Assert.NotNull(user);
            Assert.Equal(createUserRequest.Username, user.Username);
            Assert.Equal(createUserRequest.Email, user.Email);
        }
    }
}
