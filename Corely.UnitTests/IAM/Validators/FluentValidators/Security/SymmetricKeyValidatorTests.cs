using Corely.IAM.Security;
using Corely.IAM.Validators.FluentValidators.Security;
using Corely.UnitTests.ClassData;
using FluentValidation.TestHelper;

namespace Corely.UnitTests.IAM.Validators.FluentValidators.Security
{
    public class SymmetricKeyValidatorTests
    {
        /*public SymmetricKeyValidator()
        {
            RuleFor(m => m.Key)
                .NotNull()
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(m => m.Version)
                .GreaterThanOrEqualTo(0);
        }*/

        /* Template class for running validation tests
         * public class BasicAuthValidatorTests
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
        */
        private readonly SymmetricKeyValidator _validator = new();

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [MemberData(nameof(InvalidKeyData))]
        public void SymmetricKeyValidator_ShouldHaveValidationError_WhenKeyInvalid(string key)
        {
            var symmetricKey = new SymmetricKey
            {
                Key = key
            };

            var result = _validator.TestValidate(symmetricKey);
            result.ShouldHaveValidationErrorFor(x => x.Key);
        }

        public static IEnumerable<object[]> InvalidKeyData() =>
        [
            [new string('a', SymmetricKeyConstants.KEY_MAX_LENGTH + 1)]
        ];

        [Fact]
        public void SymmetricKeyValidator_ShouldHaveValidationError_WhenVersionIsNegative()
        {
            var symmetricKey = new SymmetricKey
            {
                Version = SymmetricKeyConstants.VERSION_MIN_VALUE - 1
            };

            var result = _validator.TestValidate(symmetricKey);
            result.ShouldHaveValidationErrorFor(x => x.Version);
        }
    }
}
