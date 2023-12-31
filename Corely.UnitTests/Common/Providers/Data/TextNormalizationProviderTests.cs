﻿using Corely.Common.Providers.Data;

namespace Corely.UnitTests.Common.Providers.Data
{
    public class TextNormalizationProviderTests
    {
        private readonly TextNormalizationProvider _textNormalizationProvider = new();

        [Theory, MemberData(nameof(BasicNormalizeTestData))]
        public void BasicNormalize_ShouldReturnBasicNormalizedString(string input, string expected)
        {
            string actual = _textNormalizationProvider.BasicNormalize(input);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> BasicNormalizeTestData()
        {
            yield return new object[] { "This is a test of the emergency broadcast system.", "THISISATESTOFTHEEMERGENCYBROADCASTSYSTEM" };
            yield return new object[] { string.Empty, string.Empty };
            yield return new object[] { "This is a test of the emergency broadcast system. 1234567890 !@#$%^&*()_+", "THISISATESTOFTHEEMERGENCYBROADCASTSYSTEM1234567890_" };
        }

        [Fact]
        public void BasicNormalize_ShouldThrowArgumentNullException()
        {
            var ex = Record.Exception(() => _textNormalizationProvider.BasicNormalize(null!));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory, MemberData(nameof(NormalizeAddressTestData))]
        public void NormalizeAddress_ShouldReturnNormalizedAddress(string street, string[] additional, string expected)
        {
            string actual = _textNormalizationProvider.NormalizeAddress(street, additional);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> NormalizeAddressTestData()
        {
            yield return new object[] { "1234 Main St.", new string[] { "Apt. 1" }, "1234MAINAPT1" };
            yield return new object[] { "1234 Main St.", new string[] { "Apt. 1", "Anytown, CA 12345" }, "1234MAINAPT1ANYTOWNCA12345" };
            yield return new object[] { "Via San Carlo, 156", new string[] { "80132 Napoli", "NA", "Italy" }, "VIASANCARLO15680132NAPOLINAITALY" };
        }

        [Fact]
        public void NormalizeAddress_ShouldThrowArgumentNullException()
        {
            string[] additional = ["Apt. 1"];

            var ex = Record.Exception(() => _textNormalizationProvider.NormalizeAddress(null!, additional));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory, MemberData(nameof(NormalizeAddressAndStateTestData))]
        public void NormalizeAddressAndState_ShouldReturnNormalizedAddressAndState(string street, string[] additional, string expected)
        {
            string actual = _textNormalizationProvider.NormalizeAddressAndState(street, additional);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> NormalizeAddressAndStateTestData()
        {
            yield return new object[] { "1234 Main St.", new string[] { "Apt. 1" }, "1234MAINAPT1" };
            yield return new object[] { "1234 Main St.", new string[] { "Apt. 1", "Anytown, California 12345" }, "1234MAINAPT1ANYTOWNCA12345" };
            yield return new object[] { "4950 Test Boulevard", new string[] { "Apartment 5", "Great Falls", "Montana", "56329" }, "4950TESTAPARTMENT5GREATFALLSMT56329" };
        }

        [Fact]
        public void NormalizeAddressAndState_ShouldThrowArgumentNullException()
        {
            string[] additional = ["Apt. 1"];

            var ex = Record.Exception(() => _textNormalizationProvider.NormalizeAddressAndState(null!, additional));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}
