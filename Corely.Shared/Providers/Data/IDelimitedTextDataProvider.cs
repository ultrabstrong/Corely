namespace Corely.Shared.Providers.Data
{
    public interface IDelimitedTextDataProvider
    {
        ReadRecordResult ReadStringRecord(long startposition, byte[] data_bytes);

        List<ReadRecordResult> ReadAllStringRecords(string data);

        ReadRecordResult ReadFileRecord(long startposition, string filepath);

        List<ReadRecordResult> ReadAllFileRecords(string filepath);

        string WriteRecordToString(List<string> record);

        string WriteAllRecordsToString(List<List<string>> records);

        void AppendRecordToFile(List<string> record, string filepath, bool includeRecordDelim);

        void AppendReadRecordToFile(ReadRecordResult record, string filepath, bool includeRecordDelim);

        void AppendAllRecordsToFile(List<List<string>> records, string filepath);

        void AppendAllReadRecordsToFile(List<ReadRecordResult> records, string filepath);
    }
}
