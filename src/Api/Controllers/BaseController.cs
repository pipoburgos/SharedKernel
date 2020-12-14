using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Queries;

namespace SharedKernel.Api.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    [ApiController, Produces("application/json")]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Gets the command bus
        /// </summary>
        protected ICommandBus CommandBus => HttpContext.RequestServices.GetRequiredService<ICommandBus>();

        /// <summary>
        /// Gets de query bus
        /// </summary>
        protected IQueryBus QueryBus => HttpContext.RequestServices.GetRequiredService<IQueryBus>();
    }
}
