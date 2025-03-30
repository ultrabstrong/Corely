# File Paths

The `IFilePathProvider` interface is used for performing common filepath-related operations.

## Features

- Check if a file exists
- Get a path with a file name that is protected from overwriting
- Get the file name with or without extension
- `FilePathProvider` implementation included for local file paths

## Usage

Get an overwrite protected path for a local file:
```csharp
var provider = new FilePathProvider();
var path = provider.GetOverwriteProtectedPath("C:\\temp\\file.txt");
```

### Methods
- `DoesFileExist` : Checks if a file exists
- `GetOverwriteProtectedPath` : Returns a path with a file name that is protected from overwriting
- `GetFileNameWithExtension` : Returns the file name with extension
- `GetFileNameWithoutExtension` : Returns the file name without extension

