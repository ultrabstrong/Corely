# Byte Array Extensions

## Finding BOM (Byte Order Mark)

In some cases, when reading a file, the BOM (Byte Order Mark) is prepended to the contents. This extension method returns the `System.Text.Encoding` represented by the BOM.

Example:
```csharp
var array = new byte[] { 0xEF, 0xBB, 0xBF }; // UTF-8 BOM
var encoding = array.GetByteOrderMarkEncoding();
```

See the [Delimited Text](../delimited-text.md) docs for an example of how this extension can be used.

### Supported BOMs:
- UTF-8: 0xEF, 0xBB, 0xBF
- UTF-16 (Little Endian): 0xFF, 0xFE
- UTF-16 (Big Endian): 0xFE, 0xFF
- UTF-32 (Little Endian): 0xFF, 0xFE, 0x00, 0x00
- UTF-32 (Big Endian): 0x00, 0x00, 0xFE, 0xFF

UTF-8 is returned if BOM is not found or not recognized.
