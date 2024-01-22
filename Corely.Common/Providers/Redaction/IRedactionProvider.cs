namespace Corely.Common.Providers.Redaction
{
    public interface IRedactionProvider
    {
        string? Redact(string? value);
    }
}
