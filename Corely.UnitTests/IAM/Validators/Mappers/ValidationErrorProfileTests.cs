using Corely.IAM.Validators;
using Corely.UnitTests.IAM.Mappers.AutoMapper;
using FluentValidation.Results;

namespace Corely.UnitTests.IAM.Validators.Mappers;

public class ValidationErrorProfileTests
    : ProfileTestsBase<ValidationFailure, ValidationError>
{
}
