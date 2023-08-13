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

        [Theory, MemberData(nameof(GetLevenshteinEditDistanceTestData))]
        public void GetLevenshteinEditDistance_ShouldReturnCorrectValue(
            string str1, string str2, int expected)
        {
            var actual = _textComparisonProvider.GetLevenshteinEditDistance(str1, str2);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> GetLevenshteinEditDistanceTestData()
        {
            yield return new object[] { "kitten", "sitting", 3 };
            yield return new object[] { "saturday", "sunday", 3 };
            yield return new object[] { "rosettacode", "raisethysword", 8 };
            yield return new object[] { "kitten", "kitten", 0 };
            yield return new object[] { "kitten", "", 6 };
            yield return new object[] { "", "kitten", 6 };
            yield return new object[] { "", "", 0 };
        }

        [Theory, MemberData(nameof(GetJaroWinklerDistanceTestData))]
        public void GetJaroWinklerDistance_ShouldReturnCorrectValue(
                       string str1, string str2, double expected)
        {
            var actual = _textComparisonProvider.GetJaroWinklerDistance(str1, str2);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> GetJaroWinklerDistanceTestData()
        {
            yield return new object[] { "MARTHA", "MARHTA", 0.9611111111111111 };
            yield return new object[] { "DWAYNE", "DUANE", 0.84000000000000008 };
            yield return new object[] { "DIXON", "DICKSONX", 0.8133333333333332 };
            yield return new object[] { "JELLYFISH", "SMELLYFISH", 0.89629629629629637 };
            yield return new object[] { "JELLYFISH", "JELLYFISH", 1.0 };
            yield return new object[] { "JELLYFISH", "JELLYFISHY", 0.97999999999999998 };
        }
    }
}
