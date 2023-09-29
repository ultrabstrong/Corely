using System.Text.Json;
using System.Text.Json.Serialization;

namespace Corely.Shared.Converters.Json
{
    public sealed class JsonDateTimeConverter : JsonConverter<DateTime?>
    {
        private const string format = "yyyy-MM-ddTHH:mm:ss";

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (DateTime.TryParse(reader.GetString(), out var date))
            {
                return date;
            }
            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString(format));
        }
    }
}
