using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VL.Consolo_Core.AuthenticationSolution;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.ServiceSolution;
using VL.Consolo_Core.Common.ValuesSolution;
using VL.Research.Common;
using VL.Research.Models;
using VL.Research.Repositories;
using VLTest2015.Common.MD5Solution;

namespace VL.Research.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class UserService : BaseService, IUserService
    {
        APIContext dbContext { set; get; }
        public IAuthenticationSchemeProvider Schemes { get; }
        public IAuthenticationHandlerProvider Handlers { get; }
        public IClaimsTransformation Transform { get; }

        UserRepository userRepository { set; get; }
        UserAuthorityRepository userAuthorityRepository { set; get; }
        UserRoleRepository userRoleRepository { set; get; }
        RoleRepository roleRepository { set; get; }
        RoleAuthorityRepository roleAuthorityRepository { set; get; }
        UserMenuRepository userMenuRepository { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public UserService(APIContext dbContext
                , IAuthenticationSchemeProvider Schemes
                , IAuthenticationHandlerProvider Handlers
                , IClaimsTransformation Transform)
        {
            this.dbContext = dbContext;
            this.Schemes = Schemes;
            this.Handlers = Handlers;
            this.Transform = Transform;
            userRepository = new UserRepository(dbContext.CommonDbContext);
            userAuthorityRepository = new UserAuthorityRepository(dbContext.CommonDbContext);
            userRoleRepository = new UserRoleRepository(dbContext.CommonDbContext);
            roleRepository = new RoleRepository(dbContext.CommonDbContext);
            roleAuthorityRepository = new RoleAuthorityRepository(dbContext.CommonDbContext);
            userMenuRepository = new UserMenuRepository(dbContext.CommonDbContext);
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ServiceResult<User> Register(string userName, string password)
        {
            var hashPassword = MD5Helper.GetHashValue(password);
            User user = new User()
            {
                Name = userName,
                Password = hashPassword,
            };
            var result = userRepository.GetBy(user.Name);
            if (result != null)
            {
                return Error<User>("用户名已存在");
            }
            user.Id = userRepository.Insert(user);
            return Success(user);
        }

        /// <summary>
        /// 密码登录
        /// </summary>
        public ServiceResult<User> PasswordSignIn(APIContext apiContext, string userName, string password, bool shouldLockout)
        {
            var hashPassword = MD5Helper.GetHashValue(password);
            User user = new User()
            {
                Name = userName,
                Password = hashPassword,
            };
            var result = userRepository.GetBy(user.Name, user.Password);
            if (result == null)
            {
                return Error<User>("用户名不存在或密码不匹配");
            }

            var claims = new List<Claim>();
            var sessionId = user.Name.GetHashCode().ToString();
            claims.Add(new Claim(ClaimTypes.Name, sessionId));
            var claimsIdentity = new ClaimsIdentity(claims, "CustomApiKeyAuth");
            var userPrincipal = new ClaimsPrincipal(claimsIdentity);
            apiContext.HttpContext.SignInAsync(SchemeName, userPrincipal, new AuthenticationProperties());

            var authorities = GetAllUserAuthorityIds(result.Id).Data.Select(c=>c.ToEnum<Authority>()).ToList();
            apiContext.RedisCache.Set(sessionId, authorities, DateTime.Now.AddMinutes(5));

            return Success(result);
        }

        /// <summary>
        /// 角色权限编辑
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="authorityIds"></param>
        /// <returns></returns>
        internal ServiceResult<bool> EditRoleAuthorities(long roleId, List<long>  authorityIds)
        {
            return dbContext.CommonDbContext.DelegateTransaction((g) =>
            {
                roleAuthorityRepository.DeleteBy(roleId);
                var roleAuthorities = authorityIds.Select(c => new RoleAuthority() { RoleId = roleId, AuthorityId = c }).ToArray();
                foreach (var roleAuthority in roleAuthorities)
                {
                    roleAuthority.Id = roleAuthorityRepository.Insert(roleAuthority);
                }
                return true;
            });
        }

        /// <summary>
        /// 用户角色编辑
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public ServiceResult<bool> EditUserRoles(long userId, IEnumerable<long> roleIds)
        {
            return dbContext.CommonDbContext.DelegateTransaction((g) =>
            {
                userRoleRepository.DeleteBy(userId);
                var userRoles = roleIds.Select(c => new UserRole() { UserId = userId, RoleId = c }).ToArray();
                foreach (var userRole in userRoles)
                {
                    userRole.Id = userRoleRepository.Insert(userRole);
                }
                return true;
            });
        }

        //public ServiceResult<bool> EditUserAuthorities(long userId, IEnumerable<long> authorityIds)
        //{
        //    return DelegateTransaction(() =>
        //    {
        //        userAuthorityRepository.DeleteBy(userId);
        //        var userAuthorities = authorityIds.Select(c => new UserAuthority() { UserId = userId, AuthorityId = c }).ToArray();
        //        foreach (var userAuthority in userAuthorities)
        //        {
        //            userAuthority.AuthorityId = userAuthorityRepository.Insert(userAuthority);
        //        }
        //        return true;
        //    });
        //}

        public ServiceResult<IEnumerable<long>> GetAllUserAuthorityIds(long userId)
        {
            var userAuthorities = userAuthorityRepository.GetBy(userId);
            var userRoles = userRoleRepository.GetBy(userId);
            var roleAuthorities = userRoles.Count() == 0 ? new List<RoleAuthority>() : roleAuthorityRepository.GetBy(userRoles.Select(c => c.RoleId).ToArray());
            var allAuthorityIds = userAuthorities.Select(c => c.AuthorityId).Union(roleAuthorities.Select(c => c.AuthorityId)).Distinct();
            return Success(allAuthorityIds);
        }

        //public ServiceResult<PagerResponse<User>> GetUserPageList(GetUserPageListRequest request)
        //{
        //    PagerResponse<User> result = new PagerResponse<User>();
        //    result.TotalCount = userRepository.GetUserPageListCount(request);
        //    result.Data = userRepository.GetUserPageListData(request);
        //    return Success(result);
        //}

        public ServiceResult<IEnumerable<UserRoleInfoModel>> GetRoleInfoByUserIds(params long[] userIds)
        {
            var result = roleRepository.GetUserRoleInfosBy(userIds);
            return Success(result);
        }

        public ServiceResult<long> CreateRole(string roleName)
        {
            Role role = new Role()
            {
                Name = roleName,
            };
            if (string.IsNullOrEmpty(roleName))
            {
                return Error<long>("角色名称不可为空");
            }
            var result = roleRepository.GetBy(role.Name);
            if (result != null)
            {
                return Error<long>("角色名称已存在");
            }
            var id = roleRepository.Insert(role);
            return Success(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResult<Role> GetRole(long id)
        {
            Role role = roleRepository.GetById(id);
            if (role == null)
            {
                return Error<Role>("无效的角色Id");
            }
            return Success(role);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public ServiceResult<bool> EditRole(long id , string roleName)
        {
            Role role = roleRepository.GetById(id);
            if (role==null)
            {
                return Error<bool>("无效的角色Id");
            }
            if (string.IsNullOrEmpty(roleName))
            {
                return Error<bool>("角色名称不可为空");
            }
            var repeat = roleRepository.GetBy(roleName);
            if (repeat != null)
            {
                return Error<bool>("角色名称已存在");
            }
            role.Name = roleName;
            var result = roleRepository.Update(role);
            return Success(result);
        }

        public ServiceResult<IEnumerable<long>> GetRoleAuthorityIds(long roleId)
        {
            var roleAuthorities = roleAuthorityRepository.GetBy(roleId);
            return Success(roleAuthorities.Select(c => c.AuthorityId));
        }

        public ServiceResult<IEnumerable<Role>> GetAllRoles()
        {
            var roles = roleRepository.GetAll();
            return Success(roles);
        }

        public ServiceResult<bool> UpdateUserMenu(UserMenu userMenu)
        {
            var isSuccess = userMenuRepository.Update(userMenu);
            return Success(isSuccess);
        }

        public ServiceResult<long> CreateUserMenu(UserMenu userMenu)
        {
            var id = userMenuRepository.Insert(userMenu);
            return Success(id);
        }

        public ServiceResult<List<UserMenu>> GetUserMenus(long userId)
        {
            var userMenus = userMenuRepository.GetByUserId(userId);
            userMenus.ForEach(c => c.URL = string.Format(c.URL, c.Id));
            return Success(userMenus);
        }

        public ServiceResult<UserMenu> GetUserMenuById(long customConfigId)
        {
            var userMenu = userMenuRepository.GetById(customConfigId);
            return Success(userMenu);
        }

        #region IAuthenticationService

        /// <summary>
        /// 
        /// </summary>
        public const string AuthCookieName = VLAuthenticationHandler.Cookie_AuthName;
        /// <summary>
        /// 
        /// </summary>
        public const string SchemeName = VLAuthenticationHandler.ShemeName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <returns></returns>
        public async Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
        {
            var cookieValue = context.Request.Cookies[AuthCookieName];
            if (string.IsNullOrEmpty(cookieValue))
            {
                return AuthenticateResult.NoResult();
            }
            return AuthenticateResult.Success(VLAuthenticationTicketHelper.Decrypt(cookieValue));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            context.Response.Redirect("/Home/Login");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            context.Response.StatusCode = 403;
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="principal"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties) =>
            context.RequestServices.GetRequiredService<IAuthenticationService>().SignInAsync(context, scheme, principal, properties);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="scheme"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            context.Response.Cookies.Delete(AuthCookieName);
            context.Response.Redirect("/Home/Login");
            return Task.CompletedTask;
        }

        #endregion
    }

    ///// <summary>
    ///// HttpContext 扩展
    ///// </summary>
    //public static class AuthenticationHttpContextExtensions
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="scheme"></param>
    //    /// <returns></returns>
    //    public static Task<AuthenticateResult> AuthenticateAsync(this HttpContext context, string scheme) =>
    //        context.RequestServices.GetRequiredService<IAuthenticationService>().AuthenticateAsync(context, scheme);

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="scheme"></param>
    //    /// <param name="properties"></param>
    //    /// <returns></returns>
    //    public static Task ChallengeAsync(this HttpContext context, string scheme, AuthenticationProperties properties) =>
    //        context.RequestServices.GetRequiredService<IAuthenticationService>().ChallengeAsync(context, scheme, properties);

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="scheme"></param>
    //    /// <param name="properties"></param>
    //    /// <returns></returns>
    //    public static Task ForbidAsync(this HttpContext context, string scheme, AuthenticationProperties properties) =>
    //        context.RequestServices.GetRequiredService<IAuthenticationService>().ForbidAsync(context, scheme, properties);

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="scheme"></param>
    //    /// <param name="principal"></param>
    //    /// <param name="properties"></param>
    //    /// <returns></returns>
    //    public static Task SignInAsync(this HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties) =>
    //        context.RequestServices.GetRequiredService<IAuthenticationService>().SignInAsync(context, scheme, principal, properties);

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="scheme"></param>
    //    /// <param name="properties"></param>
    //    /// <returns></returns>
    //    public static Task SignOutAsync(this HttpContext context, string scheme, AuthenticationProperties properties) =>
    //        context.RequestServices.GetRequiredService<IAuthenticationService>().SignOutAsync(context, scheme, properties);

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="context"></param>
    //    /// <param name="scheme"></param>
    //    /// <param name="tokenName"></param>
    //    /// <returns></returns>
    //    public static Task<string> GetTokenAsync(this HttpContext context, string scheme, string tokenName) =>
    //        context.RequestServices.GetRequiredService<IAuthenticationService>().GetTokenAsync(context, scheme, tokenName);

    //}
}
