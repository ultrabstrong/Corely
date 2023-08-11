using Corely.Shared.Providers.Data;

namespace Corely.UnitTests.Shared.Providers.Data
{
    public class TextComparisonProviderTests
    {
        private readonly TextComparisonProvider _textComparisonProvider;

        public TextComparisonProviderTests()
        {
            _textComparisonProvider = new TextComparisonProvider();
        }

        [Fact]
        public void Tests_ShouldBeImplemented_AtSomePointInTheFuture()
        {
            // use it to suppress IDE0052 suggestion
            _textComparisonProvider.GetType();
            Assert.True(true);
        }
    }
}
