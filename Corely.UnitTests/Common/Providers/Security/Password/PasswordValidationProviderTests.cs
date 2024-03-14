using Corely.Common.Providers.Security.Password;

namespace Corely.UnitTests.Common.Providers.Security.Password
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
            var isValid = result.Contains(ValidatePasswordResult.Success);

            Assert.Equal(expectedIsValid, isValid);
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
                ValidatePasswordResult.TooShort,
                ValidatePasswordResult.NoUppercase,
                ValidatePasswordResult.NoLowercase,
                ValidatePasswordResult.NoDigit,
                ValidatePasswordResult.NoSpecialCharacter
            };

            var result = _provider.ValidatePassword(password);

            Assert.Equal(expectedResults.Length, result.Length);
            foreach (var expectedResult in expectedResults)
            {
                Assert.Contains(expectedResult, result);
            }
        }

        [Theory]
        [MemberData(nameof(ValidatePassword_ShouldFail_WithSingleFailure_Data))]
        public void ValidatePassword_ShouldFail_WithSingleFailure(string password, bool expectedIsValid, ValidatePasswordResult expectedResult)
        {
            var results = _provider.ValidatePassword(password);
            var isValid = results.Contains(ValidatePasswordResult.Success);
            Assert.Equal(expectedIsValid, isValid);

            if (!expectedIsValid)
            {
                Assert.Contains(expectedResult, results);
            }
        }
        public static IEnumerable<object[]> ValidatePassword_ShouldFail_WithSingleFailure_Data() =>
        [
             ["Short1!", false, ValidatePasswordResult.TooShort],
             ["nouppercase123!", false, ValidatePasswordResult.NoUppercase],
             ["NOLOWERCASE123!", false, ValidatePasswordResult.NoLowercase],
             ["NoNumberPassword!", false, ValidatePasswordResult.NoDigit],
             ["ValidPassword1", false, ValidatePasswordResult.NoSpecialCharacter],
             ["ValidPa$sword1", true, ValidatePasswordResult.Success]
        ];

        [Fact]
        public void ValidatePassword_ShouldFail_WithOnlyShortFailure()
        {
            var password = "Short1!";

            var result = _provider.ValidatePassword(password);

            Assert.Single(result);
            Assert.Equal(ValidatePasswordResult.TooShort, result[0]);
        }

        [Fact]
        public void ValidatePassword_ShouldFail_WithOnlyNoUppercaseFailure()
        {
            var password = "nouppercase123!";

            var result = _provider.ValidatePassword(password);

            Assert.Single(result);
            Assert.Equal(ValidatePasswordResult.NoUppercase, result[0]);
        }

        [Fact]
        public void ValidatePassword_ShouldFail_WithOnlyNoLowercaseFailure()
        {
            var password = "NOLOWERCASE123!";

            var result = _provider.ValidatePassword(password);

            Assert.Single(result);
            Assert.Equal(ValidatePasswordResult.NoLowercase, result[0]);
        }

        [Fact]
        public void ValidatePassword_ShouldFail_WithOnlyNoDigitFailure()
        {
            var password = "NoNumberPassword!";

            var result = _provider.ValidatePassword(password);

            Assert.Single(result);
            Assert.Equal(ValidatePasswordResult.NoDigit, result[0]);
        }

        [Fact]
        public void ValidatePassword_ShouldFail_WithOnlyNoSpecialCharacterFailure()
        {
            var password = "ValidPassword1";

            var result = _provider.ValidatePassword(password);

            Assert.Single(result);
            Assert.Equal(ValidatePasswordResult.NoSpecialCharacter, result[0]);
        }
    }
}
