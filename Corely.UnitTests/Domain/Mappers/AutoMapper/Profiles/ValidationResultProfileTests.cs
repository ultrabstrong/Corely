﻿using CorelyValidationResult = Corely.Domain.Validators.ValidationResult;
using FluentValidationResult = FluentValidation.Results.ValidationResult;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper.Profiles
{
    public class ValidationResultProfileTests
    {
        public class ToCorelyValidationResult
            : ProfileTestsBase<FluentValidationResult, CorelyValidationResult>
        {
        }
    }
}
