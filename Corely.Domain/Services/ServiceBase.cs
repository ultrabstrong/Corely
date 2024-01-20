using Corely.Common.Extensions;
using Corely.Domain.Exceptions;
using Corely.Domain.Mappers;
using Corely.Domain.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.Domain.Services
{
    public abstract class ServiceBase
    {
        protected readonly IValidationProvider validationProvider;
        protected readonly IMapProvider mapProvider;
        protected readonly ILogger logger;

        public ServiceBase(
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger logger)
        {
            this.validationProvider = validationProvider.ThrowIfNull(nameof(validationProvider));
            this.mapProvider = mapProvider.ThrowIfNull(nameof(mapProvider));
            this.logger = logger.ThrowIfNull(nameof(logger));
        }

        public T MapToValid<T>(object source)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(source, nameof(source));

                var destination = mapProvider.Map<T>(source);
                validationProvider.ThrowIfInvalid(destination);

                return destination;
            }
            catch (Exception ex)
            {
                var state = new Dictionary<string, object?>
                {
                    { nameof(source), source }
                };

                if (ex is ValidationException validationException
                    && validationException.ValidationResult != null)
                {
                    state.Add("VREX", validationException.ValidationResult);
                    state.Add(nameof(validationException.ValidationResult),
                        mapProvider.Map<string>(validationException.ValidationResult));
                }

                using var scope = logger.BeginScope(state);

                logger.LogError("Error mapping {Source} to valid {Destination}", source?.GetType()?.Name, typeof(T)?.Name);
                throw;
            }
        }
    }
}
