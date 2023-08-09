using Corely.Shared.Core.Encoding;
using System.Text;

namespace Corely.Shared.DelimitedData
{
    public class DelimitedReader
    {
        #region Constructors / Initialization

        public DelimitedReader()
        {
            Setup(',', '"', Environment.NewLine);
        }

        public DelimitedReader(Delimiter delim)
        {
            switch (delim)
            {
                case Delimiter.Semicolon:
                    Setup(';', '"', Environment.NewLine);
                    break;
                case Delimiter.Pipe:
                    Setup('|', '"', Environment.NewLine);
                    break;
                case Delimiter.Tab:
                    Setup('\t', '"', Environment.NewLine);
                    break;
                case Delimiter.CSV:
                default:
                    Setup(',', '"', Environment.NewLine);
                    break;
            }
        }

        public DelimitedReader(char _tokenDelimiter, char _tokenLiteral, string _recordDelimiter)
        {
            Setup(_tokenDelimiter, _tokenLiteral, _recordDelimiter);
        }

        private void Setup(char _tokenDelimiter, char _tokenLiteral, string _recordDelimiter)
        {
            TokenDelimiter = _tokenDelimiter;
            TokenLiteral = _tokenLiteral;
            RecordDelimiter = _recordDelimiter;
        }

        public char TokenDelimiter { get; internal set; }
        public char TokenLiteral { get; internal set; }
        public string RecordDelimiter { get; internal set; }

        private ReadRecordResult ReadRecordForStream(Stream s, System.Text.Encoding encoding)
        {
            // Create record to return
            ReadRecordResult r = new();
            // Init variables for reading record
            string currenttoken = "";
            int currentrecorddelim = 0;
            bool isinliteral = false, lastcharescaped = false, lasttokenliteralescaped = false;

            using (StreamReader sr = new(s, encoding))
            {
                // Increase length for BOM if this is the start of a UTF8-BOM steam
                if (s.Position == 0 && Equals(new UTF8Encoding(true), sr.CurrentEncoding)) { r.Length += 3; }
                while (!sr.EndOfStream)
                {
                    // Get next character
                    char c = (char)sr.Read();
                    // Increase length for positioning
                    r.Length += sr.CurrentEncoding.GetByteCount(new[] { c });
                    // Check if current char is a literal
                    if (c == TokenLiteral)
                    {
                        // If not currently in a literal block
                        if (!isinliteral)
                        {
                            // If data is empty then literal is translated as 'begin block'
                            if (string.IsNullOrEmpty(currenttoken))
                            {
                                // Enter literal block
                                isinliteral = true;
                            }
                            // Only append literal if literal isn't already escaped
                            else if (currenttoken.Length > 0 && currenttoken[^1] != TokenLiteral)
                            {
                                // Append literal to data string without entering block
                                currenttoken += c;
                                lasttokenliteralescaped = false;
                            }
                        }
                        // Reader is currently in a literal block
                        else
                        {
                            // If last char was also a literal
                            if (currenttoken.Length > 0 && currenttoken[^1] == TokenLiteral)
                            {
                                // If last char was already escaped push this one
                                if (lastcharescaped)
                                {
                                    currenttoken += c;
                                    lastcharescaped = false;
                                    lasttokenliteralescaped = false;
                                }
                                else
                                {
                                    // Escape last char and discard this one
                                    lastcharescaped = true;
                                    lasttokenliteralescaped = true;
                                }
                            }
                            // Last char was not a literal
                            else
                            {
                                // Push this char and reset last escaped
                                currenttoken += c;
                                lastcharescaped = false;
                                lasttokenliteralescaped = false;
                            }
                        }
                    }
                    // Check if current char is a token delimiter
                    else if (c == TokenDelimiter)
                    {
                        // Add to current data if in literal.
                        if (isinliteral)
                        {
                            // If last char was an unescaped literal
                            if (currenttoken.Length > 0 && currenttoken[^1] == TokenLiteral && !lastcharescaped)
                            {
                                // Token is complete. Remove last literal char, reset vars, and push token
                                currenttoken = currenttoken[..^1];
                                r.Tokens.Add(currenttoken);
                                currenttoken = "";
                                isinliteral = false;
                            }
                            // Last char was not an unescaped literal
                            else
                            {
                                // Push this char
                                currenttoken += c;
                            }
                        }
                        // Push current data to csv data and reset if not in literal
                        else
                        {
                            r.Tokens.Add(currenttoken);
                            currenttoken = "";
                            isinliteral = false;
                        }
                        // Reset last char escaped
                        lastcharescaped = false;
                    }
                    else
                    {
                        // Add to current data
                        currenttoken += c;
                        // Reset last char escaped
                        lastcharescaped = false;
                    }

                    // Check current character belongs to the next expected sequence for a record delimiter string
                    if (c == RecordDelimiter[currentrecorddelim])
                    {
                        // If not currently in a literal block
                        if (!isinliteral)
                        {
                            // Record delimiter has been fully matched so record is read
                            if (currentrecorddelim == RecordDelimiter.Length - 1)
                            {
                                // Remove record delimiter chars from current data
                                currenttoken = currenttoken[..^RecordDelimiter.Length];
                                // Record is complete. Exit reader
                                break;
                            }
                            // Record delimiter is only partially matched
                            else
                            {
                                // Move to the next record delimiter character
                                currentrecorddelim++;
                            }
                        }
                        else
                        {
                            // If this is the end of a new line in an unescaped literal
                            if (currentrecorddelim == RecordDelimiter.Length - 1)
                            {
                                // If the character before the record delimiter is a literal char
                                if (currenttoken[currenttoken.Length - RecordDelimiter.Length - 1] == TokenLiteral)
                                {
                                    // Find out if the token literal before the record delimiter was escaped or not
                                    if (lasttokenliteralescaped)
                                    {
                                        // Token is not complete, and record delimiter should be included. Keep delimiter and carry on
                                        currentrecorddelim = 0;
                                    }
                                    else
                                    {
                                        // Token is complete. Remove last literal char, record delimiter chars, reset vars, and push token
                                        currenttoken = currenttoken[..(currenttoken.Length - RecordDelimiter.Length - 1)];
                                        isinliteral = false;
                                        // Record is complete. Exit reader
                                        break;
                                    }
                                }
                                else
                                {
                                    // Token is not complete, and record delimiter should be included. Keep delimiter and carry on
                                    currentrecorddelim = 0;
                                }
                            }
                            // Record delimiter is only partially matched
                            else
                            {
                                // Move to the next record delimiter character
                                currentrecorddelim++;
                            }
                        }
                    }
                    else
                    {
                        // Record delimiter not matched. Reset the current record delim
                        currentrecorddelim = 0;
                    }
                }
            }
            // Push last token
            r.Tokens.Add(currenttoken);
            // Return record
            return r;
        }

