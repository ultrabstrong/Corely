using Corely.Shared.Core.Encoding;
using System.Text;

namespace Corely.UnitTests.Shared.Core.Encoding
{
    public class BomReaderTests
    {
        public static IEnumerable<object[]> GetBomReaderTestData()
        {
#pragma warning disable SYSLIB0001 // Type or member is obsolete
            yield return new object[] { new byte[] { 0x2b, 0x2f, 0x76 }, System.Text.Encoding.UTF7 };
#pragma warning restore SYSLIB0001 // Type or member is obsolete
            yield return new object[] { new byte[] { 0xef, 0xbb, 0xbf }, new UTF8Encoding(true) };
            yield return new object[] { new byte[] { 0xff, 0xfe, 0x00, 0x00 }, System.Text.Encoding.UTF32 }; //UTF-32LE
            yield return new object[] { new byte[] { 0xff, 0xfe }, System.Text.Encoding.Unicode }; //UTF-16LE
            yield return new object[] { new byte[] { 0xfe, 0xff }, System.Text.Encoding.BigEndianUnicode }; //UTF-16BE
            yield return new object[] { new byte[] { 0x00, 0x00, 0xfe, 0xff }, new UTF32Encoding(true, true) };  //UTF-32BE
            yield return new object[] { new byte[] { 0x00, 0x00, 0x00, 0x00 }, new UTF8Encoding(false) };
        }

        [Theory, MemberData(nameof(GetBomReaderTestData))]
        public void GetEncoding_ShouldReturnCorrectEncoding(byte[] bom, System.Text.Encoding expectedEncoding)
        {
            var encoding = BOMReader.GetEncoding(bom);
            Assert.Equal(expectedEncoding, encoding);
        }

    }
}
