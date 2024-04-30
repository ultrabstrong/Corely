namespace Corely.IAM.Security
{
    public class SymmetricKey
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public string Key { get; set; } = null!;
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
    }
}
