using System.Text;
using System.Text.RegularExpressions;

namespace Corely.Common.Extensions;

public static class RegexExtensions
{
    public static string ReplaceGroup(this Regex regex, string input, int groupIndex, string replacement)
    {
        var sb = new StringBuilder();
        int previousGroupEnd = 0;

        var matches = regex.Matches(input);
        if (matches != null)
        {
            foreach (var match in matches.ToList())
            {
                var group = match.Groups[groupIndex];

                // Append up to group index
                sb.Append(input, previousGroupEnd, group.Index - previousGroupEnd);
                sb.Append(replacement);
                previousGroupEnd = group.Index + group.Length;
            }
        }

        // Append the remainder of the string
        if (previousGroupEnd < input.Length)
        {
            sb.Append(input, previousGroupEnd, input.Length - previousGroupEnd);
        }

        return sb.ToString();
    }
}
