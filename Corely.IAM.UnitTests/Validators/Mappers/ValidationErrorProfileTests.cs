using Corely.IAM.Validators;
using Corely.IAM.UnitTests.Mappers.AutoMapper;
using FluentValidation.Results;

namespace Corely.IAM.UnitTests.Validators.Mappers;

public class ValidationErrorProfileTests
    : ProfileTestsBase<ValidationFailure, ValidationError>
{
}
