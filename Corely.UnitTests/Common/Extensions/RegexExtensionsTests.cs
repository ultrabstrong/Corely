using Corely.Common.Extensions;
using System.Text.RegularExpressions;

namespace Corely.UnitTests.Common.Extensions
{
    public partial class RegexExtensionsTests
    {
        private const string REDACTED = "REDACTED";

        [GeneratedRegex(@"(password)")]
        private static partial Regex PasswordRegex();

        private readonly Regex _passwordRegex = PasswordRegex();

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("password", "REDACTED")]
        [InlineData("password123", "REDACTED123")]
        [InlineData("123password", "123REDACTED")]
        public void ReplaceGroup_ReplacesGroup(string? input, string? expected)
        {
            var actual = _passwordRegex.ReplaceGroup(input!, 1, REDACTED);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("password password", "REDACTED REDACTED")]
        [InlineData("password123 password123", "REDACTED123 REDACTED123")]
        [InlineData("123password 123password", "123REDACTED 123REDACTED")]
        public void ReplaceGroup_ReplacesMultiple(string input, string expected)
        {
            var actual = _passwordRegex.ReplaceGroup(input, 1, REDACTED);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReplaceGroup_Throws_WhenInputIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _passwordRegex.ReplaceGroup(null!, 1, REDACTED));
        }
    }
}
