using Corely.IAM.Validators;
using Corely.UnitTests.Mappers.AutoMapper;
using FluentValidation.Results;

namespace Corely.UnitTests.Validators.Mappers;

public class ValidationErrorProfileTests
    : ProfileTestsBase<ValidationFailure, ValidationError>
{
}
