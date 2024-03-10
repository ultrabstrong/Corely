using Corely.Domain.Entities.Accounts;
using Corely.Domain.Exceptions;
using Corely.Domain.Mappers;
using Corely.Domain.Models.Accounts;
using Corely.Domain.Repos;
using Corely.Domain.Services.Accounts;
using Corely.Domain.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.Domain.Services.Accounts
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
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<AccountService>>());
        }

        [Fact]
        public async Task CreateAccountAsync_ShouldReturnAccountEntity_WhenValidAccountName()
        {
            var createAccountRequest = new CreateAccountRequest(VALID_ACCOUNT_NAME);
            await _accountService.CreateAccountAsync(createAccountRequest);

            var ex = await Record.ExceptionAsync(() => _accountService.CreateAccountAsync(createAccountRequest));

            Assert.NotNull(ex);
            Assert.IsType<AccountExistsException>(ex);
        }

        [Fact]
        public async Task CreateAccount_ShouldReturnCreateAccountResult()
        {
            var createAccountRequest = new CreateAccountRequest(VALID_ACCOUNT_NAME);
            var createAccountResult = await _accountService.CreateAccountAsync(createAccountRequest);

            Assert.True(createAccountResult.IsSuccess);
        }

        [Fact]
        public async Task CreateAccount_ShouldThrowArgumentNullException_WithNullRequest()
        {
            var ex = await Record.ExceptionAsync(() => _accountService.CreateAccountAsync(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenAccountRepoIsNull()
        {
            AccountService act() => new(
                null,
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<AccountService>>());

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenMapProviderIsNull()
        {
            AccountService act() => new(
                Mock.Of<IRepoExtendedGet<AccountEntity>>(),
                null,
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                _serviceFactory.GetRequiredService<ILogger<AccountService>>());

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenValidationProviderIsNull()
        {
            AccountService act() => new(
                Mock.Of<IRepoExtendedGet<AccountEntity>>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                null,
                _serviceFactory.GetRequiredService<ILogger<AccountService>>());

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            AccountService act() => new(
                Mock.Of<IRepoExtendedGet<AccountEntity>>(),
                _serviceFactory.GetRequiredService<IMapProvider>(),
                _serviceFactory.GetRequiredService<IValidationProvider>(),
                null);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}
