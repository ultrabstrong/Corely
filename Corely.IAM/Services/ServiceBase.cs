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
        private readonly IValidationProvider _validationProvider;
        private readonly IMapProvider _mapProvider;

        protected readonly ILogger Logger;

        public ServiceBase(
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger logger)
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            _validationProvider = validationProvider.ThrowIfNull(nameof(validationProvider));
            _mapProvider = mapProvider.ThrowIfNull(nameof(mapProvider));
            Logger = logger.ThrowIfNull(nameof(logger));
        }

        public T MapAndValidate<T>(object source)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(source, nameof(source));

                var destination = _mapProvider.Map<T>(source);
                _validationProvider.ThrowIfInvalid(destination);

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

                using var scope = Logger.BeginScope(state);

                Logger.LogWarning("Failed to map {MapSourceType} to valid {MapDestinationType}", source?.GetType()?.Name, typeof(T)?.Name);
                throw;
            }
        }

        public T Map<T>(object source)
        {
            return _mapProvider.Map<T>(source);
        }

        public T? MapOrNull<T>(object? source)
        {
            return source == null ? default : _mapProvider.Map<T>(source);
        }
    }
}
