using AutoFixture;
using Corely.DataAccess.Interfaces.Repos;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Accounts.Exceptions;
using Corely.IAM.Accounts.Models;
using Corely.IAM.Accounts.Services;
using Corely.IAM.Mappers;
using Corely.IAM.Security.Services;
using Corely.IAM.Users.Entities;
using Corely.IAM.Users.Exceptions;
using Corely.IAM.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.Accounts.Services
{
    public class AccountServiceTests
    {
        private const string VALID_ACCOUNT_NAME = "accountname";

        private readonly Fixture _fixture = new();
        private readonly ServiceFactory _serviceFactory = new();
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _accountService = new AccountService(
                _serviceFactory.GetRequiredService<IRepoExtendedGet<AccountEntity>>(),
                _serviceFactory.GetRequiredService<IReadonlyRepo<UserEntity>>(),
                _serviceFactory.GetRequiredService<ISecurityService>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<AccountService>>());
        }

        private async Task<int> CreateUserAsync()
        {
            var userId = _fixture.Create<int>();
            var user = new UserEntity { Id = userId };
            var userRepo = _serviceFactory.GetRequiredService<IRepo<UserEntity>>();
            return await userRepo.CreateAsync(user);
        }

        [Fact]
        public async Task CreateAccountAsync_Throws_WhenAccountExists()
        {
            var createAccountRequest = new CreateAccountRequest(VALID_ACCOUNT_NAME, await CreateUserAsync());
            await _accountService.CreateAccountAsync(createAccountRequest);

            var ex = await Record.ExceptionAsync(() => _accountService.CreateAccountAsync(createAccountRequest));

            Assert.NotNull(ex);
            Assert.IsType<AccountExistsException>(ex);
        }

        [Fact]
        public async Task CreateAccount_ReturnsCreateAccountResult()
        {
            var userIdOfOwner = await CreateUserAsync();
            var createAccountRequest = new CreateAccountRequest(VALID_ACCOUNT_NAME, userIdOfOwner);

            var createAccountResult = await _accountService.CreateAccountAsync(createAccountRequest);

            Assert.True(createAccountResult.IsSuccess);

            // Verify account is linked to owner user id
            var accountRepo = _serviceFactory.GetRequiredService<IRepoExtendedGet<AccountEntity>>();
            var accountEntity = await accountRepo.GetAsync(
                a => a.Id == createAccountResult.CreatedId,
                include: q => q.Include(a => a.Users));
            Assert.NotNull(accountEntity);
            Assert.NotNull(accountEntity.Users);
            Assert.Single(accountEntity.Users);
            Assert.Equal(userIdOfOwner, accountEntity.Users.First().Id);
        }

        [Fact]
        public async Task CreateAccount_Throws_WithNullRequest()
        {
            var ex = await Record.ExceptionAsync(() => _accountService.CreateAccountAsync(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task CreateAccount_Throws_WithInvalidUserId()
        {
            var createAccountRequest = new CreateAccountRequest(VALID_ACCOUNT_NAME, -1);
            var ex = await Record.ExceptionAsync(() => _accountService.CreateAccountAsync(createAccountRequest));

            Assert.NotNull(ex);
            Assert.IsType<UserDoesNotExistException>(ex);
        }

        [Fact]
        public async Task CreateAccount_Throws_WithNullAccountName()
        {
            var createAccountRequest = new CreateAccountRequest(null!, -1);
            var ex = await Record.ExceptionAsync(() => _accountService.CreateAccountAsync(createAccountRequest));

            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
        }

        [Fact]
        public async Task GetAccountByAccountIdAsync_ReturnsNull_WhenAccountDNE()
        {
            var account = await _accountService.GetAccountAsync(_fixture.Create<int>());

            Assert.Null(account);
        }

        [Fact]
        public async Task GetAccountByAccountIdAsync_ReturnsAccount_WhenAccountExists()
        {
            var createAccountRequest = new CreateAccountRequest(VALID_ACCOUNT_NAME, await CreateUserAsync());
            var createAccountResult = await _accountService.CreateAccountAsync(createAccountRequest);

            var account = await _accountService.GetAccountAsync(createAccountResult.CreatedId);

            Assert.NotNull(account);
            Assert.Equal(VALID_ACCOUNT_NAME, account.AccountName);
        }

        [Fact]
        public async Task GetAccountByAccountNameAsync_ReturnsNull_WhenAccountDNE()
        {
            var account = await _accountService.GetAccountAsync(_fixture.Create<string>());

            Assert.Null(account);
        }

        [Fact]
        public async Task GetAccountByAccountNameAsync_ReturnsAccount_WhenAccountExists()
        {
            var createAccountRequest = new CreateAccountRequest(VALID_ACCOUNT_NAME, await CreateUserAsync());
            await _accountService.CreateAccountAsync(createAccountRequest);

            var account = await _accountService.GetAccountAsync(VALID_ACCOUNT_NAME);

            Assert.NotNull(account);
            Assert.Equal(VALID_ACCOUNT_NAME, account.AccountName);
        }
    }
}
