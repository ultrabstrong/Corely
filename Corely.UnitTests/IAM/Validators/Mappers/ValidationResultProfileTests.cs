using Corely.UnitTests.IAM.Mappers.AutoMapper;
using CorelyValidationResult = Corely.IAM.Validators.ValidationResult;
using FluentValidationResult = FluentValidation.Results.ValidationResult;

namespace Corely.UnitTests.IAM.Validators.Mappers
{
    public class ValidationResultProfileTests
    {
        public class ToCorelyValidationResult
            : ProfileTestsBase<FluentValidationResult, CorelyValidationResult>
        {
        }
    }
}
