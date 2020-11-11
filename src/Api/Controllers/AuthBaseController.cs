using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Cqrs.Commands;
using SharedKernel.Application.Cqrs.Queries;

namespace SharedKernel.Api.Controllers
{
    /// <summary>
    /// Para herencia
    /// </summary>
    [ApiController, Route("api/[controller]"), Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class AuthBaseController : ControllerBase
    {
        /// <summary>
        /// Ejecutar comandos
        /// </summary>
        protected ICommandBus CommandBus => HttpContext.RequestServices.GetService<ICommandBus>();

        /// <summary>
        /// Ejecutar consultas
        /// </summary>
        protected IQueryBus QueryBus => HttpContext.RequestServices.GetService<IQueryBus>();
    }
}
