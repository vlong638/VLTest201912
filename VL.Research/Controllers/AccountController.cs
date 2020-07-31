using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VL.Research.Models;
using VL.Research.Services;

namespace VL.Research.Controllers
{
    public enum SignInStatus
    {
        Success = 0,
        LockedOut = 1,
        RequiresVerification = 2,
        Failure = 3
    }
    /// <summary>
    /// 
    /// </summary>
    public class AccountController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public AccountController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserService"></param>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login([FromServices] UserService UserService, LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 这不会计入到为执行帐户锁定而统计的登录失败次数中
            // 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
            var result = UserService.PasswordSignIn(model.UserName, model.Password, false);
            if (result.IsSuccess)
            {
                var user = result.Data;
                var authorityIds = UserService.GetAllUserAuthorityIds(result.Data.Id).Data;

                #region 登录缓存处理

                //SetCurrentUser(new CurrentUser()
                //{
                //    UserId = user.Id,
                //    UserName = user.Name,
                //    AuthorityIds = authorityIds.ToList(),
                //});

                #endregion

                return RedirectToLocal(returnUrl);
            }
            else
            {
                switch (result.Code)
                {
                    case (int)SignInStatus.LockedOut:
                        return View("Lockout");
                    case (int)SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case (int)SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", string.Join(",", result.Messages));
                        return View(model);
                }
            }
        }
    }
}
