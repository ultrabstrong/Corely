using Corely.Common.Models;
using Microsoft.Extensions.Logging;

namespace Corely.Common.Extensions
{
    public static class LoggerExtensions
    {
        public static ILogger ForContext(this ILogger logger, params (string Key, object Value)[] properties)
        {
            ArgumentNullException.ThrowIfNull(logger);

            if (properties == null || properties.Length == 0)
                return logger;

            var scopeDictionary = properties.ToDictionary(p => p.Key, p => p.Value);
            var scope = logger.BeginScope(scopeDictionary);

            if (scope != null)
                return new ScopedLogger(logger, scope);

            logger.LogWarning("ILogger.BeginScope returned a null scope. Scoping is not supported by the current logging provider.");
            return logger;
        }

        private sealed class ScopedLogger : DisposeBase, ILogger
        {
            private readonly ILogger _logger;
            private readonly IDisposable _scope;

            public ScopedLogger(ILogger logger, IDisposable scope)
            {
                _logger = logger;
                _scope = scope;
            }

            public IDisposable? BeginScope<TState>(TState state)
                where TState : notnull
                => _logger.BeginScope(state);

            public bool IsEnabled(LogLevel logLevel)
                => _logger.IsEnabled(logLevel);

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
                => _logger.Log(logLevel, eventId, state, exception, formatter);

            protected override void DisposeManagedResources()
            {
                _scope?.Dispose();
            }
        }
    }
}
