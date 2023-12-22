using Corely.Common.Extensions;
using System.Text;

namespace Corely.UnitTests.Shared.Extensions
{
    public class BomReaderExtensionsTests
    {
        [Theory, MemberData(nameof(GetEncodingTestData))]
        public void GetEncoding_ShouldReturnCorrectEncoding(byte[] bom, Encoding expectedEncoding)
        {
            var encoding = bom.GetEncoding();
            Assert.Equal(expectedEncoding, encoding);
        }

        public static IEnumerable<object[]> GetEncodingTestData()
        {
#pragma warning disable SYSLIB0001 // Type or member is obsolete
            yield return new object[] { new byte[] { 0x2b, 0x2f, 0x76 }, Encoding.UTF7 };
#pragma warning restore SYSLIB0001 // Type or member is obsolete
            yield return new object[] { new byte[] { 0xef, 0xbb, 0xbf }, new UTF8Encoding(true) };
            yield return new object[] { new byte[] { 0xff, 0xfe, 0x00, 0x00 }, Encoding.UTF32 }; //UTF-32LE
            yield return new object[] { new byte[] { 0xff, 0xfe }, Encoding.Unicode }; //UTF-16LE
            yield return new object[] { new byte[] { 0xfe, 0xff }, Encoding.BigEndianUnicode }; //UTF-16BE
            yield return new object[] { new byte[] { 0x00, 0x00, 0xfe, 0xff }, new UTF32Encoding(true, true) };  //UTF-32BE
            yield return new object[] { new byte[] { 0x00, 0x00, 0x00, 0x00 }, new UTF8Encoding(false) };
        }

    }
}
