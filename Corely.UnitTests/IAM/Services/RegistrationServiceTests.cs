using AutoFixture;
using Corely.DataAccess.Interfaces.UnitOfWork;
using Corely.IAM.Accounts.Models;
using Corely.IAM.Accounts.Processors;
using Corely.IAM.BasicAuths.Models;
using Corely.IAM.BasicAuths.Processors;
using Corely.IAM.Enums;
using Corely.IAM.Models;
using Corely.IAM.Services;
using Corely.IAM.Users.Models;
using Corely.IAM.Users.Processors;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.Services
{
    public class RegistrationServiceTests : ServiceBaseTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IUnitOfWorkProvider> _unitOfWorkProviderMock = new();
        private readonly Mock<IAccountProcessor> _accountProcessorMock;
        private readonly Mock<IUserProcessor> _userProcessorMock;
        private readonly Mock<IBasicAuthProcessor> _basicAuthProcessorMock;
        private readonly RegistrationService _registrationService;

        private bool _createAccountSuccess = true;
        private bool _createUserSuccess = true;
        private bool _createBasicAuthSuccess = true;

        public RegistrationServiceTests() : base()
        {
            _accountProcessorMock = GetMockAccountProcessor();
            _userProcessorMock = GetMockUserProcessor();
            _basicAuthProcessorMock = GetMockBasicAuthProcessor();

            _registrationService = new RegistrationService(
                _serviceFactory.GetRequiredService<ILogger<RegistrationService>>(),
                _accountProcessorMock.Object,
                _userProcessorMock.Object,
                _basicAuthProcessorMock.Object,
                _unitOfWorkProviderMock.Object);
        }

        private Mock<IAccountProcessor> GetMockAccountProcessor()
        {
            var accountProcessorMock = new Mock<IAccountProcessor>();

            accountProcessorMock
                .Setup(m => m.CreateAccountAsync(
                    It.IsAny<CreateAccountRequest>()))
                .ReturnsAsync(() =>
                    new CreateResult(_createAccountSuccess, string.Empty, _fixture.Create<int>()));

            return accountProcessorMock;
        }

        private Mock<IUserProcessor> GetMockUserProcessor()
        {
            var userProcessorMock = new Mock<IUserProcessor>();

            userProcessorMock
                .Setup(m => m.CreateUserAsync(
                    It.IsAny<CreateUserRequest>()))
                .ReturnsAsync(() =>
                    new CreateResult(_createUserSuccess, string.Empty, _fixture.Create<int>()));

            return userProcessorMock;
        }

        private Mock<IBasicAuthProcessor> GetMockBasicAuthProcessor()
        {
            var basicAuthProcessorMock = new Mock<IBasicAuthProcessor>();

            basicAuthProcessorMock
                .Setup(m => m.UpsertBasicAuthAsync(
                    It.IsAny<UpsertBasicAuthRequest>()))
                .ReturnsAsync(() =>
                    new UpsertBasicAuthResult(_createBasicAuthSuccess, string.Empty,
                        _fixture.Create<int>(), _fixture.Create<UpsertType>()));

            return basicAuthProcessorMock;
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsSuccessResult_WhenAllServicesSucceed()
        {
            var request = _fixture.Create<RegisterUserRequest>();

            var result = await _registrationService.RegisterUserAsync(request);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsFailureResult_WhenUserProcessorFails()
        {
            _createUserSuccess = false;
            var request = _fixture.Create<RegisterUserRequest>();

            var result = await _registrationService.RegisterUserAsync(request);

            Assert.False(result.IsSuccess);
            _basicAuthProcessorMock.Verify(m => m.UpsertBasicAuthAsync(It.IsAny<UpsertBasicAuthRequest>()), Times.Never);
            _unitOfWorkProviderMock.Verify(m => m.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsFailureResult_WhenBasicAuthProcessorFails()
        {
            _createBasicAuthSuccess = false;
            var request = _fixture.Create<RegisterUserRequest>();

            var result = await _registrationService.RegisterUserAsync(request);

            Assert.False(result.IsSuccess);
            _unitOfWorkProviderMock.Verify(m => m.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_Throws_WithNullRequest()
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
        public async Task RegisterAccountAsync_ReturnsFailureResult_WhenAccountProcessorFails()
        {
            _createAccountSuccess = false;
            var request = _fixture.Create<RegisterAccountRequest>();

            var result = await _registrationService.RegisterAccountAsync(request);

            Assert.False(result.IsSuccess);
            _unitOfWorkProviderMock.Verify(m => m.RollbackAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterAccountAsync_Throws_WithNullRequest()
        {
            var ex = await Record.ExceptionAsync(() => _registrationService.RegisterAccountAsync(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}
