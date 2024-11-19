using Corely.Common.Extensions;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.Common.Extensions
{
    public class LoggerExtentionsTests
    {
        [Fact]
        public void ForContext_Throws_WhenLoggerIsNull()
        {
            ILogger logger = null!;

            var ex = Record.Exception(() => logger.ForContext(("key", "value")));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void ForContext_ReturnsLogger_WhenPropertiesIsNull()
        {
            var logger = Mock.Of<ILogger>();
            var result = logger.ForContext(null);
            Assert.Same(logger, result);
        }

        [Fact]
        public void ForContext_ReturnsLogger_WhenPropertiesIsEmpty()
        {
            var logger = Mock.Of<ILogger>();
            var result = logger.ForContext();
            Assert.Same(logger, result);
        }

        [Fact]
        public void ForContext_ReturnsScopedLogger_WhenScopeNotEmpty()
        {
            var loggerMock = new Mock<ILogger>();

            loggerMock.Setup(l => l
                .BeginScope(It.IsAny<IDictionary<string, object>>()))
                .Returns(Mock.Of<IDisposable>());

            var result = loggerMock.Object.ForContext(("key", "value"));

            loggerMock.Verify(l => l
                .BeginScope(It.Is<IDictionary<string, object>>(d => HasValidContext(d, "key", "value"))),
                Times.Once);
        }

        private static bool HasValidContext(IDictionary<string, object> d, string expectedKey, string expectedValue)
        {
            Assert.True(d.TryGetValue(expectedKey, out var v));
            var value = Assert.IsType<string>(v);
            Assert.Equal(expectedValue, value);
            return true;
        }

        [Fact]
        public void ForContext_ReturnsLogger_WhenScopeIsNull()
        {
            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(l => l
                .BeginScope(It.IsAny<IDictionary<string, object>>()))
                .Returns<IDisposable>(null);

            var result = loggerMock.Object.ForContext(("key", "value"));

            Assert.Same(loggerMock.Object, result);
        }
    }
}
