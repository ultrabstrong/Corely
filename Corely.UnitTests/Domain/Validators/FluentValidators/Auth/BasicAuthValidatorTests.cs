﻿using Corely.Common.Models.Security;
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
