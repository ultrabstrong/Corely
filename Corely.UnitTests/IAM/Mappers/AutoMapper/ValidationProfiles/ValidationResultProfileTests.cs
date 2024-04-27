using CorelyValidationResult = Corely.IAM.Validators.ValidationResult;
using FluentValidationResult = FluentValidation.Results.ValidationResult;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.ValidationProfiles
{
    public class ValidationResultProfileTests
    {
        public class ToCorelyValidationResult
            : ProfileTestsBase<FluentValidationResult, CorelyValidationResult>
        {
        }
    }
}