        public ReadRecordResult ReadStringRecord(long startposition, byte[] data_bytes)
        {
            ReadRecordResult result;
            long mslength = 0;

            using (MemoryStream ms = new(data_bytes))
            {
                byte[] bom = new byte[4];
                ms.Read(bom, 0, 4);
                Encoding encoding = BOMReader.GetEncoding(bom);
                ms.Position = startposition;

                mslength = ms.Length;
                result = ReadRecordForStream(ms, encoding);
                result.StartPosition = startposition;
            }

            if (result != null && result.EndPosition >= mslength)
            {
                result.HasMore = false;
            }
            return result ?? new() { HasMore = false };
        }

        public List<ReadRecordResult> ReadAllStringRecords(string data)
        {
            List<ReadRecordResult> records = new();
            byte[] data_bytes = Encoding.ASCII.GetBytes(data);
            ReadRecordResult record = new();
            do
            {
                record = ReadStringRecord(record.EndPosition, data_bytes);
                records.Add(record);
            }
            while (record.HasMore);

            return records;
        }

        public ReadRecordResult ReadFileRecord(long startposition, string filepath)
        {
            ReadRecordResult result;
            long fslength = 0;

            using (FileStream fs = new(filepath, FileMode.Open, FileAccess.Read))
            {
                byte[] bom = new byte[4];
                fs.Read(bom, 0, 4);
                Encoding encoding = BOMReader.GetEncoding(bom);

                fs.Position = startposition;
                fslength = fs.Length;
                result = ReadRecordForStream(fs, encoding);
                result.StartPosition = startposition;
            }

            if (result != null && result.EndPosition >= fslength)
            {
                result.HasMore = false;
            }
            return result ?? new() { HasMore = false };
        }

        public List<ReadRecordResult> ReadAllFileRecords(string filepath)
        {
            List<ReadRecordResult> records = new();
            ReadRecordResult record = new();
            do
            {
                record = ReadFileRecord(record.EndPosition, filepath);
                records.Add(record);
            }
            while (record.HasMore);

            return records;
        }

        #endregion
    }
}
