using System.Text.RegularExpressions;

namespace Corely.Common.Providers.Redaction
{
    public partial class PasswordRedactionProvider : RedactionProviderBase
    {
        protected override List<Regex> GetReplacePatterns() => [
            JsonPasswordProperty(),
            LogPasswordProperty()
        ];

        [GeneratedRegex(@"""?(?:password|pwd)""?.*?""((?:[^""\\]|\\.)+)""", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        private static partial Regex JsonPasswordProperty();

        [GeneratedRegex(@"(?:password|pwd) = ([^\s]*)", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        private static partial Regex LogPasswordProperty();
    }
}
