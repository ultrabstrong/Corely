using System.Net.Http.Headers;

namespace Corely.Shared.Proxies
{
    public class HttpProxy : IDisposable
    {
        public HttpProxy(string host)
        {
            Connect(host);
        }

        private HttpClient _httpClient { get; set; }

        public bool IsConnected { get; internal set; } = false;

        public virtual void Connect(string host)
        {
            if (!IsConnected)
            {
                try
                {
                    _httpClient = new HttpClient() { BaseAddress = new Uri(host) };

                    IsConnected = true;
                }
                catch
                {
                    IsConnected = false;
                    throw;
                }
            }
        }

        public virtual void Disconnect()
        {
            if (IsConnected)
            {
                IsConnected = false;
                _httpClient?.Dispose();
                _httpClient = null;
            }
        }

        public virtual HttpResponseMessage SendRequestForHttpResponseNoErrors(string requestUri, HttpMethod method, HttpContent content, Dictionary<string, string> headers, HttpParameters parameters)
        {
            if (!IsConnected)
            {
                throw new Exception("proxy is not connected");
            }

            if (parameters != null && (parameters.HasParameters() || parameters.HasTempParameters()))
            {
                requestUri += $"?{parameters.GetParamString()}";
            }

            HttpRequestMessage message = new(method, requestUri);

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    message.Headers.Add(header.Key, header.Value);
                }
            }

            if (content != null)
            {
                message.Content = content;
            }

            HttpResponseMessage result = _httpClient.SendAsync(message).GetAwaiter().GetResult();
            return result;
        }

        public virtual HttpResponseMessage SendRequestForHttpResponse(string requestUri, HttpMethod method, HttpContent content, Dictionary<string, string> headers, HttpParameters parameters)
        {
            HttpResponseMessage result = SendRequestForHttpResponseNoErrors(requestUri, method, content, headers, parameters);

            if (result.IsSuccessStatusCode)
            {
                return result;
            }
            else
            {
                string body = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                throw new HttpRequestException($"{(int)result.StatusCode} {result.StatusCode} - {result.ReasonPhrase}{Environment.NewLine}{result.RequestMessage}", new Exception(body));
            }
        }

        public virtual string SendRequestForStringResult(string requestUri, HttpMethod method, HttpContent content, Dictionary<string, string> headers = null, HttpParameters parameters = null)
        {
            HttpResponseMessage result = SendRequestForHttpResponse(requestUri, method, content, headers, parameters);
            return result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        public virtual string SendFormRequestForStringResult(string requestUri, HttpMethod method, Dictionary<string, string> formdata, Dictionary<string, string> headers = null, HttpParameters parameters = null)
        {
            HttpContent content = BuildFormRequestContent(formdata);
            return SendRequestForStringResult(requestUri, method, content, headers, parameters);
        }

        public virtual string SendUrlencodedRequestForStringResult(string requestUri, HttpMethod method, Dictionary<string, string> formdata, Dictionary<string, string> headers = null, HttpParameters parameters = null)
        {
            HttpContent content = BuildUrlencodedContent(formdata);
            return SendRequestForStringResult(requestUri, method, content, headers, parameters);
        }

        public virtual string SendJsonRequestForStringResult(string requestUri, HttpMethod method, string jsonContent, Dictionary<string, string> headers = null, HttpParameters parameters = null)
        {
            HttpContent content = BuildJsonContent(jsonContent);
            return SendRequestForStringResult(requestUri, method, content, headers, parameters);
        }

        public virtual void SendRequest(string requestUri, HttpMethod method, HttpContent content, Dictionary<string, string> headers = null, HttpParameters parameters = null)
        {
            SendRequestForHttpResponse(requestUri, method, content, headers, parameters);
        }

        public virtual void SendFormRequest(string requestUri, HttpMethod method, Dictionary<string, string> formdata, Dictionary<string, string> headers = null, HttpParameters parameters = null)
        {
            HttpContent content = BuildFormRequestContent(formdata);
            SendRequest(requestUri, method, content, headers, parameters);
        }

        public virtual void SendUrlencodedRequest(string requestUri, HttpMethod method, Dictionary<string, string> formdata, Dictionary<string, string> headers = null, HttpParameters parameters = null)
        {
            HttpContent content = BuildUrlencodedContent(formdata);
            SendRequest(requestUri, method, content, headers, parameters);
        }

        public virtual void SendJsonRequest(string requestUri, HttpMethod method, string jsonContent, Dictionary<string, string> headers = null, HttpParameters parameters = null)
        {
            HttpContent content = BuildJsonContent(jsonContent);
            SendRequest(requestUri, method, content, headers, parameters);
        }

        public virtual HttpContent BuildFormRequestContent(Dictionary<string, string> formdata)
        {
            MultipartFormDataContent content = new();

            if (formdata != null)
            {
                foreach (KeyValuePair<string, string> formvals in formdata)
                {
                    content.Add(new StringContent(formvals.Value), formvals.Key);
                }
            }
            return content;
        }

        public virtual HttpContent BuildUrlencodedContent(Dictionary<string, string> formdata)
        {
            FormUrlEncodedContent content = new(formdata);

            return content;
        }

        public virtual HttpContent BuildJsonContent(string jsonContent)
        {
            HttpContent content = new StringContent(jsonContent);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return content;
        }

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    try { Disconnect(); }
                    catch { }
                }
                _disposed = true;
            }
        }
    }
}
