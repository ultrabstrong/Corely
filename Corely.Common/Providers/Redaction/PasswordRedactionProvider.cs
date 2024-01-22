using System.Text.RegularExpressions;

namespace Corely.Common.Providers.Redaction
{
    public partial class PasswordRedactionProvider : RedactionProviderBase
    {
        protected override List<Regex> GetReplacePatterns()
        {
            return [
                JsonPasswordProperty(),
                JsonPwdProperty()
            ];
        }

        [GeneratedRegex(@"\""password\"".*?\""((?:[^\""\\]|\\.)+)\""", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        private static partial Regex JsonPasswordProperty();


        [GeneratedRegex(@"\""pwd\"".*?\""((?:[^\""\\]|\\.)+)\""", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        private static partial Regex JsonPwdProperty();
    }
}
