﻿namespace Corely.Shared.Providers.Http.Models
{
    public class HttpStringContentBase : IHttpContent<string>
    {
        public string Content { get; }

        public HttpStringContentBase(string content)
        {
            Content = content;
        }
    }
}
