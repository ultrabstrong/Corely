# Dispose Base

`DisposeBase` is an abstract class that implements `IDisposable` and `IAsyncDisposable`. It provides public `Dispose` and `DisposeAsync` methods and has protected abstractions for disposing managed and unmanaged resources.

## Features

- Implements the dispose pattern
- Supports both synchronous and asynchronous disposal
- Provides structured methods for disposing different resource types

## Usage

Example Usage:

```charp
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
    protected async override ValueTask DisposeAsyncCore()
    {
        // Dispose managed resources asynchronously
    }
}
```