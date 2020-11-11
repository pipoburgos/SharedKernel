namespace SharedKernel.Application.Adapter
{
    /// <summary>
    /// Base contract for adapter factory
    /// </summary>
    public interface ITypeAdapterFactory
    {
        ITypeAdapter Create();
    }
}
