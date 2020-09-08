﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VL.Consolo_Core.AuthenticationSolution
{
    public class VLAuthenticationHandler : IAuthenticationHandler, IAuthenticationSignInHandler, IAuthenticationSignOutHandler
    {
        public AuthenticationScheme Scheme { get; private set; }
        protected HttpContext Context { get; private set; }

        public const string Cookie_AuthName = "vlcookie";
        public const string ShemeName = "vlsheme";

        public Task InitializeAsync(AuthenticationScheme scheme, Microsoft.AspNetCore.Http.HttpContext context)
        {
            Scheme = scheme;
            Context = context;
            return Task.CompletedTask;
        }

        public async Task<AuthenticateResult> AuthenticateAsync()
        {
            var cookieValue = Context.Request.Cookies[Cookie_AuthName];
            if (string.IsNullOrEmpty(cookieValue))
            {
                return AuthenticateResult.NoResult();
            }
            return AuthenticateResult.Success(VLAuthenticationTicketHelper.Decrypt(cookieValue));
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            Context.Response.Redirect("/Home/Login");
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            Context.Response.StatusCode = 403;
            return Task.CompletedTask;
        }

        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            var ticket = new AuthenticationTicket(user, properties, Scheme.Name);
            Context.Response.Cookies.Append(Cookie_AuthName, VLAuthenticationTicketHelper.Encrypt(ticket));
            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            Context.Response.Cookies.Delete(Cookie_AuthName);
            return Task.CompletedTask;
        }
    }
}
