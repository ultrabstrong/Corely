using Corely.Common.Redaction;
using System.Text.RegularExpressions;

namespace Corely.UnitTests.Common.Providers.Redaction
{
    public partial class RedactionProviderBaseTests
    {
        private partial class TestRedactionProvider : RedactionProviderBase
        {
            protected override List<Regex> GetReplacePatterns() => [
                PasswordRegex(),
                DigitRegex()
            ];

            [GeneratedRegex(@"(password)")]
            private static partial Regex PasswordRegex();

            [GeneratedRegex(@"(\d+)")]
            private static partial Regex DigitRegex();
        }

        private readonly TestRedactionProvider _redactionProvider = new();

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("password", "REDACTED")]
        [InlineData("password123", "REDACTEDREDACTED")]
        [InlineData("123password", "REDACTEDREDACTED")]
        public void Redact_Redacts_WithMultipleRegexes(string? input, string? expected)
        {
            var actual = _redactionProvider.Redact(input!);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("password password", "REDACTED REDACTED")]
        [InlineData("password123 password123", "REDACTEDREDACTED REDACTEDREDACTED")]
        [InlineData("123password 123password", "REDACTEDREDACTED REDACTEDREDACTED")]
        public void Redact_RedactsMultiple_WithMultipleRegexes(string? input, string? expected)
        {
            var actual = _redactionProvider.Redact(input!);

            Assert.Equal(expected, actual);
        }
    }
}
