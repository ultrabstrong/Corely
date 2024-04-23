using Corely.Security.Password;
using Corely.Security.PasswordValidation.Providers;

namespace Corely.UnitTests.Security.PasswordValidation.Providers
{
    public class PasswordValidationProviderTests
    {
        private readonly PasswordValidationProvider _provider = new()
        {
            MinimumLength = 8,
            RequireUppercase = true,
            RequireLowercase = true,
            RequireNumber = true,
            RequireSpecialCharacter = true
        };

        [Theory, MemberData(nameof(ValidatePassword_ShouldReturnExpectedResult_Data))]
        public void ValidatePassword_ShouldReturnExpectedResult(string password, bool expectedIsValid)
        {
            var result = _provider.ValidatePassword(password);

            Assert.Equal(expectedIsValid, result.IsSuccess);
        }

        public static IEnumerable<object[]> ValidatePassword_ShouldReturnExpectedResult_Data() =>
        [
            ["Short1", false],
            ["LongEnoughButNoDigitsOrUppercase1", false],
            ["nouppercase1", false],
            ["NOLOWERCASE1", false],
            ["NoDigitPassword!", false],
            ["ValidPa$sword1", true]
        ];

        [Fact]
        public void ValidatePassword_IdentifiesMultipleFailures()
        {
            var password = "";
            var expectedResults = new[]
            {
                PasswordValidationConstants.PASSWORD_TOO_SHORT,
                PasswordValidationConstants.PASSWORD_MISSING_UPPERCASE,
                PasswordValidationConstants.PASSWORD_MISSING_LOWERCASE,
                PasswordValidationConstants.PASSWORD_MISSING_DIGIT,
                PasswordValidationConstants.PASSWORD_MISSING_SPECIAL_CHARACTER
            };

            var result = _provider.ValidatePassword(password);

            Assert.Equal(expectedResults.Length, result.ValidationFailures.Length);
            foreach (var expectedResult in expectedResults)
            {
                Assert.Contains(expectedResult, result.ValidationFailures);
            }
        }

        [Fact]
        public void ValidatePassword_ShouldFail_WithOnlyShortFailure()
        {
            var password = "Short1!";

            var result = _provider.ValidatePassword(password);

            Assert.Single(result.ValidationFailures);
            Assert.Equal(PasswordValidationConstants.PASSWORD_TOO_SHORT, result.ValidationFailures[0]);
        }

        [Fact]
        public void ValidatePassword_ShouldFail_WithOnlyNoUppercaseFailure()
        {
            var password = "nouppercase123!";

            var result = _provider.ValidatePassword(password);

            Assert.Single(result.ValidationFailures);
            Assert.Equal(PasswordValidationConstants.PASSWORD_MISSING_UPPERCASE, result.ValidationFailures[0]);
        }

        [Fact]
        public void ValidatePassword_ShouldFail_WithOnlyNoLowercaseFailure()
        {
            var password = "NOLOWERCASE123!";

            var result = _provider.ValidatePassword(password);

            Assert.Single(result.ValidationFailures);
            Assert.Equal(PasswordValidationConstants.PASSWORD_MISSING_LOWERCASE, result.ValidationFailures[0]);
        }

        [Fact]
        public void ValidatePassword_ShouldFail_WithOnlyNoDigitFailure()
        {
            var password = "NoNumberPassword!";

            var result = _provider.ValidatePassword(password);

            Assert.Single(result.ValidationFailures);
            Assert.Equal(PasswordValidationConstants.PASSWORD_MISSING_DIGIT, result.ValidationFailures[0]);
        }

        [Fact]
        public void ValidatePassword_ShouldFail_WithOnlyNoSpecialCharacterFailure()
        {
            var password = "ValidPassword1";

            var result = _provider.ValidatePassword(password);

            Assert.Single(result.ValidationFailures);
            Assert.Equal(PasswordValidationConstants.PASSWORD_MISSING_SPECIAL_CHARACTER, result.ValidationFailures[0]);
        }
    }
}
