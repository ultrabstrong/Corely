# Corely Common
This library contains common utilities to be used in other projects.

## Table of Contents
- [Installation](#installation)
- [Getting Started](#getting-started)
- [Repository](#repository)
- [Included Utilities](#included-utilities)
  - [Redaction Provider](#redaction-provider)
  - [Paged Result](#paged-result)
  - [Dispose Base](#dispose-base)
  - [File Paths](#file-paths)
    - [FilePathProvider](#filepathprovider)
  - [Extensions](#extensions)
    - [Regex Extensions](#regex-extensions)
      - [ReplaceGroups](#replacegroups) 
    - [Null Checking Extensions](#null-checking-extensions)
    - [String Extensions For Encoding And Decoding](#string-extension-for-encoding-and-decoding)
      - [Base64](#base64)
      - [URL](#url)
    - [Byte Array Extension For Finding BOM (Byte Order Mark)](#byte-array-extension-for-finding-bom-byte-order-mark)
  - [Converters](#converters)
    - [Json Date / DateTime Converters](#json-date--datetime-converters)
- [Contributing](#contributing)
- [License](#license)

## Installation
`dotnet add package Corely.Common`

## Getting Started
Here's a brief example of how to use the library:

## Repository
[Corely.Common](https://github.com/ultrabstrong/Corely/tree/master/Corely.Common)

## Included Utilities
- [Redaction Provider](#redaction-provider)
- [Paged Result](#paged-result)
- [Dispose Base](#dispose-base)
- [File Paths](#file-paths)
  - [FilePathProvider](#filepathprovider)
- [Extensions](#extensions)
  - [Regex Extensions](#regex-extensions)
    - [ReplaceGroups](#replacegroups) 
  - [Null Checking Extensions](#null-checking-extensions)
  - [String Extensions For Encoding And Decoding](#string-extension-for-encoding-and-decoding)
    - [Base64](#base64)
    - [URL](#url)
  - [Byte Array Extension For Finding BOM (Byte Order Mark)](#byte-array-extension-for-finding-bom-byte-order-mark)
- [Converters](#converters)
  - [Json Date / DateTime Converters](#json-date--datetime-converters)

## Redaction Provider
`IRedactionProvider` is an interface that provides a method to redact sensitive information from a string. The `RedactionProviderBase` class implements this interface and provides a default implementation for redacting sensitive information from a string. The `RedactionProviderBase` class uses the [ReplaceGroups Regex Extension](#replacegroups) to redact sensitive information from a string. You can easily create new redaction providers:
public partial class MyRedactionProvider : RedactionProviderBase
{
    protected override List<Regex> GetReplacePatterns() => [
        RegexToReplace()
    ];

    [GeneratedRegex(@"(text-to-replace)", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
    private static partial Regex RegexToReplace();
}

Example usage:
var redactionProvider = new MyRedactionProvider();
var redacted = redactionProvider.Redact("text-to-replace");

## Paged Result
`PagedResult<T>` is a class that helps in managing paginated data. It provides properties and methods to handle pagination logic, such as skipping items, taking a specific number of items, and determining if there are more items to fetch.

Example usage:
var pagedResult = new PagedResult<MyItem>(0, 10); 
pagedResult.OnGetNextChunk += (pagedResponse) =>
{
    pagedResponse.AddItems(
        _dataSource
        .Skip(pagedResponse.Skip)
        .Take(pagedResponse.Take));
    return pagedResponse;
}; 
var nextChunk = pagedResult.GetNextChunk();

### Properties
- `Items`: The list of items in the current page.
- `Skip`: The number of items to skip.
- `Take`: The number of items to take.
- `PageNum`: The current page number.
- `HasMore`: Indicates if there are more items to fetch.

### Methods
- `GetNextChunk()`: Fetches the next chunk of items.
- `SetItems(IEnumerable<T> items)`: Sets the items for the current page.
- `AddItems(IEnumerable<T> items)`: Adds items to the current page.

### Events
- `OnGetNextChunk`: An event that is triggered to fetch the next chunk of items when `GetNextChunk` is called.

## Dispose Base
`DisposeBase` is an abstract class that implements `IDisposable` and `IAsyncDisposable`. It provides `Dispose` and `DisposeAsync` public methods and has protected overrides for disposing managed and unmanaged resources.

Example Usage:
public class MyClass : DisposeBase
{
    protected override void DisposeManagedResources()
    {
        // Dispose managed resources
    }
    protected override void DisposeUnmanagedResources()
    {
        // Dispose unmanaged resources
    }
    protected async override ValueTask DisposeAsyncCore(){
        // Dispose managed resources asynchronously
    }}
}

## File Paths
`IFilePathProvider` supports the following operations:
- `DoesFileExist` : Checks if a file exists
- `GetOverwriteProtectedPath` : Returns a path with a file name that is protected from overwriting
- `GetFileNameWithExtension` : Returns the file name with extension
- `GetFileNameWithoutExtension` : Returns the file name without extension

The following implementations of `IFilePathProvider` are included:
### FilePathProvider
This implements `IFilePathProvider` for local file paths. It is used to provide file path operations for local files
var provider = new FilePathProvider();
var path = provider.GetOverwriteProtectedPath("C:\\temp\\file.txt");

## Extensions

### Regex Extensions
Extensions for the `System.Text.RegularExpressions.Regex` class are provided to simplify the use of regular expressions. The following extensions are included:

#### ReplaceGroups
This extension allows you to replace the groups in a regular expression with a value. The following example demonstrates how to replace `Hello, World!` with `redacted, redacted!`:
var input = "Hello, World!";
var replacement = "redacted";
var regex = new Regex(@"(Hello), (World)!");
var result = regex.ReplaceGroups(input, replacement);
This is useful for sanitizing logs or other data. Here is an example of regex used to redact passwords in JSON:
[GeneratedRegex(@"""?(?:password|pwd)""?.*?""((?:[^""\\]|\\.)+)""", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
private static partial Regex JsonPasswordProperty();

### Null Checking Extensions
These extensions are used to check for null or empty values in objects and throw an exception if they are invalid. The following extensions are included:

- `ThrowIfNull`: Throws an exception if the object is null. 
- `ThrowIfAnyNull`: Throws an exception if any of the objects in the `IEnumerable` are null.
- `ThrowIfNullOrWhiteSpace` : Throws an exception if the string is null or whitespace.
- `ThrowIfAnyNullOrWhiteSpace` : Throws an exception if any of the strings in the `IEnumerable` are null or whitespace.
- `ThrowIfNullOrEmpty` : Throws an exception if the string is null or empty.
- `ThrowIfAnyNullOrEmpty` : Throws an exception if any of the strings in the `IEnumerable` are null or empty.

These extensions are useful for validating input parameters in methods. For example:
public MyClass(IInterface interface)
{
	var i = interface.ThrowIfNull(nameof(interface));
}

### String Extensions For Encoding And Decoding
String extensions are provided for encoding and decoding different formats. Supported formats are:
#### Base64
var base64 = "SGVsbG8gV29ybGQ=";
var decoded = base64.Base64Decode();
var encoded = decoded.Base64Encode();

#### URL
var url = "Hello%20World";
var decoded = url.UrlDecode();
var encoded = decoded.UrlEncode();

### Byte Array Extension For Finding BOM (Byte Order Mark)
In some cases, when reading a file, the BOM (Byte Order Mark) is included in the byte array. This extension method allows you to find the BOM in the byte array and remove it.

This extension returns a `System.Text.Encoding` for the BOM. It can be invoked as follows:
var array = new byte[] { 0xEF, 0xBB, 0xBF }; // UTF-8 BOM
var encoding = array.GetByteOrderMarkEncoding();

Supported BOMs:
- UTF-8: 0xEF, 0xBB, 0xBF
- UTF-16 (Little Endian): 0xFF, 0xFE
- UTF-16 (Big Endian): 0xFE, 0xFF
- UTF-32 (Little Endian): 0xFF, 0xFE, 0x00, 0x00
- UTF-32 (Big Endian): 0x00, 0x00, 0xFE, 0xFF

UTF-8 will be returned if BOM is not found or not recognized.

## Converters

### Json Date / DateTime Converters
Extension of the `System.Text.Json.Serialization.JsonConverter` class to handle the conversion of dates and datetimes to and from JSON. The converters are used to serialize and deserialize dates and datetimes in the format `yyyy-MM-dd` and `yyyy-MM-ddTHH:mm:ss` respectively.

## Contributing
We welcome contributions! Please read our [contributing guidelines](../CONTRIBUTING.md) to get started.

## License
This project is licensed under the MIT License - see the [LICENSE](../LICENSE) file for details.
