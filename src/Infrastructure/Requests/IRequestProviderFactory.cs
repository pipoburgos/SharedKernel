using System;

namespace SharedKernel.Infrastructure.Requests;

/// <summary>  </summary>
internal interface IRequestProviderFactory
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="uniqueName"></param>
    /// <returns></returns>
    Type Get(string uniqueName);
}