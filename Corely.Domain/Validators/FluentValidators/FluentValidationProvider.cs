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
            ValidationResult corelyResult = null!;
            if (model == null)
            {
                corelyResult = new()
                {
                    Errors = ["Model is null"]
                };
            }
            else
            {
                var validator = _fluentValidatorFactory.GetValidator<T>();
                var fluentResult = validator.Validate(model);
                corelyResult = _mapper.Map<ValidationResult>(fluentResult);
            }

            corelyResult.Message = $"Validation for {typeof(T).Name} {(corelyResult.IsValid ? "succeeded" : "failed")}";

            return corelyResult;
        }

        public void ThrowIfInvalid<T>(T model)
        {
            Validate(model).ThrowIfInvalid();
        }
    }
}
