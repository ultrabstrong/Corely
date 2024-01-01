using Corely.Domain.Constants.Users;
using Corely.Domain.Models.Users;
using Corely.Domain.Validators.FluentValidators.Users;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Domain.Validators.FluentValidators.Users
{
    public class UserValidatorTests
    {
        private readonly UserValidator _validator = new();

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [MemberData(nameof(InvalidUsernameData))]
        public void UserValidator_ShouldHaveValidationError_WhenUsernameInvalid(string username)
        {
            var user = new User
            {
                Username = username,
                Email = ""
            };

            var result = _validator.Validate(user);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Errors);
        }

        public static IEnumerable<object[]> InvalidUsernameData()
        {
            yield return new object[] { new string('a', UserConstants.USERNAME_MAX_LENGTH + 1) };
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [MemberData(nameof(InvalidEmailData))]
        public void UserValidator_ShouldHaveValidationError_WhenEmailInvalid(string email)
        {
            var user = new User
            {
                Username = "",
                Email = email
            };

            var result = _validator.Validate(user);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Errors);
        }

        public static IEnumerable<object[]> InvalidEmailData()
        {
            yield return new object[] { new string('a', UserConstants.EMAIL_MAX_LENGTH + 1) };
            yield return new object[] { "invalid" };
        }
    }
}
