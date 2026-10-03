using Microsoft.AspNetCore.Routing;

namespace SharedKernel.Api.Endpoints;

public interface IEndpointGroup
{
    string Name { get; }

    string Prefix { get; }

    void Configure(RouteGroupBuilder group);
}