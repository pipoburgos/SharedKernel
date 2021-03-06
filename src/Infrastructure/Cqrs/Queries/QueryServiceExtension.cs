﻿using System;
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
    /// <summary>
    /// 
    /// </summary>
    public static class QueryServiceExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="infrastructureAssembly"></param>
        /// <returns></returns>
        public static IServiceCollection AddQueriesHandlers(this IServiceCollection services, Assembly infrastructureAssembly)
        {
            return services.AddFromAssembly(infrastructureAssembly, typeof(IQueryRequestHandler<,>));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="queryHandlerType"></param>
        /// <returns></returns>
        public static IServiceCollection AddQueriesHandlers(this IServiceCollection services, Type queryHandlerType)
        {
            return services.AddFromAssembly(queryHandlerType.Assembly, typeof(IQueryRequestHandler<,>));
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
                .AddTransient<ExecuteMiddlewaresService>()
                .AddTransient(typeof(IEntityValidator<>), typeof(FluentValidator<>));
        }
    }
}
