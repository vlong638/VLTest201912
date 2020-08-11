using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace VL.Consolo_Core.AuthenticationSolution
{
    public class VLAuthenticationTicketHelper
    {
        public static string Encrypt(AuthenticationTicket ticket)
        {
            return "testCookie";
        }
        public static AuthenticationTicket Decrypt(string ticketStr)
        {
            var scheme = "";
            var claims = new System.Collections.Generic.List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "MyUser"));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, "MyUserID"));
            claims.Add(new Claim(ClaimTypes.Role, "MyRole"));
            var claimIdentity = new ClaimsIdentity("简单声明 IsAuthenticated 将为 false");
            claimIdentity.AddClaim(new Claim(ClaimTypes.Name, "jim"));
            var claimsprincipal = new ClaimsPrincipal(claimIdentity);
            return new AuthenticationTicket(claimsprincipal, scheme);
        }
    }
}
