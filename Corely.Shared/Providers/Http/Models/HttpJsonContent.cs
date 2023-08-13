﻿using System.Text.Json;

namespace Corely.Shared.Providers.Http.Models
{
    public sealed class HttpJsonContent : HttpStringContentBase
    {
        public HttpJsonContent(object content)
            : base(JsonSerializer.Serialize(content))
        {

        }

        public HttpJsonContent(string content)
            : base(content)
        {

        }
    }
}
