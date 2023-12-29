using AutoMapper;
using Corely.Common.Extensions;
using FluentValidation;

namespace Corely.Domain.Validators.FluentValidators
{
    public class FluentValidationProvider(IServiceProvider serviceProvider,
        IMapper mapper) : IValidationProvider
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider.ThrowIfNull(nameof(serviceProvider));
        private readonly IMapper _mapper = mapper.ThrowIfNull(nameof(mapper));

        public ValidationResult Validate<T>(T model)
        {
            if (_serviceProvider.GetService(typeof(IValidator<T>)) is not IValidator<T> validator)
            {
                throw new InvalidOperationException($"No validator found for type {typeof(T).Name}");
            }

            var result = validator.Validate(model);

            return _mapper.Map<ValidationResult>(result);
        }
    }
}
