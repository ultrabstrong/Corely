using System.Text.Json;
using System.Text.Json.Serialization;

namespace Corely.Shared.Attributes.Json
{
    public sealed class JsonDateConverter : JsonConverter<DateTime?>
    {
        private const string format = "yyyy-MM-dd";

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
