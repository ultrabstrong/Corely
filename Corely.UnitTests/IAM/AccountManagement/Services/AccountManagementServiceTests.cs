using AutoFixture;
using Corely.IAM.AccountManagement.Models;
using Corely.IAM.AccountManagement.Services;
using Corely.IAM.Accounts.Models;
using Corely.IAM.Accounts.Services;
using Corely.IAM.Auth.Models;
using Corely.IAM.Auth.Services;
using Corely.IAM.Enums;
using Corely.IAM.Models;
using Corely.IAM.Repos;
using Corely.IAM.Users.Models;
using Corely.IAM.Users.Services;
using Corely.UnitTests.IAM.Services;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.AccountManagement.Services
{
    public class AccountManagementServiceTests : ServiceBaseTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IUnitOfWorkProvider> _unitOfWorkProviderMock = new();
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AccountManagementService _accountManagementService;

        private readonly User _user;
        private bool _createAccountSuccess = true;
        private bool _createUserSuccess = true;
        private bool _createAuthSuccess = true;

        public AccountManagementServiceTests() : base()
        {
            _user = _fixture.Create<User>();
            _accountServiceMock = GetMockAccountService();
            _userServiceMock = GetMockUserService();
            _authServiceMock = GetMockAuthService();

            _accountManagementService = new AccountManagementService(
                _serviceFactory.GetRequiredService<ILogger<AccountManagementService>>(),
                _accountServiceMock.Object,
                _userServiceMock.Object,
                _authServiceMock.Object,
                _unitOfWorkProviderMock.Object);
        }

        private Mock<IAccountService> GetMockAccountService()
        {
            var accountServiceMock = new Mock<IAccountService>();

            accountServiceMock
                .Setup(m => m.CreateAccountAsync(
                    It.IsAny<CreateAccountRequest>()))
                .ReturnsAsync(() =>
                    new CreateResult(_createAccountSuccess, string.Empty, _fixture.Create<int>()));

            return accountServiceMock;
        }

        private Mock<IUserService> GetMockUserService()
        {
            var userServiceMock = new Mock<IUserService>();

            userServiceMock
                .Setup(m => m.CreateUserAsync(
                    It.IsAny<CreateUserRequest>()))
                .ReturnsAsync(() =>
                    new CreateResult(_createUserSuccess, string.Empty, _fixture.Create<int>()));

            userServiceMock
                .Setup(m => m.GetUserAsync(
                    It.IsAny<string>()))
                .ReturnsAsync(() => _user);

            userServiceMock
                .Setup(m => m.GetUserAsync(
                    It.IsAny<int>()))
                .ReturnsAsync(() => _user);

            return userServiceMock;
        }

        private Mock<IAuthService> GetMockAuthService()
        {
            var authServiceMock = new Mock<IAuthService>();

            authServiceMock
                .Setup(m => m.UpsertBasicAuthAsync(
                    It.IsAny<UpsertBasicAuthRequest>()))
                .ReturnsAsync(() =>
                    new UpsertBasicAuthResult(_createAuthSuccess, string.Empty,
                        _fixture.Create<int>(), _fixture.Create<UpsertType>()));
            authServiceMock
                .Setup(m => m.VerifyBasicAuthAsync(
                    It.IsAny<VerifyBasicAuthRequest>()))
                .ReturnsAsync(true);

            return authServiceMock;
        }

        [Fact]
        public async Task RegisterAsync_ReturnsSuccessResult_WhenAllServicesSucceed()
        {
            var request = _fixture.Create<RegisterRequest>();

            var result = await _accountManagementService.RegisterAsync(request);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsFailureResult_WhenAccountServiceFails()
        {
            _createAccountSuccess = false;
            var request = _fixture.Create<RegisterRequest>();

            var result = await _accountManagementService.RegisterAsync(request);

            Assert.False(result.IsSuccess);
            _userServiceMock.Verify(m => m.CreateUserAsync(It.IsAny<CreateUserRequest>()), Times.Never);
            _authServiceMock.Verify(m => m.UpsertBasicAuthAsync(It.IsAny<UpsertBasicAuthRequest>()), Times.Never);
            _unitOfWorkProviderMock.Verify(m => m.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsFailureResult_WhenUserServiceFails()
        {
            _createUserSuccess = false;
            var request = _fixture.Create<RegisterRequest>();

            var result = await _accountManagementService.RegisterAsync(request);

            Assert.False(result.IsSuccess);
            _authServiceMock.Verify(m => m.UpsertBasicAuthAsync(It.IsAny<UpsertBasicAuthRequest>()), Times.Never);
            _unitOfWorkProviderMock.Verify(m => m.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsFailureResult_WhenAuthServiceFails()
        {
            _createAuthSuccess = false;
            var request = _fixture.Create<RegisterRequest>();

            var result = await _accountManagementService.RegisterAsync(request);

            Assert.False(result.IsSuccess);
            _unitOfWorkProviderMock.Verify(m => m.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ThrowsArgumentNullException_WithNullRequest()
        {
            var ex = await Record.ExceptionAsync(() => _accountManagementService.RegisterAsync(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task SignInAsync_ReturnsSuccessResultAndUpdateSuccessfulLogin_WhenUserExistsAndPasswordIsValid()
        {
            var request = new SignInRequest(_user.Username, _fixture.Create<string>());
            _user.TotalSuccessfulLogins = 0;
            _user.LastSuccessfulLoginUtc = null;
            _user.TotalFailedLogins = 0;
            _user.FailedLoginsSinceLastSuccess = 0;
            _user.LastFailedLoginUtc = null;

            var result = await _accountManagementService.SignInAsync(request);

            Assert.True(result.IsSuccess);

            _userServiceMock
                .Verify(m => m.UpdateUserAsync(It.Is<User>(u =>
                    HasUpdatedSuccessLogins(u))),
                Times.Once);
        }

        private static bool HasUpdatedSuccessLogins(User modified)
        {
            Assert.Equal(1, modified.TotalSuccessfulLogins);
            Assert.NotNull(modified.LastSuccessfulLoginUtc);
            Assert.True((DateTime.UtcNow - modified.LastSuccessfulLoginUtc).Value.TotalSeconds < 5);
            Assert.Equal(0, modified.TotalFailedLogins);
            Assert.Equal(0, modified.FailedLoginsSinceLastSuccess);
            Assert.Null(modified.LastFailedLoginUtc);
            return true;
        }

        [Fact]
        public async Task SignInAsync_ReturnsFailureResult_WhenUserDoesNotExist()
        {
            var request = _fixture.Create<SignInRequest>();

            _userServiceMock
                .Setup(m => m.GetUserAsync(request.Username))
                .ReturnsAsync((User)null!);

            var result = await _accountManagementService.SignInAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Equal("User not found", result.Message);
            Assert.Equal(string.Empty, result.AuthToken);
        }

        [Fact]
        public async Task SignInAsync_ReturnsFailureResultAndUpdatedFailedLogins_WhenPasswordIsInvalid()
        {
            var request = new SignInRequest(_user.Username, _fixture.Create<string>());
            _user.TotalSuccessfulLogins = 0;
            _user.LastSuccessfulLoginUtc = null;
            _user.TotalFailedLogins = 0;
            _user.FailedLoginsSinceLastSuccess = 0;
            _user.LastFailedLoginUtc = null;

            _authServiceMock
                .Setup(m => m.VerifyBasicAuthAsync(
                    It.IsAny<VerifyBasicAuthRequest>()))
                .ReturnsAsync(false);

            var result = await _accountManagementService.SignInAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid password", result.Message);
            Assert.Equal(string.Empty, result.AuthToken);

            _userServiceMock
                .Verify(m => m.UpdateUserAsync(It.Is<User>(u =>
                    HasUpdatedFailedLogins(u))),
                Times.Once);
        }

        private static bool HasUpdatedFailedLogins(User modified)
        {
            Assert.Equal(0, modified.TotalSuccessfulLogins);
            Assert.Null(modified.LastSuccessfulLoginUtc);
            Assert.Equal(1, modified.TotalFailedLogins);
            Assert.Equal(1, modified.FailedLoginsSinceLastSuccess);
            Assert.NotNull(modified.LastFailedLoginUtc);
            Assert.True((DateTime.UtcNow - modified.LastFailedLoginUtc).Value.TotalSeconds < 5);
            return true;
        }

        [Fact]
        public async Task SignInAsync_ThrowsArgumentNullException_WithNullRequest()
        {
            var ex = await Record.ExceptionAsync(() => _accountManagementService.SignInAsync(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}
