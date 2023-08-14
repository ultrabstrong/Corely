namespace Corely.Shared.Providers.Data.Models
{
    [Serializable]
    public class ReadRecordResult
    {
        public List<string> Tokens { get; set; } = new List<string>();

        public long StartPosition { get; set; } = 0;

        public long Length { get; set; } = 0;

        public bool HasMore { get; set; } = true;

        public long EndPosition => StartPosition + Length;

        public override string ToString()
        {
            return string.Join(',', Tokens);
        }
    }
}
