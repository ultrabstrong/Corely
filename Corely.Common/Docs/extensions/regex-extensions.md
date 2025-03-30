# Regex Extensions

Extensions for the `System.Text.RegularExpressions.Regex` class are provided to simplify the use of regular expressions.

## ReplaceGroups

This extension replaces the groups in a regular expression with a value.

Example:
```csharp
var input = "Hello, World!";
var replacement = "redacted";
var regex = new Regex(@"(Hello), (World)!");
var result = regex.ReplaceGroups(input, replacement);
Console.WriteLine(result); // Output: "redacted, redacted!"
```

This is useful for sanitizing logs or other data. See the [Redaction Provider](../redaction-provider.md) docs for more information.
