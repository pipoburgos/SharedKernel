using SharedKernel.Application.Reflection;
using SharedKernel.Domain.Requests;

namespace SharedKernel.Infrastructure.Requests;

/// <summary>  </summary>
internal static class RequestExtensions
{
    /// <summary>  </summary>
    public static Dictionary<string, string> ToPrimitives(this IRequest domainEvent)
    {
        var primitives = new Dictionary<string, string>();

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
