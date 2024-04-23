using Corely.Security.Password;
using Corely.Security.PasswordValidation.Providers;

namespace Corely.UnitTests.Security.PasswordValidation
{
    public class PasswordValidationExceptionTests
    {
        private readonly PasswordValidationResult _validationResult = new(false, ["error"]);

        [Fact]
        public void DefaultConstructor_ShouldWork()
        {
            _ = new PasswordValidationException(_validationResult);
        }

        [Fact]
        public void MessageConstructor_ShouldWork()
        {
            _ = new PasswordValidationException(_validationResult, default);
        }

        [Fact]
        public void MessageInnerExceptionConstructor_ShouldWork()
        {
            _ = new PasswordValidationException(_validationResult, default, default);
        }
    }
}
