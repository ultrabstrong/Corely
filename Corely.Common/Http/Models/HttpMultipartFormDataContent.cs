﻿namespace Corely.Common.Http.Models;

public sealed class HttpMultipartFormDataContent : HttpDictionaryContentBase
{
    public HttpMultipartFormDataContent(Dictionary<string, string> content)
        : base(content) { }
}
