namespace SharedKernel.Application.Settings;

/// <summary> Used to retrieve configured <typeparamref name="TOptions"/> instances.. </summary>
/// <typeparam name="TOptions">The type of options being requested.</typeparam>
public interface IOptionsService<out TOptions> where TOptions : class, new()
{
    /// <summary> The default configured <typeparamref name="TOptions"/> instance. </summary>
    TOptions Value { get; }
}
