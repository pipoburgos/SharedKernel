using Asp.Versioning;

namespace BankAccounts.Api;

public static class ApiGroup
{
    private static IEndpointRouteBuilder? _routeGroupBuilder;

    public static IEndpointRouteBuilder MapApiGroup(this IEndpointRouteBuilder app)
    {
        if (_routeGroupBuilder != null)
            return _routeGroupBuilder;

        var apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .HasApiVersion(new ApiVersion(2, 0))
            .ReportApiVersions()
            .Build();

        _routeGroupBuilder = app.MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);

        return _routeGroupBuilder;
    }
}
