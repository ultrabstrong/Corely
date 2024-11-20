using Corely.Common.Extensions;
using Corely.IAM.Mappers;
using Corely.IAM.Validators;
using Microsoft.Extensions.Logging;

namespace Corely.IAM.Processors
{
    internal abstract class ProcessorBase
    {
        private readonly IValidationProvider _validationProvider;
        private readonly IMapProvider _mapProvider;

        protected readonly ILogger Logger;

        public ProcessorBase(
            IMapProvider mapProvider,
            IValidationProvider validationProvider,
            ILogger logger)
        {
            _validationProvider = validationProvider.ThrowIfNull(nameof(validationProvider));
            _mapProvider = mapProvider.ThrowIfNull(nameof(mapProvider));
            Logger = logger.ThrowIfNull(nameof(logger));
        }

        protected void LogRequest<T>(string className, string methodName, T request)
        {
            Logger.LogDebug("[{Class}] {Method} starting with request {@Request}", className, methodName, request);
        }

        protected T LogResult<T>(string className, string methodName, T result)
        {
            Logger.LogDebug("[{Class}] {Method} completed with result {@Result}", className, methodName, result);
            return result;
        }

        protected void LogResult(string className, string methodName)
        {
            Logger.LogDebug("[{Class}] {Method} completed", className, methodName);
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
                    { "@MapSource", source }
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
                    state.Add("@ValidationResult", validationException.ValidationResult);
                }

                using var scope = Logger.BeginScope(state);
                Logger.LogWarning(ex, "Validation failed for {ModelType}", model?.GetType()?.Name);
                throw;
            }
        }
    }
}
