namespace Corely.Common.Models
{
    public abstract class DisposeBase : IDisposable
    {
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    DisposeManagedResources();
                }
                DisposeUnmanagedResources();

                _disposed = true;
            }
        }

        ~DisposeBase()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected abstract void DisposeManagedResources();
        protected virtual void DisposeUnmanagedResources() { }
    }
}
