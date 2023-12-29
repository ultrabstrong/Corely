using Corely.Common.Converters.Json;
using Corely.UnitTests.ClassData;
using System.Text;
using System.Text.Json;

namespace Corely.UnitTests.Common.Converters.Json
{
    public class JsonDateTimeConverterTests
    {
        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData("true")]
        [InlineData("1")]
        public void Read_ReturnsNull_WhenInputIsNotDate(string input)
        {
            var converter = new JsonDateTimeConverter();
            var reader = GetReader(input);
            var result = converter.Read(ref reader, typeof(DateTime?), null);

            Assert.Null(result);
        }

        [Theory, MemberData(nameof(ReadTestData))]
        public void Read_ReturnsDate_WhenInputIsDate(string input, DateTime expected)
        {
            var converter = new JsonDateTimeConverter();
            var reader = GetReader(input);
            var result = converter.Read(ref reader, typeof(DateTime?), null);

            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> ReadTestData()
        {
            yield return new object[] { "2020-01-01", new DateTime(2020, 1, 1) };
            yield return new object[] { "2020-01-01T00:00:00", new DateTime(2020, 1, 1) };
            yield return new object[] { "2020-01-01T00:00:01", new DateTime(2020, 1, 1, 0, 0, 1) };
            yield return new object[] { "2020-01-01T00:01:00", new DateTime(2020, 1, 1, 0, 1, 0) };
            yield return new object[] { "2020-01-01T01:00:00", new DateTime(2020, 1, 1, 1, 0, 0) };
        }

        private static Utf8JsonReader GetReader(string input)
        {
            byte[] jsonData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(input));
            var jsonSpan = new ReadOnlySpan<byte>(jsonData);
            var reader = new Utf8JsonReader(jsonSpan);
            reader.Read();
            return reader;
        }

        [Theory, MemberData(nameof(WriteTestData))]
        public void Write_ReturnsDate_WhenInputIsDate(DateTime input, string expected)
        {
            var converter = new JsonDateTimeConverter();
            using var memoryStream = new MemoryStream();

            var writer = new Utf8JsonWriter(memoryStream);
            converter.Write(writer, input, null);
            writer.Flush();

            memoryStream.Position = 0;
            var result = Encoding.UTF8.GetString(memoryStream.ToArray());

            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> WriteTestData()
        {
            yield return new object[] { new DateTime(2020, 1, 1), "\"2020-01-01T00:00:00\"" };
            yield return new object[] { new DateTime(2020, 1, 1, 0, 0, 1), "\"2020-01-01T00:00:01\"" };
            yield return new object[] { new DateTime(2020, 1, 1, 0, 1, 0), "\"2020-01-01T00:01:00\"" };
            yield return new object[] { new DateTime(2020, 1, 1, 1, 0, 0), "\"2020-01-01T01:00:00\"" };
        }
    }
}
