using System.Text;
using System.Text.RegularExpressions;

namespace Corely.Common.Extensions;

public static class RegexExtensions
{
    public static string ReplaceGroups(this Regex regex, string input, string replacement)
    {
        ArgumentNullException.ThrowIfNull(input);

        var sb = new StringBuilder(input.Length);
        var previousGroupEnd = 0;
        var inputSpan = input.AsSpan();

        foreach (Match match in regex.Matches(input))
        {
            foreach (Group group in match.Groups.Cast<Group>().Skip(1)) // Skip the 0th group as it is the entire match
            {
                if (group.Success)
                {
                    // Append up to group index
                    sb.Append(inputSpan[previousGroupEnd..group.Index]);
                    sb.Append(replacement);
                    previousGroupEnd = group.Index + group.Length;
                }
            }
        }

        // Append the remainder of the string
        if (previousGroupEnd < input.Length)
        {
            sb.Append(inputSpan[previousGroupEnd..]);
        }

        return sb.ToString();
    }
}
