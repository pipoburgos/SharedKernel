using Microsoft.AspNetCore.Routing;

namespace SharedKernel.Api.Endpoints;

public interface IEndpoint
{
    void Map(IEndpointRouteBuilder group);
}

// ReSharper disable once UnusedTypeParameter
#pragma warning disable S2326
public interface IEndpoint<TGroup> : IEndpoint where TGroup : IEndpointGroup;
#pragma warning restore S2326
