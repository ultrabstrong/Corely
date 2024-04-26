using Corely.IAM.Validators;
using FluentValidation.Results;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.Profiles
{
    public class ValidationErrorProfileTests
        : ProfileTestsBase<ValidationFailure, ValidationError>
    {
    }
}
