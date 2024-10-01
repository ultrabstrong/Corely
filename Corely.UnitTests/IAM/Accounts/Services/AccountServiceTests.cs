using Corely.IAM.Accounts.Entities;
using Corely.IAM.Accounts.Exceptions;
using Corely.IAM.Accounts.Models;
using Corely.IAM.Accounts.Services;
using Corely.IAM.Mappers;
using Corely.IAM.Repos;
using Corely.IAM.Security.Services;
using Corely.IAM.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.Accounts.Services
{
    public class AccountServiceTests
    {
        private const string VALID_ACCOUNT_NAME = "accountname";

        private readonly ServiceFactory _serviceFactory = new();
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _accountService = new AccountService(
                _serviceFactory.GetRequiredService<IRepoExtendedGet<AccountEntity>>(),
                _serviceFactory.GetRequiredService<ISecurityService>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<AccountService>>());
        }

        [Fact]
        public async Task CreateAccountAsync_ReturnsAccountEntity_WhenValidAccountName()
        {
            var createAccountRequest = new CreateAccountRequest(VALID_ACCOUNT_NAME);
            await _accountService.CreateAccountAsync(createAccountRequest);

            var ex = await Record.ExceptionAsync(() => _accountService.CreateAccountAsync(createAccountRequest));

            Assert.NotNull(ex);
            Assert.IsType<AccountExistsException>(ex);
        }

        [Fact]
        public async Task CreateAccount_ReturnsCreateAccountResult()
        {
            var createAccountRequest = new CreateAccountRequest(VALID_ACCOUNT_NAME);
            var createAccountResult = await _accountService.CreateAccountAsync(createAccountRequest);

            Assert.True(createAccountResult.IsSuccess);
        }

        [Fact]
        public async Task CreateAccount_ThrowsArgumentNullException_WithNullRequest()
        {
            var ex = await Record.ExceptionAsync(() => _accountService.CreateAccountAsync(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task GetAccountAsync_ReturnsAccountEntity_WhenValidAccountName()
        {
            var createAccountRequest = new CreateAccountRequest(VALID_ACCOUNT_NAME);
            await _accountService.CreateAccountAsync(createAccountRequest);

            var getAccountRequest = GetAccountRequest.ForAccountName(VALID_ACCOUNT_NAME);
            var account = await _accountService.GetAccountAsync(getAccountRequest);

            Assert.NotNull(account);
            Assert.Equal(VALID_ACCOUNT_NAME, account!.AccountName);
        }

        [Fact]
        public async Task GetAccountAsync_ReturnsAccountEntity_WhenValidAccountId()
        {
            var createAccountRequest = new CreateAccountRequest(VALID_ACCOUNT_NAME);
            var createAccountResult = await _accountService.CreateAccountAsync(createAccountRequest);

            var getAccountRequest = GetAccountRequest.ForAccountId(createAccountResult.CreatedId);
            var account = await _accountService.GetAccountAsync(getAccountRequest);

            Assert.NotNull(account);
            Assert.Equal(VALID_ACCOUNT_NAME, account!.AccountName);
        }

        [Fact]
        public async Task GetAccountAsync_ReturnsNull_WhenInvalidAccountName()
        {
            var getAccountRequest = GetAccountRequest.ForAccountName(VALID_ACCOUNT_NAME);
            var account = await _accountService.GetAccountAsync(getAccountRequest);

            Assert.Null(account);
        }

        [Fact]
        public async Task GetAccountAsync_ReturnsNull_WhenInvalidAccountId()
        {
            var getAccountRequest = GetAccountRequest.ForAccountId(0);
            var account = await _accountService.GetAccountAsync(getAccountRequest);

            Assert.Null(account);
        }
    }
}
