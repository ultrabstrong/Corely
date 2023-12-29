using Microsoft.Extensions.Logging;
using Serilog;

namespace Corely.UnitTests.Fixtures
{
    public class LoggerFixture : IDisposable
    {
        private readonly ILoggerFactory _loggerFactory;

        public LoggerFixture()
        {
            var logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            _loggerFactory = LoggerFactory.Create(builder => builder.AddSerilog(logger));
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
