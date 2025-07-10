using SharedKernel.Domain.Exceptions;
using System.Globalization;

namespace SharedKernel.Domain.Requests;

/// <summary> Request. </summary>
public abstract class Request : IRequest
{
    #region Constructors

    /// <summary> Request serializable constructor. </summary>
    protected Request() : this(default)
    {
    }

    /// <summary> Request constructor. </summary>
    protected Request(string? requestId = default, string? occurredOn = default)
    {
        RequestId = requestId ?? Guid.NewGuid().ToString();
        OccurredOn = occurredOn ?? DateTime.UtcNow.ToString("s", CultureInfo.CurrentCulture);
    }

    #endregion

    #region Properties

    /// <summary> The request identifier. </summary>
    public string RequestId { get; }

    /// <summary> When the request occurred. </summary>
    public string OccurredOn { get; }

    #endregion

    #region Methods

    /// <summary> The request identifier for message queues. </summary>
    public abstract string GetUniqueName();

    /// <summary> Create a new request with default values. </summary>
    public abstract Request FromPrimitives(Dictionary<string, string> body, string id, string occurredOn);

    /// <summary> Cast enum from body. </summary>
    public TEnum GetEnumFromBody<TEnum>(Dictionary<string, string> body, string name) where TEnum : struct
    {
        if (string.IsNullOrWhiteSpace(body[name]))
            return default;

        if (Enum.TryParse<TEnum>(body[name], out var result))
            return result;

        throw new TextException($"Error parsing {body[name]}");
    }

    /// <summary> Cast Datetime from body. </summary>
    public DateTime ConvertToDateTime(Dictionary<string, string> body, string name)
    {
        return ConvertToDateTime(body[name]);
    }

    /// <summary> Cast Datetime from body. </summary>
    public DateTime ConvertToDateTime(string value)
    {
        return Convert.ToDateTime(value);
    }

    #endregion
}
