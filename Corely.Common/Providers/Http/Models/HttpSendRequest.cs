﻿using Corely.Common.Extensions;

namespace Corely.Common.Providers.Http.Models
{
    public class HttpSendRequest(
        string requestUri,
        HttpMethod httpMethod)
    {
        private readonly string _requestUri = requestUri.ThrowIfNullOrWhiteSpace(nameof(requestUri));
        private readonly HttpMethod _httpMethod = httpMethod.ThrowIfNull(nameof(httpMethod));

        public IHttpParameters Parameters { get; set; } = null!;
        public Dictionary<string, string> Headers { get; set; } = null!;

        public HttpRequestMessage CreateHttpRequestMessage()
        {
            HttpRequestMessage message = new(_httpMethod, CreateRequestUri());

            if (Headers != null)
            {
                foreach (KeyValuePair<string, string> header in Headers)
                {
                    message.Headers.Add(header.Key, header.Value);
                }
            }

            return message;
        }

        private string CreateRequestUri()
        {
            string? paramsString = Parameters?.CreateParameters();
            if (string.IsNullOrWhiteSpace(paramsString))
            {
                return _requestUri;
            }
            return $"{_requestUri}?{paramsString}";
        }
    }
}
