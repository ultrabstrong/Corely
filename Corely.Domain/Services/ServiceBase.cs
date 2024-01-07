using Corely.Common.Extensions;
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
            catch
            {
                using var scope = logger.BeginScope(new Dictionary<string, object?>
                {
                    { nameof(source), source }
                });

                logger.LogError("Error mapping {Source} to valid {Destination}", source.GetType().Name, typeof(T).Name);
                throw;
            }
        }
    }
}
