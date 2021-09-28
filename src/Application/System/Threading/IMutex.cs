namespace SharedKernel.Application.System.Threading
{
    /// <summary>
    /// Object to release mutex
    /// </summary>
    public interface IMutex
    {
        /// <summary>
        /// Relase mutex
        /// </summary>
        void Release();
    }
}