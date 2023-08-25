namespace SharedKernel.Infrastructure.Requests;

/// <summary>  </summary>
public interface IRequestSerializer
{
    /// <summary>  </summary>
    string Serialize(Request domainEvent);
}
