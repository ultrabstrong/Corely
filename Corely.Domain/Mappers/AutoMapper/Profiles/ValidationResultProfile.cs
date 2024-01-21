using AutoMapper;
using CorelyValidationResult = Corely.Domain.Validators.ValidationResult;
using FluentValidationResult = FluentValidation.Results.ValidationResult;

namespace Corely.Domain.Mappers.AutoMapper.Profiles
{
    internal sealed class ValidationResultProfile : Profile
    {
        public ValidationResultProfile()
        {
            CreateMap<FluentValidationResult, CorelyValidationResult>()
                .ForMember(dest => dest.Message, opt => opt.Ignore());
        }
    }
}
