using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace SharedKernel.Api.Controllers
{
    /// <summary>
    /// Auth base controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class AuthBaseController : BaseController { }
}
