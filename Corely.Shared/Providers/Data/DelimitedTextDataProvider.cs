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

        public List<ReadRecordResult> ReadAllRecords(Stream stream)
        {
            List<ReadRecordResult> records = new();
            ReadRecordResult record = new();
            do
            {
                record = ReadNextRecord(stream, 0);
                records.Add(record);
            }
            while (record.HasMore);

            return records;
        }

        public ReadRecordResult ReadNextRecord(Stream stream, long startPosition)
        {
            ReadRecordResult result;
            long streamLength = 0;

            byte[] bom = new byte[4];
            stream.Read(bom, 0, 4);
            Encoding encoding = BomReaderExtensions.GetEncoding(bom);
            stream.Position = startPosition;

            streamLength = stream.Length;
            result = ReadNextRecord(stream, encoding);
            result.StartPosition = startPosition;

            if (result != null && result.EndPosition >= streamLength)
            {
                result.HasMore = false;
            }
            return result ?? new() { HasMore = false };
        }

        private ReadRecordResult ReadNextRecord(Stream stream, Encoding encoding)
        {
            ReadRecordResult result = new();

            string currentToken = "";
            int currentRecordDelim = 0;

            bool isInLiteral = false,
                lastCharEscaped = false,
                lastTokenLiteralEscaped = false;

            StreamReader streamReader = new(stream, encoding);

            // Increase length for BOM if this is the start of a UTF8-BOM steam
            if (stream.Position == 0 && Equals(new UTF8Encoding(true), streamReader.CurrentEncoding)) { result.Length += 3; }
            while (!streamReader.EndOfStream)
            {
                // Get next character
                char c = (char)streamReader.Read();
                // Increase length for positioning
                result.Length += streamReader.CurrentEncoding.GetByteCount(new[] { c });
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
            // Push last token
            result.Tokens.Add(currentToken);
            // Return record
            return result;
        }

        public void WriteAllRecords(List<List<string>> records, Stream writeTo)
        {
            StreamWriter writer = new(writeTo, Encoding.UTF8);
            if (records.Count > 0)
            {
                WriteRecord(records[0], writer);
            }

            for (int i = 1; i < records.Count; i++)
            {
                writer.Write(_recordDelimiter);
                WriteRecord(records[i], writer);
            }
            writer.Flush();
        }

        public void WriteRecord(List<string> record, Stream writeTo)
        {
            StreamWriter writer = new(writeTo, Encoding.UTF8);
            WriteRecord(record, writer);
            writer.Flush();
        }

        private void WriteRecord(List<string> record, StreamWriter writer)
        {
            if (record.Count > 0)
            {
                AppendTokenLiteral(record[0], writer);
            }

            for (int i = 1; i < record.Count; i++)
            {
                writer.Write(_tokenDelimiter);
                AppendTokenLiteral(record[i], writer);
            }
        }

        private void AppendTokenLiteral(string token, StreamWriter writer)
        {
            token ??= "";
            token = token.Replace(_tokenLiteral.ToString(), $"{_tokenLiteral}{_tokenLiteral}");

            if (token.Contains(_tokenDelimiter) || token.Contains(_recordDelimiter))
            {
                token = $"{_tokenLiteral}{token}{_tokenLiteral}";
            }
            writer.Write(token);
        }
    }
}
