using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SharedKernel.Api.Endpoints;

public static class EndpointMappingExtensions
{
    public static WebApplication UseEndpoints(
        this WebApplication app,
        params Assembly[] assemblies)
    {
        var endpointTypes =
            assemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Select(t => new
                {
                    Type = t.AsType(),

                    GroupInterface = t.ImplementedInterfaces
                        .SingleOrDefault(i =>
                            i.IsGenericType &&
                            i.GetGenericTypeDefinition() ==
                            typeof(IEndpoint<>)),
                })
                .Where(x => x.GroupInterface is not null)
                .OrderBy(x => x.Type.FullName)
                .ToList();

        foreach (var endpointsByGroup in
                 endpointTypes.GroupBy(
                     x => x.GroupInterface!
                         .GetGenericArguments()[0]))
        {
            var groupType = endpointsByGroup.Key;

            if (!typeof(IEndpointGroup).IsAssignableFrom(groupType))
            {
                throw new InvalidOperationException(
                    $"El grupo '{groupType.FullName}' " +
                    $"debe implementar IEndpointGroup.");
            }

            var groupDefinition =
                (IEndpointGroup)app.Services.GetRequiredService(groupType)
                ?? throw new InvalidOperationException(
                    $"El grupo '{groupType.FullName}' no está registrado en DI.");

            var group =
                app.NewVersionedApi(groupDefinition.Name)
                    .MapGroup(groupDefinition.Prefix);

            groupDefinition.Configure(group);

            foreach (var endpoint in endpointsByGroup)
            {
                var instance =
                    (IEndpoint)app.Services.GetRequiredService(endpoint.Type)
                    ?? throw new InvalidOperationException(
                        $"El endpoint '{endpoint.Type.FullName}' no está registrado en DI.");

                instance.Map(group);
            }
        }

        return app;
    }
}