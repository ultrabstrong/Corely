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

        public T MapThenValidateTo<T>(object source)
        {
            var mapped = MapTo<T>(source);
            Validate(mapped);
            return mapped;
        }

        public T MapTo<T>(object source)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(source, nameof(source));
                return _mapProvider.MapTo<T>(source); ;
            }
            catch (Exception ex)
            {
                using var scope = Logger.BeginScope(new Dictionary<string, object?>
                {
                    { "MapSource", JsonSerializer.Serialize(source, _jsonSerializerOptions) }
                });

                Logger.LogWarning(ex, "Failed to map {MapSourceType} to {MapDestinationType}", source?.GetType()?.Name, typeof(T)?.Name);
                throw;
            }
        }

        public void Validate<T>(T model)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(model, nameof(model));
                _validationProvider.ThrowIfInvalid(model);
            }
            catch (Exception ex)
            {
                var state = new Dictionary<string, object?>();

                if (ex is ValidationException validationException
                    && validationException.ValidationResult != null)
                {
                    state.Add("ValidationResult",
                        JsonSerializer.Serialize(validationException.ValidationResult, _jsonSerializerOptions));
                }

                Logger.LogWarning(ex, "Validation failed for {ModelType}", model?.GetType()?.Name);
                throw;
            }
        }
    }
}
