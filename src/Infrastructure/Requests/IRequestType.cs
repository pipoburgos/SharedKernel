using System;

namespace SharedKernel.Infrastructure.Requests;

/// <summary>  </summary>
public interface IRequestType
{
    /// <summary>  </summary>
    string UniqueName { get; }

    /// <summary>  </summary>
    Type Type { get; }
}
