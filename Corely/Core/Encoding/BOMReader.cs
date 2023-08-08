using System.Text;

namespace Corely.Core.Encoding
{
    public static class BOMReader
    {
        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to UTF8 when detection of the text file's endianness fails
        /// </summary>
        /// <param name="bom"></param>
        /// <returns>The detected encoding</returns>
        public static System.Text.Encoding GetEncoding(byte[] bom)
        {
            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return System.Text.Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return new UTF8Encoding(true);
            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return System.Text.Encoding.UTF32; //UTF-32LE
            if (bom[0] == 0xff && bom[1] == 0xfe) return System.Text.Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return System.Text.Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new UTF32Encoding(true, true);  //UTF-32BE

            // Encoding not specified. Return UTF8 as default
            return new UTF8Encoding(false);
        }
    }
}
