namespace SharedKernel.Application.Adapter
{
    /// <summary>
    /// Base contract for adapter factory
    /// </summary>
    public interface ITypeAdapterFactory
    {
        /// <summary>
        /// Create a generic type adapter for mapping objects
        /// </summary>
        /// <returns></returns>
        ITypeAdapter Create();
    }
}
