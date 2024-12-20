﻿using AutoMapper;
using Corely.IAM.Validators;
using FluentValidation.Results;

namespace Corely.IAM.Validators.Mappers;

internal sealed class ValidationErrorProfile : Profile
{
    public ValidationErrorProfile()
    {
        CreateMap<ValidationFailure, ValidationError>()
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.ErrorMessage));
    }
}
