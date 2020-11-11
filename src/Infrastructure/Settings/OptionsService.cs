using Microsoft.Extensions.Options;
using SharedKernel.Application.Settings;

namespace SharedKernel.Infrastructure.Settings
{
    public class OptionsService<TOptions> : IOptionsService<TOptions> where TOptions : class, new()
    {
        private readonly IOptions<TOptions> _options;

        public OptionsService(IOptions<TOptions> options)
        {
            _options = options;
        }

        public TOptions Value => _options.Value;
    }
}
