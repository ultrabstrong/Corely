namespace Corely.Common.Text.Delimited;

public interface IDelimitedTextProvider
{
    List<ReadRecordResult> ReadAllRecords(Stream stream);

    ReadRecordResult ReadNextRecord(Stream stream, long startPosition);

    void WriteAllRecords(IEnumerable<IEnumerable<string>> records, Stream writeTo);

    void WriteRecord(IEnumerable<string> record, Stream writeTo);
}
