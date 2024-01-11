using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Corely.UnitTests.Fixtures
{
    public class LoggerFixture : IDisposable
    {
        private readonly ILoggerFactory _loggerFactory;

        public LoggerFixture()
        {
            _loggerFactory = NullLoggerFactory.Instance;
        }

        public ILoggerFactory GetLoggerFactory()
            => _loggerFactory;

        public ILogger<T> CreateLogger<T>()
            => _loggerFactory.CreateLogger<T>();

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
