using Corely.Common.Models.Security;
using Corely.Common.Providers.Security.Hashing;
using Corely.Domain.Constants.Auth;
using Corely.Domain.Models.Auth;
using Corely.Domain.Validators.FluentValidators.Auth;
using Corely.UnitTests.ClassData;
using FluentValidation.TestHelper;

namespace Corely.UnitTests.Domain.Validators.FluentValidators.Auth
{
    public class BasicAuthValidatorTests
    {
        private const string VALID_USERNAME = "username";
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

            var result = _validator.TestValidate(basicAuth);
            result.ShouldHaveValidationErrorFor(x => x.Username);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        public static IEnumerable<object[]> InvalidUsernameData() =>
        [
            [new string('a', BasicAuthConstants.USERNAME_MIN_LENGTH - 1)],
            [new string('a', BasicAuthConstants.USERNAME_MAX_LENGTH + 1)]
        ];

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [MemberData(nameof(InvalidPasswordData))]
        public void BasicAuthValidator_ShouldHaveValidationError_WhenPasswordInvalid(string password)
        {
            var basicAuth = new BasicAuth
            {
                Username = VALID_USERNAME,
                Password = new HashedValue(Mock.Of<IHashProvider>())
                {
                    Hash = password
                }
            };

            var result = _validator.TestValidate(basicAuth);
            result.ShouldHaveValidationErrorFor(x => x.Password.Hash);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
            result.ShouldNotHaveValidationErrorFor(x => x.Username);
        }

        public static IEnumerable<object[]> InvalidPasswordData() =>
        [
            [new string('a', BasicAuthConstants.PASSWORD_MAX_LENGTH + 1)]
        ];

        [Fact]
        public void BasicAuthValidator_ShouldHaveValidationError_WhenPasswordIsNull()
        {
            var basicAuth = new BasicAuth
            {
                Username = VALID_USERNAME,
                Password = null
            };

            var result = _validator.TestValidate(basicAuth);
            result.ShouldHaveValidationErrorFor(x => x.Password);
            result.ShouldNotHaveValidationErrorFor(x => x.Username);
        }
    }
}
