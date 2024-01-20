using AutoMapper;
using Corely.Domain.Validators;
using FluentValidation.Results;

namespace Corely.Domain.Mappers.AutoMapper.Profiles
{
    internal sealed class ValidationErrorProfile : Profile
    {
        public ValidationErrorProfile()
        {
            CreateMap<ValidationFailure, ValidationError>()
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.ErrorMessage))
                .ForMember(dest => dest.PropertyValue, opt => opt.MapFrom(src => src.AttemptedValue));
        }
    }
}
