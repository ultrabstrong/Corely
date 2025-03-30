# String Extensions

String extensions are provided for encoding and decoding different formats.

## Base64

Extensions for encoding and decoding strings to and from Base64 format.
```csharp
var helloWorld = "Hello World";
var encoded = helloWorld.Base64Encode();
var decoded = encoded.Base64Decode();
```

## URL

Extensions for encoding and decoding strings to and from URL format.
```csharp
var helloWorld = "Hello World";
var encoded = helloWorld.UrlEncode();
var decoded = encoded.UrlDecode();
```
