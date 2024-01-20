using AutoMapper;
using System.Text.Json;

namespace Corely.Domain.Mappers.AutoMapper.TypeConverters
{
    internal sealed class JsonifyTypeConverter<T> : ITypeConverter<T, string>
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public JsonifyTypeConverter()
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }

        public string Convert(T source, string destination, ResolutionContext context)
        {
            return JsonSerializer.Serialize(source, _jsonSerializerOptions);
        }
    }
}
