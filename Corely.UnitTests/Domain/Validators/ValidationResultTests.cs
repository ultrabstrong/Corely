using Corely.Domain.Exceptions;
using Corely.Domain.Validators;

namespace Corely.UnitTests.Domain.Validators
{
    public class ValidationResultTests
    {
        [Fact]
        public void IsValid_ShouldReturnFalse_WhenErrorsIsNotNull()
        {
            var result = new ValidationResult
            {
                Errors = [new()]
            };

            Assert.False(result.IsValid);
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenErrorsIsNull()
        {
            var result = new ValidationResult
            {
                Errors = null
            };

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ThrowIfInvalid_ShouldThrowValidationException_WhenErrorsIsNotNull()
        {
            var result = new ValidationResult
            {
                Errors = [new()]
            };

            var ex = Record.Exception(result.ThrowIfInvalid);

            Assert.False(result.IsValid);
            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
        }

        [Fact]
        public void ThrowIfInvalid_ShouldNotThrowValidationException_WhenErrorsIsNull()
        {
            var result = new ValidationResult();
            Assert.True(result.IsValid);
            result.ThrowIfInvalid();
        }
    }
}
