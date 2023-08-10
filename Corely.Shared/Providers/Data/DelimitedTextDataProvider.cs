using Corely.Shared.Extensions;
using System.Text;

namespace Corely.Shared.Providers.Data
{
    public class DelimitedTextDataProvider : IDelimitedTextDataProvider
    {
        private readonly char _tokenDelimiter;
        private readonly char _tokenLiteral;
        private readonly string _recordDelimiter;

        public DelimitedTextDataProvider() : this(',', '"', Environment.NewLine)
        {
        }

        public DelimitedTextDataProvider(Delimiter delimiter)
        {
            (_tokenDelimiter, _tokenLiteral, _recordDelimiter) =
                delimiter switch
                {
                    Delimiter.Semicolon => (';', '"', Environment.NewLine),
                    Delimiter.Pipe => ('|', '"', Environment.NewLine),
                    Delimiter.Tab => ('\t', '"', Environment.NewLine),
                    _ => (',', '"', Environment.NewLine),
                };
        }

        public DelimitedTextDataProvider(char tokenDelimiter, char tokenLiteral, string recordDelimiter)
        {
            (_tokenDelimiter, _tokenLiteral, _recordDelimiter) = (tokenDelimiter, tokenLiteral, recordDelimiter);
        }

        public ReadRecordResult ReadStringRecord(long startPosition, byte[] dataBytes)
        {
            ReadRecordResult result;
            long streamLength = 0;

            using (MemoryStream stream = new(dataBytes))
            {
                byte[] bom = new byte[4];
                stream.Read(bom, 0, 4);
                Encoding encoding = BomReaderExtensions.GetEncoding(bom);
                stream.Position = startPosition;

                streamLength = stream.Length;
                result = ReadRecordForStream(stream, encoding);
                result.StartPosition = startPosition;
            }

            if (result != null && result.EndPosition >= streamLength)
            {
                result.HasMore = false;
            }
            return result ?? new() { HasMore = false };
        }

        public ReadRecordResult ReadFileRecord(long startPosition, string filePath)
        {
            ReadRecordResult result;
            long streamLength = 0;

            using (FileStream stream = new(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] bom = new byte[4];
                stream.Read(bom, 0, 4);
                Encoding encoding = BomReaderExtensions.GetEncoding(bom);
                stream.Position = startPosition;

                streamLength = stream.Length;
                result = ReadRecordForStream(stream, encoding);
                result.StartPosition = startPosition;
            }

            if (result != null && result.EndPosition >= streamLength)
            {
                result.HasMore = false;
            }
            return result ?? new() { HasMore = false };
        }

        private ReadRecordResult ReadRecordForStream(Stream s, Encoding encoding)
        {
            ReadRecordResult result = new();

            string currentToken = "";
            int currentRecordDelim = 0;
            bool isInLiteral = false,
                lastCharEscaped = false,
                lastTokenLiteralEscaped = false;

            using (StreamReader sr = new(s, encoding))
            {
                // Increase length for BOM if this is the start of a UTF8-BOM steam
                if (s.Position == 0 && Equals(new UTF8Encoding(true), sr.CurrentEncoding)) { result.Length += 3; }
                while (!sr.EndOfStream)
                {
                    // Get next character
                    char c = (char)sr.Read();
                    // Increase length for positioning
                    result.Length += sr.CurrentEncoding.GetByteCount(new[] { c });
                    // Check if current char is a literal
                    if (c == _tokenLiteral)
                    {
                        // If not currently in a literal block
                        if (!isInLiteral)
                        {
                            // If data is empty then literal is translated as 'begin block'
                            if (string.IsNullOrEmpty(currentToken))
                            {
                                // Enter literal block
                                isInLiteral = true;
                            }
                            // Only append literal if literal isn't already escaped
                            else if (currentToken.Length > 0 && currentToken[^1] != _tokenLiteral)
                            {
                                // Append literal to data string without entering block
                                currentToken += c;
                                lastTokenLiteralEscaped = false;
                            }
                        }
                        // Reader is currently in a literal block
                        else
                        {
                            // If last char was also a literal
                            if (currentToken.Length > 0 && currentToken[^1] == _tokenLiteral)
                            {
                                // If last char was already escaped push this one
                                if (lastCharEscaped)
                                {
                                    currentToken += c;
                                    lastCharEscaped = false;
                                    lastTokenLiteralEscaped = false;
                                }
                                else
                                {
                                    // Escape last char and discard this one
                                    lastCharEscaped = true;
                                    lastTokenLiteralEscaped = true;
                                }
                            }
                            // Last char was not a literal
                            else
                            {
                                // Push this char and reset last escaped
                                currentToken += c;
                                lastCharEscaped = false;
                                lastTokenLiteralEscaped = false;
                            }
                        }
                    }
                    // Check if current char is a token delimiter
                    else if (c == _tokenDelimiter)
                    {
                        // Add to current data if in literal.
                        if (isInLiteral)
                        {
                            // If last char was an unescaped literal
                            if (currentToken.Length > 0 && currentToken[^1] == _tokenLiteral && !lastCharEscaped)
                            {
                                // Token is complete. Remove last literal char, reset vars, and push token
                                currentToken = currentToken[..^1];
                                result.Tokens.Add(currentToken);
                                currentToken = "";
                                isInLiteral = false;
                            }
                            // Last char was not an unescaped literal
                            else
                            {
                                // Push this char
                                currentToken += c;
                            }
                        }
                        // Push current data to csv data and reset if not in literal
                        else
                        {
                            result.Tokens.Add(currentToken);
                            currentToken = "";
                            isInLiteral = false;
                        }
                        // Reset last char escaped
                        lastCharEscaped = false;
                    }
                    else
                    {
                        // Add to current data
                        currentToken += c;
                        // Reset last char escaped
                        lastCharEscaped = false;
                    }

                    // Check current character belongs to the next expected sequence for a record delimiter string
                    if (c == _recordDelimiter[currentRecordDelim])
                    {
                        // If not currently in a literal block
                        if (!isInLiteral)
                        {
                            // Record delimiter has been fully matched so record is read
                            if (currentRecordDelim == _recordDelimiter.Length - 1)
                            {
                                // Remove record delimiter chars from current data
                                currentToken = currentToken[..^_recordDelimiter.Length];
                                // Record is complete. Exit reader
                                break;
                            }
                            // Record delimiter is only partially matched
                            else
                            {
                                // Move to the next record delimiter character
                                currentRecordDelim++;
                            }
                        }
                        else
                        {
                            // If this is the end of a new line in an unescaped literal
                            if (currentRecordDelim == _recordDelimiter.Length - 1)
                            {
                                // If the character before the record delimiter is a literal char
                                if (currentToken[currentToken.Length - _recordDelimiter.Length - 1] == _tokenLiteral)
                                {
                                    // Find out if the token literal before the record delimiter was escaped or not
                                    if (lastTokenLiteralEscaped)
                                    {
                                        // Token is not complete, and record delimiter should be included. Keep delimiter and carry on
                                        currentRecordDelim = 0;
                                    }
                                    else
                                    {
                                        // Token is complete. Remove last literal char, record delimiter chars, reset vars, and push token
                                        currentToken = currentToken[..(currentToken.Length - _recordDelimiter.Length - 1)];
                                        isInLiteral = false;
                                        // Record is complete. Exit reader
                                        break;
                                    }
                                }
                                else
                                {
                                    // Token is not complete, and record delimiter should be included. Keep delimiter and carry on
                                    currentRecordDelim = 0;
                                }
                            }
                            // Record delimiter is only partially matched
                            else
                            {
                                // Move to the next record delimiter character
                                currentRecordDelim++;
                            }
                        }
                    }
                    else
                    {
                        // Record delimiter not matched. Reset the current record delim
                        currentRecordDelim = 0;
                    }
                }
            }
            // Push last token
            result.Tokens.Add(currentToken);
            // Return record
            return result;
        }

