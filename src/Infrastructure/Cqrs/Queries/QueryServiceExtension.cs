using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Queries;
using SharedKernel.Application.Validator;
using SharedKernel.Infrastructure.Cqrs.Middlewares;
using SharedKernel.Infrastructure.Cqrs.Queries.InMemory;
using SharedKernel.Infrastructure.System;
using SharedKernel.Infrastructure.Validators;
using System;
using System.Reflection;

namespace SharedKernel.Infrastructure.Cqrs.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public static class QueryServiceExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection AddQueriesHandlers(this IServiceCollection services,
            Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            services.AddFromAssembly(assembly, serviceLifetime, typeof(IQueryRequestHandler<,>));
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IServiceCollection AddQueriesHandlers(this IServiceCollection services, Type type,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            services.AddFromAssembly(type.Assembly, serviceLifetime, typeof(IQueryRequestHandler<,>));

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="infrastructureAssembly"></param>
        /// <returns></returns>
        public static IServiceCollection AddQueriesHandlers(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient, params Assembly[] infrastructureAssembly)
        {
            foreach (var assembly in infrastructureAssembly)
                services.AddFromAssembly(assembly, serviceLifetime, typeof(IQueryRequestHandler<,>));

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="queryHandlerTypes"></param>
        /// <returns></returns>
        public static IServiceCollection AddQueriesHandlers(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Transient, params Type[] queryHandlerTypes)
        {
            foreach (var queryHandlerType in queryHandlerTypes)
                services.AddFromAssembly(queryHandlerType.Assembly, serviceLifetime, typeof(IQueryRequestHandler<,>));

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddInMemoryQueryBus(this IServiceCollection services)
        {
            return services
                .AddQueryBus()
                .AddTransient<IQueryBus, InMemoryQueryBus>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddQueryBus(this IServiceCollection services)
        {
            return services
                .AddTransient<IExecuteMiddlewaresService, ExecuteMiddlewaresService>()
                .AddTransient(typeof(IEntityValidator<>), typeof(FluentValidator<>));
        }
    }
}
