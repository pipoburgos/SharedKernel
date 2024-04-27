namespace SharedKernel.Infrastructure.Requests;

/// <summary>  </summary>
public interface IRequestMediator
{
    /// <summary>  </summary>
    bool HandlerImplemented(string requestSerialized, Type requestType);

    /// <summary>  </summary>
    Task Execute(string requestSerialized, Type requestType, string method, CancellationToken cancellationToken);
}
