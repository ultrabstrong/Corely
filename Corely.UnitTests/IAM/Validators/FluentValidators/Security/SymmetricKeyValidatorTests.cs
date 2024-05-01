using Corely.IAM.Security.Constants;
using Corely.IAM.Security.Models;
using Corely.IAM.Validators.FluentValidators.Security;
using Corely.Security.Encryption.Models;
using Corely.Security.Encryption.Providers;
using Corely.UnitTests.ClassData;
using FluentValidation.TestHelper;

namespace Corely.UnitTests.IAM.Validators.FluentValidators.Security
{
    public class SymmetricKeyValidatorTests
    {
        private readonly SymmetricKeyValidator _validator = new();

        [Fact]
        public void SymmetricKeyValidator_ShouldHaveValidationError_WhenKeyIsNull()
        {
            var symmetricKey = new SymmetricKey
            {
                Key = null
            };

            var result = _validator.TestValidate(symmetricKey);
            result.ShouldHaveValidationErrorFor(x => x.Key);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [MemberData(nameof(InvalidKeyData))]
        public void SymmetricKeyValidator_ShouldHaveValidationError_WhenKeyInvalid(string key)
        {
            var symmetricKey = new SymmetricKey
            {
                Key = new SymmetricEncryptedValue(new AesEncryptionProvider())
                {
                    Secret = key
                }
            };

            var result = _validator.TestValidate(symmetricKey);
            result.ShouldHaveValidationErrorFor(x => x.Key.Secret);
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
                Version = SymmetricKeyConstants.VERSION_MIN_VALUE - 1,
                Key = new SymmetricEncryptedValue(new AesEncryptionProvider())
                {
                    Secret = "key"
                }
            };

            var result = _validator.TestValidate(symmetricKey);
            result.ShouldHaveValidationErrorFor(x => x.Version);
        }
    }
}
