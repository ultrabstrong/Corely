using Corely.Common.Models.Security;
using Corely.Common.Providers.Security.Hashing;
using Corely.Domain.Constants.Auth;
using Corely.Domain.Models.Auth;
using Corely.Domain.Validators.FluentValidators.Auth;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Domain.Validators.FluentValidators.Auth
{
    public class BasicAuthValidatorTests
    {
        private readonly BasicAuthValidator _validator = new();

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [MemberData(nameof(InvalidUsernameData))]
        public void BasicAuthValidator_ShouldHaveValidationError_WhenUsernameInvalid(string username)
        {
            var basicAuth = new BasicAuth
            {
                Username = username,
                Password = new HashedValue(Mock.Of<IHashProvider>())
                {
                    Hash = ""
                }
            };

            var result = _validator.Validate(basicAuth);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Errors);
        }

        public static IEnumerable<object[]> InvalidUsernameData()
        {
            yield return new object[] { new string('a', BasicAuthConstants.USERNAME_MAX_LENGTH + 1) };
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [MemberData(nameof(InvalidPasswordData))]
        public void BasicAuthValidator_ShouldHaveValidationError_WhenPasswordInvalid(string password)
        {
            var basicAuth = new BasicAuth
            {
                Username = "",
                Password = new HashedValue(Mock.Of<IHashProvider>())
                {
                    Hash = password
                }
            };

            var result = _validator.Validate(basicAuth);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Errors);
        }

        public static IEnumerable<object[]> InvalidPasswordData()
        {
            yield return new object[] { new string('a', BasicAuthConstants.PASSWORD_MAX_LENGTH + 1) };
        }

        [Fact]
        public void BasicAuthValidator_ShouldHaveValidationError_WhenPasswordIsNull()
        {
            var basicAuth = new BasicAuth
            {
                Username = "",
                Password = null
            };

            var result = _validator.Validate(basicAuth);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Errors);
        }
    }
}
