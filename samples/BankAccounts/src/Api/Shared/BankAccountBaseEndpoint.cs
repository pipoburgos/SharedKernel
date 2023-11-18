using SharedKernel.Api.Endpoints;

namespace BankAccounts.Api.Shared;

/// <summary> Enpoint base. </summary>
[ApiController, Produces("application/json")]
public abstract class BankAccountBaseEndpoint : EndpointBase
{
}