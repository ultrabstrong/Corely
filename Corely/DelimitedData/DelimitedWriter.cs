namespace Corely.DelimitedData
{
    public class DelimitedWriter
    {
        public DelimitedWriter()
        {
            Setup(',', '"', Environment.NewLine);
        }

        public DelimitedWriter(Delimiter delim)
        {
            switch (delim)
            {
                case Delimiter.Tab:
                    Setup('\t', '"', Environment.NewLine);
                    break;
                case Delimiter.CSV:
                default:
                    Setup(',', '"', Environment.NewLine);
                    break;
            }
        }

        public DelimitedWriter(char _tokenDelimiter, char _tokenLiteral, string _recordDelimiter)
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

        private string WriteTokenToString(string token)
        {
            string tokenstring = token ?? "";
            tokenstring.Replace(TokenLiteral.ToString(), $"{TokenLiteral}{TokenLiteral}");

            if (tokenstring.Contains(TokenDelimiter) || tokenstring.Contains(RecordDelimiter))
            {
                tokenstring = $"{TokenLiteral}{tokenstring}{TokenLiteral}";
            }
            return tokenstring;
        }

        public string WriteRecordToString(List<string> record)
        {
            string recordstring = "";

            if (record.Count > 0)
            {
                recordstring = WriteTokenToString(record[0]);
            }

            for (int i = 1; i < record.Count; i++)
            {
                recordstring += $"{TokenDelimiter}{WriteTokenToString(record[i])}";
            }
            return recordstring;
        }

        public string WriteAllRecordsToString(List<List<string>> records)
        {
            string recordsstring = "";

            if (records.Count > 0)
            {
                recordsstring = WriteRecordToString(records[0]);
            }

            for (int i = 1; i < records.Count; i++)
            {
                recordsstring += $"{RecordDelimiter}{WriteRecordToString(records[i])}";
            }
            return recordsstring;
        }

        public void AppendRecordToFile(List<string> record, string filepath, bool include_record_delim)
        {
            string recordstring = WriteRecordToString(record);
            FileInfo file = new FileInfo(filepath);

            if (!file.Directory?.Exists ?? false) { file?.Directory?.Create(); }

            if (include_record_delim)
            {
                File.AppendAllText(filepath, $"{RecordDelimiter}{recordstring}");
            }
            else
            {
                File.AppendAllText(filepath, recordstring);
            }
        }

        public void AppendReadRecordToFile(ReadRecordResult record, string filepath, bool include_record_delim)
        {
            AppendRecordToFile(record.Tokens, filepath, include_record_delim);
        }

        public void AppendAllRecordsToFile(List<List<string>> records, string filepath)
        {
            if (records.Count > 0)
            {
                AppendRecordToFile(records[0], filepath, false);
            }

            for (int i = 1; i < records.Count; i++)
            {
                AppendRecordToFile(records[i], filepath, true);
            }
        }


        public void AppendAllReadRecordsToFile(List<ReadRecordResult> records, string filepath)
        {
            if (records.Count > 0)
            {
                AppendRecordToFile(records[0].Tokens, filepath, false);
            }

            for (int i = 1; i < records.Count; i++)
            {
                AppendRecordToFile(records[i].Tokens, filepath, true);
            }
        }
    }
}
