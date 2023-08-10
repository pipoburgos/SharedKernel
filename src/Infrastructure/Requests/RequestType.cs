using System;

namespace SharedKernel.Infrastructure.Requests;


/// <summary>  </summary>
public class RequestType : IRequestType
{
    /// <summary>  </summary>
    public RequestType(string uniqueName, Type type)
    {
        UniqueName = uniqueName;
        Type = type;
    }

    /// <summary>  </summary>
    public string UniqueName { get; }

    /// <summary>  </summary>
    public Type Type { get; }
}
