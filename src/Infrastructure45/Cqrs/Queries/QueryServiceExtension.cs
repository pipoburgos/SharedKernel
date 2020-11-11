using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.System;
using System.Reflection;

namespace SharedKernel.Infrastructure.Cqrs.Queries
{
    public static class QueryServiceExtension
    {
        public static IServiceCollection AddQueriesHandlers(this IServiceCollection services, Assembly assembly)
        {
            return services.AddFromAssembly(assembly, typeof(IQueryRequestHandler<,>));
        }
    }
}
