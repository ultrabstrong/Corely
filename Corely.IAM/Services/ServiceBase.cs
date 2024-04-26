using Corely.Common.Extensions;
using Corely.IAM.Mappers;
using Corely.IAM.Validators;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Corely.IAM.Services
{
    public abstract class ServiceBase
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        protected readonly IValidationProvider validationProvider;
        protected readonly IMapProvider mapProvider;
        protected readonly ILogger logger;

        public ServiceBase(
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger logger)
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

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
                    { "MapSource", JsonSerializer.Serialize(source, _jsonSerializerOptions) }
                };

                if (ex is ValidationException validationException
                    && validationException.ValidationResult != null)
                {
                    state.Add("ValidationResult",
                        JsonSerializer.Serialize(validationException.ValidationResult, _jsonSerializerOptions));
                }

                using var scope = logger.BeginScope(state);

                logger.LogWarning("Failed to map {MapSourceType} to valid {MapDestinationType}", source?.GetType()?.Name, typeof(T)?.Name);
                throw;
            }
        }
    }
}
