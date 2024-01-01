using AutoMapper;

namespace Corely.Domain.Validators.FluentValidators
{
    public class FluentValidationProvider : IValidationProvider
    {
        private readonly IFluentValidatorFactory _fluentValidatorFactory;
        private readonly IMapper _mapper;

        public FluentValidationProvider(IFluentValidatorFactory fluentValidatorFactory, IMapper mapper)
        {
            _fluentValidatorFactory = fluentValidatorFactory;
            _mapper = mapper;
        }

        public ValidationResult Validate<T>(T model)
        {
            var validator = _fluentValidatorFactory.GetValidator<T>();
            var result = validator.Validate(model);

            return _mapper.Map<ValidationResult>(result);
        }

        public void ThrowIfInvalid<T>(T model)
        {
            Validate(model).ThrowIfInvalid();
        }
    }
}
