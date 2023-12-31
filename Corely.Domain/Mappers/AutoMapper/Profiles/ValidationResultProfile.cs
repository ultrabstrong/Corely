using AutoMapper;
using Corely.Domain.Mappers.AutoMapper.ValueConverters;
using FluentValidation.Results;
using CorelyValidationResult = Corely.Domain.Validators.ValidationResult;
using FluentValidationResult = FluentValidation.Results.ValidationResult;

namespace Corely.Domain.Mappers.AutoMapper.Profiles
{
    public sealed class ValidationResultProfile : Profile
    {
        public ValidationResultProfile()
        {
            CreateMap<FluentValidationResult, CorelyValidationResult>()
                .ForMember(dest => dest.Errors, opt => opt
                    .ConvertUsing<JsonifyListValueConverter<ValidationFailure>, List<ValidationFailure>>(src => src.Errors))
                .ForMember(dest => dest.Message, opt => opt.Ignore());
        }
    }
}
