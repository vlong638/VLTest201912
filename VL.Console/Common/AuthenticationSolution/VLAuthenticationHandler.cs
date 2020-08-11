using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace VL.Consolo_Core.AuthenticationSolution
{
    public class VLAuthenticationHandler : IAuthenticationHandler
    {
        public AuthenticationScheme Scheme { get; private set; }
        protected HttpContext Context { get; private set; }
        public const string AuthCookieName = "vlcookie";
        public const string ShemeName = "vlsheme";

        public Task InitializeAsync(AuthenticationScheme scheme, Microsoft.AspNetCore.Http.HttpContext context)
        {
            Scheme = scheme;
            Context = context;
            return Task.CompletedTask;
        }

        public async Task<AuthenticateResult> AuthenticateAsync()
        {
            var cookieValue = Context.Request.Cookies[AuthCookieName];
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
    }
}
