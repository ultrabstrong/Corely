using Corely.Common.Providers.Redaction;

namespace Corely.UnitTests.Common.Providers.Redaction
{
    public class PasswordRedactionProviderTests
    {
        private readonly PasswordRedactionProvider _passwordRedactionProvider = new();

        [Theory]
        [MemberData(nameof(RedactTestData))]
        public void Redact_ShouldRedactPassword(string input, string expected)
        {
            var actual = _passwordRedactionProvider.Redact(input);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> RedactTestData() =>
        [
            [
                @"{""UserId"":1,""Username"":""username"",""Password"":""as@#$%#$^   09u09a8s09fj;qo34\""808+_)(*&^%$@!$#@^""",
                @"{""UserId"":1,""Username"":""username"",""Password"":""REDACTED"""
            ],
            [
                @"{""UserId"":1,""Username"":""username"",""Pwd"":""as@#$%#$^   09u09a8s09fj;qo34\""808+_)(*&^%$@!$#@^""",
                @"{""UserId"":1,""Username"":""username"",""Pwd"":""REDACTED"""
            ]
        ];
    }
}
