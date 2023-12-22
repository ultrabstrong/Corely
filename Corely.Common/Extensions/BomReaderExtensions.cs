using System.Text;

namespace Corely.Common.Extensions
{
    public static class BomReaderExtensions
    {
        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to UTF8 when detection of the text file's endianness fails
        /// </summary>
        /// <param name="bom"></param>
        /// <returns>The detected encoding</returns>
        public static Encoding GetEncoding(this byte[] bom)
        {
            // Analyze the BOM
#pragma warning disable SYSLIB0001 // Type or member is obsolete
            if (bom.Length > 2 && bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
#pragma warning restore SYSLIB0001 // Type or member is obsolete
            if (bom.Length > 2 && bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return new UTF8Encoding(true);
            if (bom.Length > 3 && bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return Encoding.UTF32; //UTF-32LE
            if (bom.Length > 1 && bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom.Length > 1 && bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom.Length > 3 && bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new UTF32Encoding(true, true);  //UTF-32BE

            // Encoding not specified. Return UTF8 as default
            return new UTF8Encoding(false);
        }
    }
}
