using Corely.Common.Providers.Data;

namespace Corely.Miscellaneous.UnitTests.Text.Normalization;

public class TextNormalizationProviderTests
{
    private readonly TextNormalizationProvider _textNormalizationProvider = new();

    [Theory, MemberData(nameof(BasicNormalizeTestData))]
    public void BasicNormalize_ReturnsBasicNormalizedString(string input, string expected)
    {
        string actual = _textNormalizationProvider.BasicNormalize(input);
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> BasicNormalizeTestData() =>
    [
        ["This is a test of the emergency broadcast system.", "THISISATESTOFTHEEMERGENCYBROADCASTSYSTEM"],
            [string.Empty, string.Empty],
            ["This is a test of the emergency broadcast system. 1234567890 !@#$%^&*()_+", "THISISATESTOFTHEEMERGENCYBROADCASTSYSTEM1234567890_"]
    ];

    [Fact]
    public void BasicNormalize_Throws()
    {
        var ex = Record.Exception(() => _textNormalizationProvider.BasicNormalize(null!));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Theory, MemberData(nameof(NormalizeAddressTestData))]
    public void NormalizeAddress_ReturnsNormalizedAddress(string street, string[] additional, string expected)
    {
        string actual = _textNormalizationProvider.NormalizeAddress(street, additional);
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> NormalizeAddressTestData() =>
    [
        ["1234 Main St.", new[] { "Apt. 1" }, "1234MAINAPT1"],
            ["1234 Main St.", new[] { "Apt. 1", "Anytown, CA 12345" }, "1234MAINAPT1ANYTOWNCA12345"],
            ["Via San Carlo, 156", new[] { "80132 Napoli", "NA", "Italy" }, "VIASANCARLO15680132NAPOLINAITALY"]
    ];

    [Fact]
    public void NormalizeAddress_Throws()
    {
        string[] additional = ["Apt. 1"];

        var ex = Record.Exception(() => _textNormalizationProvider.NormalizeAddress(null!, additional));

        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Theory, MemberData(nameof(NormalizeAddressAndStateTestData))]
    public void NormalizeAddressAndState_ReturnsNormalizedAddressAndState(string street, string[] additional, string expected)
    {
        string actual = _textNormalizationProvider.NormalizeAddressAndState(street, additional);
        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> NormalizeAddressAndStateTestData() =>
    [
        ["1234 Main St.", new[] { "Apt. 1" }, "1234MAINAPT1"],
            ["1234 Main St.", new[] { "Apt. 1", "Anytown, California 12345" }, "1234MAINAPT1ANYTOWNCA12345"],
            ["4950 Test Boulevard", new[] { "Apartment 5", "Great Falls", "Montana", "56329" }, "4950TESTAPARTMENT5GREATFALLSMT56329"]
    ];

    [Fact]
    public void NormalizeAddressAndState_Throws()
    {
        string[] additional = ["Apt. 1"];

        var ex = Record.Exception(() => _textNormalizationProvider.NormalizeAddressAndState(null!, additional));

        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }
}
