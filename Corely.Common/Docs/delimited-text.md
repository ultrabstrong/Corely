# Delimited Text

This utility provides functionality for reading and writing delimited text data such as CSV, TSV, or other custom-delimited formats. The most prominent feature of this utility is the ability to read data one character at a time, allowing for incremental processing of large data streams while maintaining low resource cost and resiliancy against interruptions.

## Features

- Incremental character-by-character reads with position tracking
  - Keeps memory usage low for large data streams
  - Provides resilience against interruptions
- Support for common delimiters (comma, tab, pipe, semicolon)
- Custom delimiter configuration
- Handling of escaped delimiters
- Support for multiple encodings (UTF-8, UTF-16, etc.)

## DelimitedTextProvider

The `DelimitedTextProvider` class implements `IDelimitedTextProvider` and provides robust reading and writing of delimited text data with support for various delimiters.
- `ReadAllRecords`: Reads all records from a delimited text stream
- `ReadNextRecord`: Reads the next record from a delimited text stream starting at a specific position
- `WriteAllRecords`: Writes multiple records to a delimited text stream
- `WriteRecord`: Writes a single record to a delimited text stream

The following constructors are available. Note that ILogger is from Microsoft.Extensions.Logging:
- `DelimitedTextProvider(ILogger<DelimitedTextProvider> logger)`
  - Defaults with CSV delimiters
- `DelimitedTextProvider(ILogger<DelimitedTextProvider> logger, TokenDelimiter delimiter)`
  - Uses the specified token delimiter
- `DelimitedTextProvider(ILogger<DelimitedTextProvider> logger, char tokenDelimiter, char tokenLiteral, string recordDelimiter)`
  - Uses the specified token, token literal, and record delimiters

## Usage

Add the following using directives to your code file:
```csharp
// To use DelimitedTextProvider
using Corely.Common.DelimitedText;
// To use null logger for testing
using Microsoft.Extensions.Logging.Abstractions;
```

### Reading One Record At A Time

This is the strongest feature of this utility and is preferred for large data streams
```csharp
var provider = new DelimitedTextProvider(NullLogger<DelimitedTextProvider>.Instance);
using (var stream = File.OpenRead("data.csv"))
{
    var record = new ReadRecordResult();
    while (record.HasMore)
    {
        record = provider.ReadNextRecord(stream, record.EndPosition);
        // Handle record
    }
}
```

#### ReadRecordResult

The `ReadRecordResult` class is returned from `ReadNextRecord` and contains the following properties:
- `Tokens`: Tokens of the record (i.e. record values)
- `StartPosition`: Start position of the record in the data stream
- `Length`: Length of the record in the data stream
- `HasMore`: Indicates if there are more records to read from the data stream
- `EndPosition`: End position of the record in the data stream

### Reading All Records At Once

This can be used for small data streams.
```csharp
var provider = new DelimitedTextProvider(NullLogger<DelimitedTextProvider>.Instance);
using (var stream = File.OpenRead("data.csv"))
{
    var records = provider.ReadAllRecords(stream);
    foreach (var record in records)
    {
        // Handle record
    }
}
```

### Writing One Record At A Time

```csharp
var provider = new DelimitedTextProvider(NullLogger<DelimitedTextProvider>.Instance);
var data = new string[][] {
    ["Record1 Field1", "Record1 Field2", "Record1 Field3"],
    ["Record2 Field1", "Record2 Field2", "Record2 Field3"],
    ["Record3 Field1", "Record3 Field2", "Record3 Field3"],
};
using (var stream = File.OpenWrite("data.csv"))
{
    foreach (var record in data)
    {
        provider.WriteRecord(record, stream);
    }
}
```

### Writing All Records At Once

```csharp
var provider = new DelimitedTextProvider(NullLogger<DelimitedTextProvider>.Instance);
var data = new string[][] {
    ["Record1 Field1", "Record1 Field2", "Record1 Field3"],
    ["Record2 Field1", "Record2 Field2", "Record2 Field3"],
    ["Record3 Field1", "Record3 Field2", "Record3 Field3"],
};
using (var stream = File.OpenWrite("data.csv"))
{
    provider.WriteAllRecords(data, stream);
}
```

## Advanced Usage

### Recovering From Read Interruptions

If a read operation is interrupted, the `ReadRecordResult` object can be used to resume reading from the last known position. This is useful for scenarios where a read operation is interrupted and needs to be resumed from the last known position.
```csharp
long lastRecordEndPosition = GetLastRecordEndPosition(); // Get the last successful record end position
var provider = new DelimitedTextProvider(NullLogger<DelimitedTextProvider>.Instance);
using (var stream = File.OpenRead("data.csv"))
{
    var record = new ReadRecordResult() { StartPosition = lastRecordEndPosition };
    while (record.HasMore)
    {
        record = provider.ReadNextRecord(stream, record.EndPosition);
        // Handle record
        // Save the end position of this record in case next record read is interrupted
    }
}
```