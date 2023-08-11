namespace Corely.Shared.Providers.Data
{
    public interface IDelimitedTextDataProvider
    {
        List<ReadRecordResult> ReadAllRecords(Stream stream);

        ReadRecordResult ReadNextRecord(Stream stream, long startPosition);

        void WriteAllRecords(List<List<string>> records, Stream writeTo);

        void WriteRecord(List<string> record, Stream writeTo);
    }
}
