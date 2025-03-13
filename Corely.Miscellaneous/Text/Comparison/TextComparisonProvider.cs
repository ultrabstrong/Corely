namespace Corely.Common.Text.Comparison;

public class TextComparisonProvider
{
    public static int GetLevenshteinEditDistance(string str1, string str2)
    {
        ArgumentNullException.ThrowIfNull(str1, nameof(str1));
        ArgumentNullException.ThrowIfNull(str2, nameof(str2));

        if (str1.Length == 0) { return str2.Length; }
        if (str2.Length == 0) { return str1.Length; }

        int[] prevCost = new int[str1.Length + 1]; //'previous' cost array, horizontally
        int[] curCost = new int[str1.Length + 1]; // cost array, horizontally

        int i, j;

        for (i = 0; i <= str1.Length; i++)
        {
            prevCost[i] = i;
        }

        for (j = 1; j <= str2.Length; j++)
        {
            curCost[0] = j;

            for (i = 1; i <= str1.Length; i++)
            {
                int cost = str1[i - 1] == str2[j - 1] ? 0 : 1;
                // minimum of cell to the left+1, to the top+1, diagonally left and up +cost                
                curCost[i] = Math.Min(
                    Math.Min(
                        curCost[i - 1] + 1,
                        prevCost[i] + 1),
                    prevCost[i - 1] + cost);
            }

            // copy current distance counts to 'previous row' distance counts
            (curCost, prevCost) = (prevCost, curCost);
        }

        // our last action in the above loop was to switch curCost and prevCost,
        // so prevCost now actually has the most recent cost counts
        return prevCost[str1.Length];
    }

    public static double GetJaroWinklerDistance(
        string str1,
        string str2,
        double weightThreshold = 0.7,
        int numCharPrefix = 4)
    {
        var len1 = str1.Length;
        var len2 = str2.Length;
        if (len1 == 0) { return len2 == 0 ? 1.0 : 0.0; }

        var searchRange = Math.Max(0, Math.Max(len1, len2) / 2 - 1);

        var matched1 = new bool[len1];
        var matched2 = new bool[len2];

        var numCommon = 0;
        for (var i = 0; i < len1; ++i)
        {
            var lStart = Math.Max(0, i - searchRange);
            var lEnd = Math.Min(i + searchRange + 1, len2);
            for (var j = lStart; j < lEnd; ++j)
            {
                if (matched2[j] || str1[i] != str2[j])
                {
                    continue;
                }
                matched1[i] = true;
                matched2[j] = true;
                ++numCommon;
                break;
            }
        }

        if (numCommon == 0) return 0.0;

        var numHalfTransposed = 0;
        var k = 0;
        for (var i = 0; i < len1; ++i)
        {
            if (!matched1[i])
            {
                continue;
            }

            while (!matched2[k])
            {
                ++k;
            }

            if (str1[i] != str2[k])
            {
                ++numHalfTransposed;
            }
            ++k;
        }
        var numTransposed = numHalfTransposed / 2;

        double numCommonD = numCommon;
        var weight = (numCommonD / len1
            + numCommonD / len2
            + (numCommon - numTransposed) / numCommonD) / 3.0;

        if (weight <= weightThreshold) return weight;
        var max = Math.Min(numCharPrefix, Math.Min(str1.Length, str2.Length));
        var pos = 0;
        while (pos < max && str1[pos] == str2[pos])
        {
            ++pos;
        }

        if (pos == 0)
        {
            return weight;
        }
        return weight + 0.1 * pos * (1.0 - weight);

    }
}
