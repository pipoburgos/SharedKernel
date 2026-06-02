using Microsoft.AspNetCore.Routing;

namespace SharedKernel.Api.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}

