using System;

namespace SharedKernel.Domain.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class Disposable : IDisposable
    {
        private bool _disposed;

        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free other state (managed objects).
            }
            // Free your own state (unmanaged objects).
            // Set large fields to null.
            _disposed = true;
        }

        // Use C# destructor syntax for finalization code.
        ~Disposable()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }
    }
}