# Converters

## Json Date / DateTime Converters

Extension of the `System.Text.Json.Serialization.JsonConverter` class to handle the conversion of dates and datetimes to and from JSON.

### Features

- Serializes and deserializes `DateTime` values in consistent formats
- Supports both date-only and full datetime values

### Format Standards

- Date format: `yyyy-MM-dd`
- DateTime format: `yyyy-MM-ddTHH:mm:ss`

### Usage

Add a converter to the `JsonSerializerOptions` object:
```csharp
var options = new JsonSerializerOptions
{
    Converters = { new JsonDateConverter(), new JsonDateTimeConverter() }
};
```
Serialize and deserialize objects as needed:
```csharp
var json = JsonSerializer.Serialize(myObject, options);
var myObject = JsonSerializer.Deserialize<MyObject>(json, options);
```