        public List<ReadRecordResult> ReadAllStringRecords(string data)
        {
            List<ReadRecordResult> records = new();
            byte[] dataBytes = Encoding.ASCII.GetBytes(data);
            ReadRecordResult record = new();
            do
            {
                record = ReadStringRecord(record.EndPosition, dataBytes);
                records.Add(record);
            }
            while (record.HasMore);

            return records;
        }

        public List<ReadRecordResult> ReadAllFileRecords(string filePath)
        {
            List<ReadRecordResult> records = new();
            ReadRecordResult record = new();
            do
            {
                record = ReadFileRecord(record.EndPosition, filePath);
                records.Add(record);
            }
            while (record.HasMore);

            return records;
        }

        private string WriteTokenToString(string token)
        {
            token ??= "";
            token = token.Replace(_tokenLiteral.ToString(), $"{_tokenLiteral}{_tokenLiteral}");

            if (token.Contains(_tokenDelimiter) || token.Contains(_recordDelimiter))
            {
                token = $"{_tokenLiteral}{token}{_tokenLiteral}";
            }
            return token;
        }

        public string WriteRecordToString(List<string> record)
        {
            string result = "";

            if (record.Count > 0)
            {
                result = WriteTokenToString(record[0]);
            }

            for (int i = 1; i < record.Count; i++)
            {
                result += $"{_tokenDelimiter}{WriteTokenToString(record[i])}";
            }
            return result;
        }

        public string WriteAllRecordsToString(List<List<string>> records)
        {
            string result = "";

            if (records.Count > 0)
            {
                result = WriteRecordToString(records[0]);
            }

            for (int i = 1; i < records.Count; i++)
            {
                result += $"{_recordDelimiter}{WriteRecordToString(records[i])}";
            }
            return result;
        }

        public void AppendRecordToFile(List<string> record, string filePath, bool includeRecordDelim)
        {
            string result = WriteRecordToString(record);
            FileInfo file = new(filePath);

            if (!file.Directory?.Exists ?? false) { file?.Directory?.Create(); }

            if (includeRecordDelim)
            {
                File.AppendAllText(filePath, $"{_recordDelimiter}{result}");
            }
            else
            {
                File.AppendAllText(filePath, result);
            }
        }

        public void AppendReadRecordToFile(ReadRecordResult record, string filePath, bool includeRecordDelim)
        {
            AppendRecordToFile(record.Tokens, filePath, includeRecordDelim);
        }

        public void AppendAllRecordsToFile(List<List<string>> records, string filePath)
        {
            if (records.Count > 0)
            {
                AppendRecordToFile(records[0], filePath, false);
            }

            for (int i = 1; i < records.Count; i++)
            {
                AppendRecordToFile(records[i], filePath, true);
            }
        }

        public void AppendAllReadRecordsToFile(List<ReadRecordResult> records, string filePath)
        {
            if (records.Count > 0)
            {
                AppendRecordToFile(records[0].Tokens, filePath, false);
            }

            for (int i = 1; i < records.Count; i++)
            {
                AppendRecordToFile(records[i].Tokens, filePath, true);
            }
        }

    }
}
