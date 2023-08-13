using Corely.Shared.Providers.Data.Models;

namespace Corely.Shared.Providers.Data
{
    public interface IDelimitedTextProvider
    {
        List<ReadRecordResult> ReadAllRecords(Stream stream);

        ReadRecordResult ReadNextRecord(Stream stream, long startPosition);

        void WriteAllRecords(IEnumerable<IEnumerable<string>> records, Stream writeTo);

        void WriteRecord(IEnumerable<string> record, Stream writeTo);
    }
}
