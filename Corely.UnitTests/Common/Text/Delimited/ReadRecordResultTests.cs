using Corely.Common.Text.Delimited;

namespace Corely.UnitTests.Common.Text.Delimited;

public class ReadRecordResultTests
{
    private readonly ReadRecordResult _readRecordResult = new();

    [Fact]
    public void ToString_ReturnsCommaDelimitedTokens()
    {
        _readRecordResult.Tokens = ["a", "b", "c"];
        Assert.Equal("a,b,c", _readRecordResult.ToString());
    }
}
