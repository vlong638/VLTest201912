using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Security.Claims;

namespace VL.Research.Common
{
    public class VLAuthenticationTicketHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public static string Encrypt(AuthenticationTicket ticket)
        {
            return ticket.Principal.Claims.First()?.Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static AuthenticationTicket Decrypt(string sessionId)
        {
            var scheme = VLAuthenticationHandler.ShemeName;
            var claimIdentity = new ClaimsIdentity("简单声明 IsAuthenticated 将为 false");
            claimIdentity.AddClaim(new Claim(ClaimTypes.Name, sessionId));
            var claimsprincipal = new ClaimsPrincipal(claimIdentity);
            return new AuthenticationTicket(claimsprincipal, scheme);
        }
    }
}
