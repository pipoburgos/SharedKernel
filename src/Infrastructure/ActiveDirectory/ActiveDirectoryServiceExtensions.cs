using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.ActiveDirectory;

namespace SharedKernel.Infrastructure.ActiveDirectory
{
    public static class ActiveDirectoryServiceExtensions
    {
        public static IServiceCollection AddActiveDirectory(this IServiceCollection services)
        {
            return services
                .AddTransient<IActiveDirectoryService, ActiveDirectoryService>();
        }
    }
}
