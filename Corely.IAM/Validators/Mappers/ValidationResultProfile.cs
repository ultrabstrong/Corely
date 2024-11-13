using AutoMapper;
using CorelyValidationResult = Corely.IAM.Validators.ValidationResult;
using FluentValidationResult = FluentValidation.Results.ValidationResult;

namespace Corely.IAM.Validators.Mappers
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
