using System;

namespace SharedKernel.Infrastructure.Requests;

/// <summary>  </summary>
public class RequestType : IRequestType
{
    /// <summary>  </summary>
    public RequestType(string uniqueName, Type type, bool isTopic)
    {
        UniqueName = uniqueName;
        Type = type;
        IsTopic = isTopic;
    }

    /// <summary>  </summary>
    public string UniqueName { get; }

    /// <summary>  </summary>
    public Type Type { get; }

    /// <summary>  </summary>
    public bool IsTopic { get; }
}
