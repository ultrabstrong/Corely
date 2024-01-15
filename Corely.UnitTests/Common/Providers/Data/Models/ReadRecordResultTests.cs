using Corely.Common.Providers.Data.Models;

namespace Corely.UnitTests.Common.Providers.Data.Models
{
    public class ReadRecordResultTests
    {
        private readonly ReadRecordResult _readRecordResult = new();

        [Fact]
        public void ToString_ShouldReturnCommaDelimitedTokens()
        {
            _readRecordResult.Tokens = new List<string> { "a", "b", "c" };
            Assert.Equal("a,b,c", _readRecordResult.ToString());
        }
    }
}
