using Corely.IAM.Auth.Constants;
using Corely.IAM.Auth.Models;
using Corely.IAM.Validators.FluentValidators.Auth;
using Corely.Security.Hashing.Models;
using Corely.Security.Hashing.Providers;
using Corely.UnitTests.ClassData;
using FluentValidation.TestHelper;

namespace Corely.UnitTests.IAM.Validators.FluentValidators.Auth
{
    public class BasicAuthValidatorTests
    {
        private readonly BasicAuthValidator _validator = new();

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [MemberData(nameof(InvalidPasswordData))]
        public void BasicAuthValidator_ShouldHaveValidationError_WhenPasswordInvalid(string password)
        {
            var basicAuth = new BasicAuth
            {
                Password = new HashedValue(Mock.Of<IHashProvider>())
                {
                    Hash = password
                }
            };

            var result = _validator.TestValidate(basicAuth);
            result.ShouldHaveValidationErrorFor(x => x.Password.Hash);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
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
                Password = null
            };

            var result = _validator.TestValidate(basicAuth);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
    }
}
