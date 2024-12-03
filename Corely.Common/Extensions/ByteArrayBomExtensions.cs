using System.Text;

namespace Corely.Common.Extensions;

public static class ByteArrayBOMExtensions
{
    private static readonly byte[] _utf8Bom = [0xef, 0xbb, 0xbf];
    private static readonly byte[] _utf32LittleEndianBom = [0xff, 0xfe, 0x00, 0x00];
    private static readonly byte[] _utf16LittleEndianBom = [0xff, 0xfe];
    private static readonly byte[] _utf16BigEndianBom = [0xfe, 0xff];
    private static readonly byte[] _utf32BigEndianBom = [0x00, 0x00, 0xfe, 0xff];

    /// <summary>
    /// Determines a text file's encoding by analyzing its byte order mark (BOM).
    /// Defaults to UTF8 when detection of the text file's endianness fails
    /// </summary>
    /// <param name="bom"></param>
    /// <returns>The detected encoding</returns>
    public static Encoding GetByteOrderMarkEncoding(this byte[] bom)
    {
        if (bom.IsMatch(_utf8Bom)) { return new UTF8Encoding(true); }
        if (bom.IsMatch(_utf32LittleEndianBom)) { return Encoding.UTF32; }
        if (bom.IsMatch(_utf16LittleEndianBom)) { return Encoding.Unicode; }
        if (bom.IsMatch(_utf16BigEndianBom)) { return Encoding.BigEndianUnicode; }
        if (bom.IsMatch(_utf32BigEndianBom)) { return new UTF32Encoding(true, true); }

        return new UTF8Encoding(false);
    }

    private static bool IsMatch(this byte[] source, byte[] pattern)
    {
        if (source.Length < pattern.Length) { return false; }

        for (int i = 0; i < pattern.Length; i++)
        {
            if (source[i] != pattern[i])
            {
                return false;
            }
        }

        return true;
    }
}
