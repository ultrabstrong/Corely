using AutoFixture;
using Corely.Domain.Enums;
using Corely.Domain.Models;
using Corely.Domain.Models.AccountManagement;
using Corely.Domain.Models.Accounts;
using Corely.Domain.Models.Auth;
using Corely.Domain.Models.Users;
using Corely.Domain.Repos;
using Corely.Domain.Services.AccountManagement;
using Corely.Domain.Services.Accounts;
using Corely.Domain.Services.Auth;
using Corely.Domain.Services.Users;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.Domain.Services.AccountManagement
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
            var request = _fixture.Create<SignUpRequest>();

            var result = await _accountManagementService.SignUpAsync(request);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task SignUpAsync_ShouldReturnFailureResult_WhenAccountServiceFails()
        {
            _createAccountSuccess = false;
            var request = _fixture.Create<SignUpRequest>();

            var result = await _accountManagementService.SignUpAsync(request);

            Assert.False(result.IsSuccess);
            _userServiceMock.Verify(m => m.CreateUserAsync(It.IsAny<CreateUserRequest>()), Times.Never);
            _authServiceMock.Verify(m => m.UpsertBasicAuthAsync(It.IsAny<UpsertBasicAuthRequest>()), Times.Never);
        }

        [Fact]
        public async Task SignUpAsync_ShouldReturnFailureResult_WhenUserServiceFails()
        {
            _createUserSuccess = false;
            var request = _fixture.Create<SignUpRequest>();

            var result = await _accountManagementService.SignUpAsync(request);

            Assert.False(result.IsSuccess);
            _authServiceMock.Verify(m => m.UpsertBasicAuthAsync(It.IsAny<UpsertBasicAuthRequest>()), Times.Never);
        }

        [Fact]
        public async Task SignUpAsync_ShouldReturnFailureResult_WhenAuthServiceFails()
        {
            _createAuthSuccess = false;
            var request = _fixture.Create<SignUpRequest>();

            var result = await _accountManagementService.SignUpAsync(request);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task SignUpAsync_ShouldThrowArgumentNullException_WithNullRequest()
        {
            var ex = await Record.ExceptionAsync(() => _accountManagementService.SignUpAsync(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task SignUpAsync_ShouldReturnFalseResult_OnError()
        {
            var unitOfWorkProviderMock = new Mock<IUnitOfWorkProvider>();
            unitOfWorkProviderMock
                .Setup(m => m.BeginAsync())
                .ThrowsAsync(new Exception());

            var accountManagementServiceFactory = new AccountManagementService(
                Mock.Of<ILogger<AccountManagementService>>(),
                _accountServiceMock.Object,
                _userServiceMock.Object,
                _authServiceMock.Object,
                unitOfWorkProviderMock.Object);

            var request = _fixture.Create<SignUpRequest>();

            var result = await accountManagementServiceFactory.SignUpAsync(request);

            Assert.False(result.IsSuccess);
        }
    }
}
