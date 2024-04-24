using Corely.Common.Extensions;
using System.Text.RegularExpressions;

namespace Corely.Common.Redaction
{
    public abstract class RedactionProviderBase : IRedactionProvider
    {
        private const string REDACTED = "REDACTED";
        private readonly List<Regex> _regexPatterns;

        public RedactionProviderBase()
        {
            _regexPatterns = GetReplacePatterns();
        }

        protected abstract List<Regex> GetReplacePatterns();

        public string? Redact(string? input)
        {
            if (input == null) { return input; }
            string output = input;
            foreach (var regex in _regexPatterns)
            {
                output = regex.ReplaceGroup(output, 1, REDACTED);
            }
            return output;
        }
    }
}
