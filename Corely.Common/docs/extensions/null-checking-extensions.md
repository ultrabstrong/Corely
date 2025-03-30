# Null Checking Extensions

These extensions provide a consise check for null or empty arguments and throw meaningful exceptions for invalid input.

## Available Extensions

- `ThrowIfNull`: Throws an exception if the object is null. 
- `ThrowIfAnyNull`: Throws an exception if any of the objects in the `IEnumerable` are null.
- `ThrowIfNullOrWhiteSpace`: Throws an exception if the string is null or whitespace.
- `ThrowIfAnyNullOrWhiteSpace`: Throws an exception if any of the strings in the `IEnumerable` are null or whitespace.
- `ThrowIfNullOrEmpty`: Throws an exception if the string is null or empty.
- `ThrowIfAnyNullOrEmpty`: Throws an exception if any of the strings in the `IEnumerable` are null or empty.

## Usage

Verify interface is not null:
```csharp
public MyClass(IInterface interface)
{
    var i = interface.ThrowIfNull(nameof(interface));
}
```