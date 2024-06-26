﻿using Corely.IAM.Auth.Constants;
using Corely.IAM.Auth.Models;
using FluentValidation;

namespace Corely.IAM.Validators.FluentValidators.Auth
{
    internal class BasicAuthValidator : AbstractValidator<BasicAuth>
    {
        public BasicAuthValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(m => m.Password)
                .NotNull();
            RuleFor(m => m.Password.Hash)
                .NotEmpty()
                .MaximumLength(BasicAuthConstants.PASSWORD_MAX_LENGTH);
        }
    }
}
