# Redaction Provider

The `IRedactionProvider` interface is used for redacting sensitive information from strings. 
The `RedactionProviderBase` abstract class implements `IRedactionProvider` and handles redaction using the [ReplaceGroups](extensions/regex.md#replacegroups)  Regex extension.

## Features

- Redact sensitive information from strings
- Configurable via regular expressions
- Easily extensible with custom redaction patterns
- These pre-built redaction providers are included:
  - `PasswordRedactionProvider` : Redact passwords from JSON or Serilog destructured object
  - More to come!

## Usage

Define a new redaction provider:

```chsarp
public partial class MyRedactionProvider : RedactionProviderBase
{
    protected override List<Regex> GetReplacePatterns() => [
        RegexToReplace()
    ];

    [GeneratedRegex(@"(text-to-replace)", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex RegexToReplace();
}
```

Use the redaction provider to redact text from the string:
```csharp
IRedactionProvider redactionProvider = new MyRedactionProvider();
string? redacted = redactionProvider.Redact("text-to-replace");
Console.WriteLine(redacted); // Output: "REDACTED"
```
