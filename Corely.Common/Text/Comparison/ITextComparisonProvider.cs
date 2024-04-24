namespace Corely.Common.Text.Comparison
{
    public interface ITextComparisonProvider
    {
        int GetLevenshteinEditDistance(string str1, string str2);

        double GetJaroWinklerDistance(
            string str1,
            string str2,
            double weightThreshold = 0.7,
            int numCharPrefix = 4);
    }
}
