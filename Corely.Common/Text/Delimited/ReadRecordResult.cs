namespace Corely.Common.Text.Delimited;

public class ReadRecordResult
{
    /// <summary>
    /// Tokens of the record (i.e. record values)
    /// </summary>
    public List<string> Tokens { get; set; } = [];

    /// <summary>
    /// Start position of the record in the data stream
    /// </summary>
    public long StartPosition { get; set; } = 0;

    /// <summary>
    /// Length of the record in the data stream
    /// </summary>
    public long Length { get; set; } = 0;

    /// <summary>
    /// Indicates if there are more records to read from the data stream
    /// </summary>
    public bool HasMore { get; set; } = true;

    /// <summary>
    /// End position of the record in the data stream
    /// </summary>
    public long EndPosition => StartPosition + Length;

    public override string ToString()
    {
        return string.Join(',', Tokens);
    }
}
