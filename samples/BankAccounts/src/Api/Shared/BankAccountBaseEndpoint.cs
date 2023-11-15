using SharedKernel.Api.Controllers;

namespace BankAccounts.Api.Shared;

/// <summary> Enpoint base. </summary>
[ApiController, Produces("application/json")]
public abstract class BankAccountBaseEndpoint : BaseController
{
}