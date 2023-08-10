using Corely.Shared.Providers.Data;

namespace Corely.UnitTests.Shared.Providers.Data
{
    public class DelimitedTextDataProviderTests
    {
        private readonly DelimitedTextDataProvider _provider;

        public DelimitedTextDataProviderTests()
        {
            _provider = new DelimitedTextDataProvider();
        }

        [Fact]
        public void Tests_ShouldBeImplemented_AtSomePointInTheFuture()
        {
            Assert.True(true);
        }
    }
}
