using AutoFixture;
using Corely.DataAccess.Interfaces.UnitOfWork;
using Corely.IAM.Accounts.Models;
using Corely.IAM.Accounts.Services;
using Corely.IAM.BasicAuths.Models;
using Corely.IAM.BasicAuths.Services;
using Corely.IAM.Enums;
using Corely.IAM.Models;
using Corely.IAM.Services;
using Corely.IAM.Users.Models;
using Corely.IAM.Users.Services;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.Services
{
    public class RegistrationServiceTests : ServiceBaseTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IUnitOfWorkProvider> _unitOfWorkProviderMock = new();
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IBasicAuthService> _authServiceMock;
        private readonly RegistrationService _registrationService;

        private bool _createAccountSuccess = true;
        private bool _createUserSuccess = true;
        private bool _createAuthSuccess = true;

        public RegistrationServiceTests() : base()
        {
            _accountServiceMock = GetMockAccountService();
            _userServiceMock = GetMockUserService();
            _authServiceMock = GetMockAuthService();

            _registrationService = new RegistrationService(
                _serviceFactory.GetRequiredService<ILogger<RegistrationService>>(),
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

            return userServiceMock;
        }

        private Mock<IBasicAuthService> GetMockAuthService()
        {
            var authServiceMock = new Mock<IBasicAuthService>();

            authServiceMock
                .Setup(m => m.UpsertBasicAuthAsync(
                    It.IsAny<UpsertBasicAuthRequest>()))
                .ReturnsAsync(() =>
                    new UpsertBasicAuthResult(_createAuthSuccess, string.Empty,
                        _fixture.Create<int>(), _fixture.Create<UpsertType>()));

            return authServiceMock;
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsSuccessResult_WhenAllServicesSucceed()
        {
            var request = _fixture.Create<RegisterUserRequest>();

            var result = await _registrationService.RegisterUserAsync(request);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsFailureResult_WhenUserServiceFails()
        {
            _createUserSuccess = false;
            var request = _fixture.Create<RegisterUserRequest>();

            var result = await _registrationService.RegisterUserAsync(request);

            Assert.False(result.IsSuccess);
            _authServiceMock.Verify(m => m.UpsertBasicAuthAsync(It.IsAny<UpsertBasicAuthRequest>()), Times.Never);
            _unitOfWorkProviderMock.Verify(m => m.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsFailureResult_WhenAuthServiceFails()
        {
            _createAuthSuccess = false;
            var request = _fixture.Create<RegisterUserRequest>();

            var result = await _registrationService.RegisterUserAsync(request);

            Assert.False(result.IsSuccess);
            _unitOfWorkProviderMock.Verify(m => m.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_ThrowsArgumentNullException_WithNullRequest()
        {
            var ex = await Record.ExceptionAsync(() => _registrationService.RegisterUserAsync(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task RegisterAccountAsync_ReturnsSuccessResult_WhenAllServicesSucceed()
        {
            var request = _fixture.Create<RegisterAccountRequest>();

            var result = await _registrationService.RegisterAccountAsync(request);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task RegisterAccountAsync_ReturnsFailureResult_WhenAccountServiceFails()
        {
            _createAccountSuccess = false;
            var request = _fixture.Create<RegisterAccountRequest>();

            var result = await _registrationService.RegisterAccountAsync(request);

            Assert.False(result.IsSuccess);
            _unitOfWorkProviderMock.Verify(m => m.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterAccountAsync_ThrowsArgumentNullException_WithNullRequest()
        {
            var ex = await Record.ExceptionAsync(() => _registrationService.RegisterAccountAsync(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}
