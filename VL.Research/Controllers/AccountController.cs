using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VL.Research.Models;
using VL.Research.Services;

namespace VL.Research.Controllers
{
    /// <summary>
    /// 登录状态枚举
    /// </summary>
    public enum SignInStatus
    {
        /// <summary>
        /// 
        /// </summary>
        Success = 0,
        /// <summary>
        /// 
        /// </summary>
        LockedOut = 1,
        /// <summary>
        /// 
        /// </summary>
        RequiresVerification = 2,
        /// <summary>
        /// 
        /// </summary>
        Failure = 3
    }

    /// <summary>
    /// 
    /// </summary>
    public class AccountController : BaseController
    {

        /// <summary>
        /// 
        /// </summary>
        public AccountController()
        {
        }
    }
}
