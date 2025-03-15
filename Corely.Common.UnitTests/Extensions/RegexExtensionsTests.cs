using Corely.Common.Extensions;
using System.Text.RegularExpressions;

namespace Corely.Common.UnitTests.Extensions;

public partial class RegexExtensionsTests
{
    private const string REDACTED = "REDACTED";

    [GeneratedRegex(@"(password)")]
    private static partial Regex PasswordRegex();

    private readonly Regex _passwordRegex = PasswordRegex();

    [GeneratedRegex(@"(group1)|(group2)")]
    private static partial Regex OptionalGroupRegex();

    private readonly Regex _optionalGroupRegex = OptionalGroupRegex();

    [GeneratedRegex(@"(group1) (group2)")]
    private static partial Regex MultiGroupRegex();

    private readonly Regex _multiGroupRegex = MultiGroupRegex();


    [Theory]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData("password", "REDACTED")]
    [InlineData("password123", "REDACTED123")]
    [InlineData("123password", "123REDACTED")]
    [InlineData("123password123", "123REDACTED123")]
    public void ReplaceGroup_ReplacesGroup(string? input, string? expected)
    {
        var actual = _passwordRegex.ReplaceGroups(input!, REDACTED);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("password password", "REDACTED REDACTED")]
    [InlineData("password123 password123", "REDACTED123 REDACTED123")]
    [InlineData("123password 123password", "123REDACTED 123REDACTED")]
    [InlineData("123password123 123password123", "123REDACTED123 123REDACTED123")]
    public void ReplaceGroup_ReplacesMultiple(string input, string expected)
    {
        var actual = _passwordRegex.ReplaceGroups(input, REDACTED);

        Assert.Equal(expected, actual);
    }


    [Theory]
    [InlineData("group1", "REDACTED")]
    [InlineData("group2", "REDACTED")]
    [InlineData("group1 group2", "REDACTED REDACTED")]
    [InlineData("group2 group1", "REDACTED REDACTED")]
    [InlineData("group1group2", "REDACTEDREDACTED")]
    [InlineData("group2group1", "REDACTEDREDACTED")]
    public void ReplaceGroup_OptionalGroupRegex_ReplacesGroup(string input, string expected)
    {
        var actual = _optionalGroupRegex.ReplaceGroups(input, REDACTED);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("group1", "group1")]
    [InlineData("group2", "group2")]
    [InlineData("group1 group2", "REDACTED REDACTED")]
    [InlineData("group1 group2 group1 group2", "REDACTED REDACTED REDACTED REDACTED")]
    [InlineData("group1group2", "group1group2")]
    [InlineData("group2 group1", "group2 group1")]
    public void ReplaceGroup_MultiGroupRegex_ReplacesGroup(string input, string expected)
    {
        var actual = _multiGroupRegex.ReplaceGroups(input, REDACTED);
        Assert.Equal(expected, actual);
    }


    [Fact]
    public void ReplaceGroup_Throws_WhenInputIsNull()
    {
        var input = "Hello, World!";
        var replacement = "redacted";
        var regex = new Regex(@"(Hello), (World)!");
        var result = regex.ReplaceGroups(input, replacement);
        ;
        Assert.Throws<ArgumentNullException>(() => _passwordRegex.ReplaceGroups(null!, REDACTED));
    }
}
