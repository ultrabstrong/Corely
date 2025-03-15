# Corely Common
This library contains common utilities to be used in other projects. The following utilities are included:

## Table of Contents
- [Installation](#installation)
- [Regex Extensions](#regex-extensions)
  - [ReplaceGroups](#replacegroups) 
- [Json Date / DateTime Converters](#json-date--datetime-converters)
- [Null Checking Extensions](#null-checking-extensions)
- [String Extensions For Encoding And Decoding](#string-extension-for-encoding-and-decoding)
  - [Base64](#base64)
  - [URL](#url)
- [Byte Array Extension For Finding BOM (Byte Order Mark)](#byte-array-extension-for-finding-bom-byte-order-mark)

## Installation

To install the library, use the following command:
`dotnet add package Corely.Common`

## Regex Extensions
Extensions for the `System.Text.RegularExpressions.Regex` class are provided to simplify the use of regular expressions. The following extensions are included:

#### ReplaceGroups
This extension allows you to replace the groups in a regular expression with a value. The following example demonstrates how to replace `Hello, World!` with `redacted, redacted!`:
```csharp
var input = "Hello, World!";
var replacement = "redacted";
var regex = new Regex(@"(Hello), (World)!");
var result = regex.ReplaceGroups(input, replacement);
```
This is useful for sanitizing logs or other data. Here is an example of regex used to redact passwords in JSON:
```csharp
[GeneratedRegex(@"""?(?:password|pwd)""?.*?""((?:[^""\\]|\\.)+)""", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
private static partial Regex JsonPasswordProperty();
```

## Json Date / DateTime Converters
Extension of the ``System.Text.Serialization.Json.JsonConverter`` class to handle the conversion of dates and datetimes to and from JSON. The converters are used to serialize and deserialize dates and datetimes in the format `yyyy-MM-dd` and `yyyy-MM-ddTHH:mm:ss` respectively.

## Null Checking Extensions
These extensions are used to check for null or empty values in objects and throw an exception if they are invalid. The following extensions are included:

- `ThrowIfNull`: Throws an exception if the object is null. 
- `ThrowIfAnyNull`: Throws an exception if any of the objects in the `IEnumerable` are null.
- `ThrowIfNullOrWhiteSpace` : Throws an exception if the string is null or whitespace.
- `ThrowIfAnyNullOrWhiteSpace` : Throws an exception if any of the strings in the `IEnumerable` are null or whitespace.
- `ThrowIfNullOrEmpty` : Throws an exception if the string is null or empty.
- `ThrowIfAnyNullOrEmpty` : Throws an exception if any of the strings in the `IEnumerable` are null or empty.

These extensions are useful for validating input parameters in methods. For example:
```csharp
public MyClass(IInterface interface)
{
	var i = interface.ThrowIfNull(nameof(interface));
}
```

## String Extensions For Encoding And Decoding
String extensions are provided for encoding and decoding different formats. Supported formats are:
#### Base64
```csharp
var base64 = "SGVsbG8gV29ybGQ=";
var decoded = base64.Base64Decode();
var encoded = decoded.Base64Encode();
```

#### URL
```csharp
var url = "Hello%20World";
var decoded = url.UrlDecode();
var encoded = decoded.UrlEncode();
```

## Byte Array Extension For Finding BOM (Byte Order Mark)
In some cases, when reading a file, the BOM (Byte Order Mark) is included in the byte array. This extension method allows you to find the BOM in the byte array and remove it.

This extension returns a `System.Text.Encoding` for the BOM. It can be invoked as follows:
```csharp
var array = new byte[] { 0xEF, 0xBB, 0xBF }; // UTF-8 BOM
var encoding = array.GetByteOrderMarkEncoding();
```

Supported BOMs:
- UTF-8: 0xEF, 0xBB, 0xBF
- UTF-16 (Little Endian): 0xFF, 0xFE
- UTF-16 (Big Endian): 0xFE, 0xFF
- UTF-32 (Little Endian): 0xFF, 0xFE, 0x00, 0x00
- UTF-32 (Big Endian): 0x00, 0x00, 0xFE, 0xFF

UTF-8 will be returned if BOM is not found or not recognized.