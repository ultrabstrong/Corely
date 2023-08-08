using Corely.Core.Encoding;
using System.Collections.Immutable;

namespace Corely.Proxies
{
    public class HttpParameters
    {
        public HttpParameters()
        {
        }

        public HttpParameters(string key, string value)
        {
            Parameters = new Dictionary<string, string>() { { key, value } };
        }

        public HttpParameters(Dictionary<string, string> parameters)
        {
            Parameters = parameters;
        }

        public HttpParameters(Dictionary<string, string> parameters, Dictionary<string, string> tempParameters)
        {
            Parameters = parameters;
            TempParameters = tempParameters;
        }

        public Dictionary<string, string> Parameters { get; set; } = new();
        public Dictionary<string, string> TempParameters { get; set; } = new();

        public bool HasParameters() => Parameters?.Count > 0;

        public bool HasTempParameters() => TempParameters?.Count > 0;

        public string GetParamString()
        {
            List<string> paramsToInclude = new List<string>();

            Parameters
                ?.Where(IsValid)
                ?.Select(CreateParam)
                ?.ToImmutableList()
                ?.ForEach(paramsToInclude.Add);

            TempParameters
                ?.Where(IsValid)
                ?.Select(CreateParam)
                ?.ToImmutableList()
                ?.ForEach(paramsToInclude.Add);

            TempParameters = null;

            return string.Join('&', paramsToInclude);
        }

        private bool IsValid(KeyValuePair<string, string> pair)
        {
            return !string.IsNullOrWhiteSpace(pair.Key) && !string.IsNullOrWhiteSpace(pair.Value);
        }

        private string CreateParam(KeyValuePair<string, string> pair)
        {
            return $"{pair.Key}={pair.Value.UrlEncode()}";
        }
    }
}
