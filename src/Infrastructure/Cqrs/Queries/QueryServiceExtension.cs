using System;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Infrastructure.System;
using System.Reflection;
using SharedKernel.Application.Cqrs.Queries;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Cqrs.Queries.InMemory;
using SharedKernel.Infrastructure.Validators;

namespace SharedKernel.Infrastructure.Cqrs.Queries
{
    public static class QueryServiceExtension
    {
        public static IServiceCollection AddQueriesHandlers(this IServiceCollection services, Assembly assembly)
        {
            return services.AddFromAssembly(assembly, typeof(IQueryRequestHandler<,>));
        }

        public static IServiceCollection AddQueriesHandlers(this IServiceCollection services, Type type)
        {
            return services.AddFromAssembly(type.Assembly, typeof(IQueryRequestHandler<,>));
        }

        public static IServiceCollection AddInMemoryQueryBus(this IServiceCollection services)
        {
            return services
                .AddQueryBus()
                .AddTransient<IQueryBus, InMemoryQueryBus>();
        }

        private static IServiceCollection AddQueryBus(this IServiceCollection services)
        {
            return services
                .AddTransient<ExecuteMiddlewaresService>()
                .AddTransient(typeof(IEntityValidator<>), typeof(FluentValidator<>));
        }
    }
}
