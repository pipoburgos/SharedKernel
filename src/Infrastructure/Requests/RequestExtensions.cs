using SharedKernel.Application.Reflection;

namespace SharedKernel.Infrastructure.Requests;

/// <summary>  </summary>
internal static class RequestExtensions
{
    public const string Headers = "Headers";
    public const string Claims = "Claims";
    public const string Authorization = "Authorization";
    public const string Data = "Data";
    public const string Id = "Id";
    public const string Type = "Type";
    public const string OccurredOn = "OccurredOn";
    public const string Attributes = "Attributes";
    public const string Meta = "Meta";


    /// <summary>  </summary>
    public static Dictionary<string, string?> ToPrimitives(this IRequest domainEvent)
    {
        var primitives = new Dictionary<string, string?>();

        domainEvent
            .GetType()
            .GetProperties()
            .Where(p =>
                p.Name != nameof(Request.RequestId) &&
                p.Name != nameof(Request.OccurredOn))
            .ToList()
            .ForEach(p => primitives.Add(p.Name, p.GetStringValue(domainEvent.GetType(), domainEvent)));

        return primitives;
    }
}
