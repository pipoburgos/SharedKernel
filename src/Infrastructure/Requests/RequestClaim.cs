namespace SharedKernel.Infrastructure.Requests;

/// <summary> Domain claim. </summary>
internal class RequestClaim
{
    /// <summary> Constructor. </summary>
    public RequestClaim(string type, string value)
    {
        Type = type;
        Value = value;
    }

    /// <summary> Claim type. </summary>
    public string Type { get; }

    /// <summary> Claim value. </summary>
    public string Value { get; }
}
