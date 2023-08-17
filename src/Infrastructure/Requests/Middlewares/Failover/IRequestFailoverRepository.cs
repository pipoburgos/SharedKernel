using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Requests.Middlewares.Failover;

/// <summary>  </summary>
public interface IRequestFailoverRepository
{
    /// <summary>  </summary>
    Task Save(ErrorRequest request, CancellationToken cancellationToken);
}