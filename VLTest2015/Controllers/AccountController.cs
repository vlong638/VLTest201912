﻿using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using VLTest2015.Attributes;
using VLTest2015.Authentication;
using VLTest2015.Models;
using VLTest2015.Services;

namespace VLTest2015.Controllers
{
    public class AccountController : BaseController
    {

        public AccountController()
        {
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
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

                SetCurrentUser(new CurrentUser()
                {
                    UserId = user.Id,
                    UserName = user.Name,
                    AuthorityIds = authorityIds.ToList(),
                });

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

        [AllowAnonymous]
        public ActionResult OnError(string returnMessage)
        {
            ViewBag.Message = returnMessage;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Sample()
        {
            return View();
        }

        [HttpGet]
        [VLAuthentication(Authority.查看用户列表)]
        public ActionResult AccountList()
        {
            return View();
        }

        [HttpPost]
        [VLAuthentication(Authority.查看用户列表)]
        public JsonResult GetUserPagedList(GetUserPageListRequest request)
        {
            var users = UserService.GetUserPageList(request).Data;
            var userRoles = UserService.GetRoleInfoByUserIds(users.Data.Select(c => c.Id).ToArray());
            IEnumerable<GetUserPagedListResponse> result = users.Data.Select(c => new GetUserPagedListResponse()
            {
                UserId = c.Id,
                UserName = c.Name,
                RoleNames = userRoles.Data.Where(p => p.UserId == c.Id).Select(p => p.RoleName)
            });
            return Json(new { total = users.TotalCount, rows = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [VLAuthentication(Authority.查看角色列表)]
        public ActionResult RoleList()
        {
            return View();
        }

        [HttpPost]
        [VLAuthentication(Authority.查看角色列表)]
        public JsonResult GetRoleList()
        {
            var roles = UserService.GetAllRoles().Data.Select(c => new IdNameResponse() { Id = c.Id, Name = c.Name });
            return Json(new { total = roles.Count(), rows = roles }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [VLAuthentication(Authority.创建角色)]
        public JsonResult AddRole(string roleName)
        {
            var result = UserService.CreateRole(roleName);
            return Json(new { errorMsg = string.Join(",", result.Messages) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [VLAuthentication(Authority.编辑用户角色)]
        public JsonResult EditUserRole(long userId, long[] roleIds)
        {
            var result = UserService.EditUserRoles(userId, roleIds);
            return Json(new { errorMsg = string.Join(",", result.Messages) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [VLAuthentication(Authority.编辑用户角色)]
        public JsonResult GetUserRoleListByUser(GetUserRoleListByUserRequest request)
        {
            if (request.UserId <= 0)
            {
                return Json("需选中用户");
            }

            var roles = UserService.GetAllRoles().Data;
            var userRoles = UserService.GetRoleInfoByUserIds(request.UserId).Data;
            IEnumerable<CheckableIdNameResponse> result = roles.Select(c => new CheckableIdNameResponse()
            {
                Id = c.Id,
                Name = c.Name,
                IsChecked = userRoles.FirstOrDefault(d => d.RoleId == c.Id) != null
            });
            return Json(new { total = roles.Count(), rows = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [VLAuthentication(Authority.编辑角色权限)]
        public JsonResult GetRoleAuthorities(long roleId)
        {
            if (roleId <= 0)
            {
                return Json("需选中角色");
            }

            var authorities = EnumHelper.GetAllEnums<Authority>();
            var roleAuthorities = UserService.GetRoleAuthorityIds(roleId).Data;
            var result = authorities.Select(c => new CheckableIdNameResponse() { Id = (long)c, Name = c.GetDescription(), IsChecked = roleAuthorities.ToList().Contains((long)c) });
            return Json(new { total = authorities.Count(), rows = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [VLAuthentication(Authority.编辑角色权限)]
        public JsonResult EditRoleAuthority(long roleId, long[] authorityIds)
        {
            var result = UserService.EditRoleAuthorities(roleId, authorityIds);
            return Json(new { errorMsg = string.Join(",", result.Messages) }, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = UserService.Register(model.UserName, model.Password);
                if (result.IsSuccess)
                {
                    var user = UserService.PasswordSignIn(model.UserName, model.Password, false).Data;
                    var authorityIds = UserService.GetAllUserAuthorityIds(user.Id).Data;

                    #region 邮箱二次确认
                    // 有关如何启用帐户确认和密码重置的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=320771
                    // 发送包含此链接的电子邮件
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "确认你的帐户", "请通过单击 <a href=\"" + callbackUrl + "\">這裏</a>来确认你的帐户"); 
                    #endregion

                    #region 登录缓存处理

                    SetCurrentUser(new CurrentUser()
                    {
                        UserId = user.Id,
                        UserName = user.Name,
                        AuthorityIds = authorityIds.ToList(),
                    });

                    #endregion

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result.Messages);
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }
    }
}


