using Corely.Domain.Validators;
using FluentValidation.Results;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper.Profiles
{
    public class ValidationErrorProfileTests
        : ProfileTestsBase<ValidationFailure, ValidationError>
    {
    }
}
