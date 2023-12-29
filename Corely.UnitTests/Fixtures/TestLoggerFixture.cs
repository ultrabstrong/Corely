using Serilog;

namespace Corely.UnitTests.Fixtures
{
    public class TestLoggerFixture : IDisposable
    {
        public ILogger Logger { get; }

        public TestLoggerFixture()
        {
            Logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .CreateLogger();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
