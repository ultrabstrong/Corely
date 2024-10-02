using AutoFixture;
using Corely.IAM.Auth.Models;
using Corely.IAM.Auth.Services;
using Corely.IAM.Models;
using Corely.IAM.Services;
using Corely.IAM.Users.Models;
using Corely.IAM.Users.Services;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.Services
{
    public class SignInServiceTests : ServiceBaseTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly SignInService _signInService;

        private readonly User _user;

        public SignInServiceTests() : base()
        {
            _user = _fixture.Build<User>()
                .Without(u => u.SymmetricKey)
                .Without(u => u.AsymmetricKey)
                .Create();

            _userServiceMock = GetMockUserService();
            _authServiceMock = GetMockAuthService();

            _signInService = new SignInService(
                _serviceFactory.GetRequiredService<ILogger<SignInService>>(),
                _userServiceMock.Object,
                _authServiceMock.Object);
        }

        private Mock<IUserService> GetMockUserService()
        {
            var userServiceMock = new Mock<IUserService>();

            userServiceMock
                .Setup(m => m.GetUserAsync(
                    It.IsAny<string>()))
                .ReturnsAsync(() => _user);

            return userServiceMock;
        }

        private static Mock<IAuthService> GetMockAuthService()
        {
            var authServiceMock = new Mock<IAuthService>();

            authServiceMock
                .Setup(m => m.VerifyBasicAuthAsync(
                    It.IsAny<VerifyBasicAuthRequest>()))
                .ReturnsAsync(true);

            return authServiceMock;
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

            var result = await _signInService.SignInAsync(request);

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

            var result = await _signInService.SignInAsync(request);

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

            var result = await _signInService.SignInAsync(request);

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
            var ex = await Record.ExceptionAsync(() => _signInService.SignInAsync(null!));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}
