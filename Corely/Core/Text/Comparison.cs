namespace Corely.Core.Text
{
    public static class Comparison
    {
        public static int GetLevenshteinEditDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s)) { throw new ArgumentNullException(s); }
            if (string.IsNullOrEmpty(t)) { throw new ArgumentNullException(t); }

            // Check for empty input
            if (s.Length == 0) { return t.Length; }
            if (t.Length == 0) { return s.Length; }

            int[] p = new int[s.Length + 1]; //'previous' cost array, horizontally
            int[] d = new int[s.Length + 1]; // cost array, horizontally

            int i, j;

            for (i = 0; i <= s.Length; i++)
            {
                p[i] = i;
            }

            for (j = 1; j <= t.Length; j++)
            {
                char tJ = t[j - 1]; // jth character of t
                d[0] = j;

                for (i = 1; i <= s.Length; i++)
                {
                    // cost
                    int cost = s[i - 1] == tJ ? 0 : 1;
                    // minimum of cell to the left+1, to the top+1, diagonally left and up +cost                
                    d[i] = Math.Min(Math.Min(d[i - 1] + 1, p[i] + 1), p[i - 1] + cost);
                }

                // copy current distance counts to 'previous row' distance counts
                int[] dPlaceholder = p; //placeholder to assist in swapping p and d
                p = d;
                d = dPlaceholder;
            }

            // our last action in the above loop was to switch d and p, so p now 
            // actually has the most recent cost counts
            return p[s.Length];
        }

        /// <summary>
        /// Returns the Jaro-Winkler distance between the specified  
        /// strings. The distance is symmetric and will fall in the 
        /// range 0 (no match) to 1 (perfect match). 
        /// </summary>
        /// <param name="aString1">First String</param>
        /// <param name="aString2">Second String</param>
        /// <param name="weightThreshold"><para>
        /// The Winkler modification will not be applied unless the 
        /// percent match was at or above the mWeightThreshold percent
        /// without the modification.
        /// Winkler's paper used a default value of 0.7</para></param>
        /// <param name="numCharPrefix"><para>
        /// Size of the prefix to be concidered by the Winkler modification. 
        /// Winkler's paper used a default value of 4
        /// </para></param>
        /// <returns></returns>
        public static double GetJaroWinklerDistance(string aString1, string aString2, double weightThreshold = 0.7, int numCharPrefix = 4)
        {
            IEqualityComparer<char> comparer = EqualityComparer<char>.Default;

            var lLen1 = aString1.Length;
            var lLen2 = aString2.Length;
            if (lLen1 == 0) { return lLen2 == 0 ? 1.0 : 0.0; }


            var lSearchRange = Math.Max(0, Math.Max(lLen1, lLen2) / 2 - 1);

            var lMatched1 = new bool[lLen1];
            var lMatched2 = new bool[lLen2];

            var lNumCommon = 0;
            for (var i = 0; i < lLen1; ++i)
            {
                var lStart = Math.Max(0, i - lSearchRange);
                var lEnd = Math.Min(i + lSearchRange + 1, lLen2);
                for (var j = lStart; j < lEnd; ++j)
                {
                    if (lMatched2[j]) continue;
                    if (!comparer.Equals(aString1[i], aString2[j]))
                        continue;
                    lMatched1[i] = true;
                    lMatched2[j] = true;
                    ++lNumCommon;
                    break;
                }
            }

            if (lNumCommon == 0) return 0.0;

            var lNumHalfTransposed = 0;
            var k = 0;
            for (var i = 0; i < lLen1; ++i)
            {
                if (!lMatched1[i]) continue;
                while (!lMatched2[k]) ++k;
                if (!comparer.Equals(aString1[i], aString2[k]))
                    ++lNumHalfTransposed;
                ++k;
            }
            var lNumTransposed = lNumHalfTransposed / 2;

            double lNumCommonD = lNumCommon;
            var lWeight = (lNumCommonD / lLen1
                              + lNumCommonD / lLen2
                              + (lNumCommon - lNumTransposed) / lNumCommonD) / 3.0;

            if (lWeight <= weightThreshold) return lWeight;
            var lMax = Math.Min(numCharPrefix, Math.Min(aString1.Length, aString2.Length));
            var lPos = 0;
            while (lPos < lMax && comparer.Equals(aString1[lPos], aString2[lPos]))
                ++lPos;
            if (lPos == 0) return lWeight;
            return lWeight + 0.1 * lPos * (1.0 - lWeight);

        }
    }
}
