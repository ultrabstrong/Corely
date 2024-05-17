using Corely.IAM.Users.Constants;
using Corely.IAM.Users.Models;
using Corely.IAM.Validators.FluentValidators.Users;
using Corely.UnitTests.ClassData;
using FluentValidation.TestHelper;

namespace Corely.UnitTests.IAM.Validators.FluentValidators.Users
{
    public class UserValidatorTests
    {
        private const string VALID_USERNAME = "username";
        private const string VALID_EMAIL = "username@x.y";
        private readonly UserValidator _validator = new();

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [MemberData(nameof(InvalidUsernameData))]
        public void UserValidator_HasValidationError_WhenUsernameInvalid(string username)
        {
            var user = new User
            {
                Username = username,
                Email = VALID_EMAIL
            };

            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(x => x.Username);
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        public static IEnumerable<object[]> InvalidUsernameData() =>
        [
            [new string('a', UserConstants.USERNAME_MIN_LENGTH - 1)],
            [new string('a', UserConstants.USERNAME_MAX_LENGTH + 1)]
        ];

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [MemberData(nameof(InvalidEmailData))]
        public void UserValidator_HasValidationError_WhenEmailInvalid(string email)
        {
            var user = new User
            {
                Username = VALID_USERNAME,
                Email = email
            };

            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(x => x.Email);
            result.ShouldNotHaveValidationErrorFor(x => x.Username);
        }

        public static IEnumerable<object[]> InvalidEmailData() =>
        [
            [new string('a', UserConstants.EMAIL_MAX_LENGTH + 1)],
            ["invalid"]
        ];
    }
}
