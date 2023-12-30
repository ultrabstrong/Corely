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
        {
            return _loggerFactory;
        }

        public ILogger<T> CreateLogger<T>()
        {
            return _loggerFactory.CreateLogger<T>();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
