using Corely.Common.Providers.Data;

namespace Corely.UnitTests.Common.Providers.Data
{
    public class TextComparisonProviderTests
    {
        [Theory, MemberData(nameof(GetLevenshteinEditDistanceTestData))]
        public void GetLevenshteinEditDistance_ShouldReturnCorrectValue(
            string str1, string str2, int expected)
        {
            var actual = TextComparisonProvider.GetLevenshteinEditDistance(str1, str2);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> GetLevenshteinEditDistanceTestData() =>
        [
            ["kitten", "sitting", 3],
            ["saturday", "sunday", 3],
            ["rosettacode", "raisethysword", 8],
            ["kitten", "kitten", 0],
            ["kitten", "", 6],
            ["", "kitten", 6],
            ["", "", 0]
        ];

        [Theory, MemberData(nameof(GetJaroWinklerDistanceTestData))]
        public void GetJaroWinklerDistance_ShouldReturnCorrectValue(
                       string str1, string str2, double expected)
        {
            var actual = TextComparisonProvider.GetJaroWinklerDistance(str1, str2);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> GetJaroWinklerDistanceTestData() =>
        [
            ["MARTHA", "MARHTA", 0.9611111111111111],
            ["DWAYNE", "DUANE", 0.84000000000000008],
            ["DIXON", "DICKSONX", 0.8133333333333332],
            ["JELLYFISH", "SMELLYFISH", 0.89629629629629637],
            ["JELLYFISH", "JELLYFISH", 1.0],
            ["JELLYFISH", "JELLYFISHY", 0.97999999999999998]
        ];
    }
}
