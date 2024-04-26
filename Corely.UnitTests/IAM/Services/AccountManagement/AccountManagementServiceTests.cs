using AutoFixture;
using Corely.IAM.Accounts.Models;
using Corely.IAM.Accounts.Services;
using Corely.IAM.Auth.Models;
using Corely.IAM.Auth.Services;
using Corely.IAM.Enums;
using Corely.IAM.Models;
using Corely.IAM.Models.AccountManagement;
using Corely.IAM.Models.Users;
using Corely.IAM.Repos;
using Corely.IAM.Services.AccountManagement;
using Corely.IAM.Services.Users;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.Services.AccountManagement
{
    public class AccountManagementServiceTests : ServiceBaseTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AccountManagementService _accountManagementService;
        private bool _createAccountSuccess = true;
        private bool _createUserSuccess = true;
        private bool _createAuthSuccess = true;

        public AccountManagementServiceTests() : base()
        {
            _accountServiceMock = GetMockAccountService();
            _userServiceMock = GetMockUserService();
            _authServiceMock = GetMockAuthService();

            _accountManagementService = new AccountManagementService(
                _serviceFactory.GetRequiredService<ILogger<AccountManagementService>>(),
                _accountServiceMock.Object,
                _userServiceMock.Object,
                _authServiceMock.Object,
                Mock.Of<IUnitOfWorkProvider>());
        }

        private Mock<IAccountService> GetMockAccountService()
        {
            var accountServiceMock = new Mock<IAccountService>();

            accountServiceMock
                .Setup(m => m.CreateAccountAsync(
                    It.IsAny<CreateAccountRequest>()))
                .ReturnsAsync(() =>
                    new CreateResult(_createAccountSuccess, "", _fixture.Create<int>()));

            return accountServiceMock;
        }

        private Mock<IUserService> GetMockUserService()
        {
            var userServiceMock = new Mock<IUserService>();

            userServiceMock
                .Setup(m => m.CreateUserAsync(
                    It.IsAny<CreateUserRequest>()))
                .ReturnsAsync(() =>
                    new CreateResult(_createUserSuccess, "", _fixture.Create<int>()));

            return userServiceMock;
        }

        private Mock<IAuthService> GetMockAuthService()
        {
            var authServiceMock = new Mock<IAuthService>();

            authServiceMock
                .Setup(m => m.UpsertBasicAuthAsync(
                    It.IsAny<UpsertBasicAuthRequest>()))
                .ReturnsAsync(() =>
                    new UpsertBasicAuthResult(_createAuthSuccess, "",
                        _fixture.Create<int>(), _fixture.Create<UpsertType>()));

            return authServiceMock;
        }

        [Fact]
        public async Task SignUpAsync_ShouldReturnSuccessResult_WhenAllServicesSucceed()
        {
            var request = _fixture.Create<RegisterRequest>();

            var result = await _accountManagementService.RegisterAsync(request);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task SignUpAsync_ShouldReturnFailureResult_WhenAccountServiceFails()
        {
            _createAccountSuccess = false;
            var request = _fixture.Create<RegisterRequest>();

            var result = await _accountManagementService.RegisterAsync(request);

            Assert.False(result.IsSuccess);
            _userServiceMock.Verify(m => m.CreateUserAsync(It.IsAny<CreateUserRequest>()), Times.Never);
            _authServiceMock.Verify(m => m.UpsertBasicAuthAsync(It.IsAny<UpsertBasicAuthRequest>()), Times.Never);
        }

        [Fact]
        public async Task SignUpAsync_ShouldReturnFailureResult_WhenUserServiceFails()
        {
            _createUserSuccess = false;
            var request = _fixture.Create<RegisterRequest>();

            var result = await _accountManagementService.RegisterAsync(request);

            Assert.False(result.IsSuccess);
            _authServiceMock.Verify(m => m.UpsertBasicAuthAsync(It.IsAny<UpsertBasicAuthRequest>()), Times.Never);
        }

        [Fact]
        public async Task SignUpAsync_ShouldReturnFailureResult_WhenAuthServiceFails()
        {
            _createAuthSuccess = false;
            var request = _fixture.Create<RegisterRequest>();

            var result = await _accountManagementService.RegisterAsync(request);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task SignUpAsync_ShouldThrowArgumentNullException_WithNullRequest()
        {
            var ex = await Record.ExceptionAsync(() => _accountManagementService.RegisterAsync(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
        {
            AccountManagementService act() => new(
                null,
                _accountServiceMock.Object,
                _userServiceMock.Object,
                _authServiceMock.Object,
                Mock.Of<IUnitOfWorkProvider>());

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenAccountServiceIsNull()
        {
            AccountManagementService act() => new(
                _serviceFactory.GetRequiredService<ILogger<AccountManagementService>>(),
                null,
                _userServiceMock.Object,
                _authServiceMock.Object,
                Mock.Of<IUnitOfWorkProvider>());

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenUserServiceIsNull()
        {
            AccountManagementService act() => new(
                _serviceFactory.GetRequiredService<ILogger<AccountManagementService>>(),
                _accountServiceMock.Object,
                null,
                _authServiceMock.Object,
                Mock.Of<IUnitOfWorkProvider>());

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenAuthServiceIsNull()
        {
            AccountManagementService act() => new(
                _serviceFactory.GetRequiredService<ILogger<AccountManagementService>>(),
                _accountServiceMock.Object,
                _userServiceMock.Object,
                null,
                Mock.Of<IUnitOfWorkProvider>());

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenUnitOfWorkProviderIsNull()
        {
            AccountManagementService act() => new(
                _serviceFactory.GetRequiredService<ILogger<AccountManagementService>>(),
                _accountServiceMock.Object,
                _userServiceMock.Object,
                _authServiceMock.Object,
                null);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}
