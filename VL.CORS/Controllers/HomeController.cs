using Autobots.Infrastracture.Common.ControllerSolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchAPI.CORS.Common;
using ResearchAPI.CORS.Services;

namespace ResearchAPI.CORS.Controllers
{
    /// <summary>
    /// 账户权限接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class HomeController : APIBaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public APIResult<string> Login([FromServices] APIContext apiContext, [FromServices] AccountService service, [FromBody] LoginRequest request)
        {
            var result = service.PasswordSignIn(request.UserName, request.Password);
            if (result.IsSuccess)
            {
                var token = apiContext.SetCurrentUser(new CurrentUser(result.Data)).ToMD5();
                return new APIResult<string>(token, result.Messages);
            }
            return new APIResult<string>(null, result.Messages);
        }
    }
}
