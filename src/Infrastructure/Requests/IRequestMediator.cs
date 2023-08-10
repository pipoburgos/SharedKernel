using SharedKernel.Domain.Requests;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Requests;

/// <summary>  </summary>
public interface IRequestMediator
{
    /// <summary>  </summary>
    Task ExecuteHandler(string commandSerialized, CancellationToken cancellationToken);

    /// <summary>  </summary>
    Task ExecuteHandler(string body, Request commandSerialized, Type commandHandler,
        CancellationToken cancellationToken);
}
