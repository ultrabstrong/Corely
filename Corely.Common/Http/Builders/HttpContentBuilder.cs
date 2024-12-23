﻿using Corely.Common.Http.Models;

namespace Corely.Common.Http.Builders;

public class HttpContentBuilder : IHttpContentBuilder
{
    public HttpContent Build<T>(IHttpContent<T> content)
    {
        ArgumentNullException.ThrowIfNull(content, nameof(content));
        return content switch
        {
            HttpMultipartFormDataContent c => BuildMultipartFormDataContent(c),
            HttpFormUrlEncodedContent c => BuildFormUrlEncodedContent(c),
            HttpJsonContent c => BuildJsonContent(c),
            HttpTextContent c => BuildTextContent(c),
            _ => throw new NotImplementedException()
        };
    }

    private static MultipartFormDataContent BuildMultipartFormDataContent(HttpDictionaryContentBase content)
    {
        MultipartFormDataContent result = [];

        foreach (KeyValuePair<string, string> formvals in content.Content)
        {
            result.Add(new StringContent(formvals.Value), formvals.Key);
        }

        return result;
    }

    private static FormUrlEncodedContent BuildFormUrlEncodedContent(HttpDictionaryContentBase content)
    {
        return new FormUrlEncodedContent(content.Content);
    }

    private static StringContent BuildJsonContent(HttpStringContentBase content)
    {
        return new StringContent(content.Content, mediaType: new("application/json"));
    }

    private static StringContent BuildTextContent(HttpStringContentBase content)
    {
        return new StringContent(content.Content, mediaType: new("text/plain"));
    }
}
