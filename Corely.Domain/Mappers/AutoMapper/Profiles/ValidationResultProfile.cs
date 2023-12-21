using AutoMapper;
using Corely.Domain.Validators;
using System.Text.Json;

namespace Corely.Domain.Mappers.AutoMapper.Profiles
{
    public class ValidationResultProfile : Profile
    {
        public ValidationResultProfile()
        {
            CreateMap<FluentValidation.Results.ValidationResult, ValidationResult>()
                .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => SerializeErrors(src.Errors)));
        }

        private static List<string> SerializeErrors(IEnumerable<FluentValidation.Results.ValidationFailure> errors)
        {
            return errors.Select(e => JsonSerializer.Serialize(e)).ToList();
        }
    }
}
