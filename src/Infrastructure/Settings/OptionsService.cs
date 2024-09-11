using Microsoft.Extensions.Options;
using SharedKernel.Application.Settings;

namespace SharedKernel.Infrastructure.Settings;

/// <summary> . </summary>
public class OptionsService<TOptions> : IOptionsService<TOptions> where TOptions : class, new()
{
    private readonly IOptions<TOptions> _options;

    /// <summary> . </summary>
    public OptionsService(IOptions<TOptions> options)
    {
        _options = options;
    }

    /// <summary> . </summary>
    public TOptions Value => _options.Value;
}
