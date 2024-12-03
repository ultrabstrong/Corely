namespace Corely.Common.Redaction;

public interface IRedactionProvider
{
    string? Redact(string? value);
}
