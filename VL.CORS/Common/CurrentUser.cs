using Autobots.Infrastracture.Common.ValuesSolution;
using System;

namespace ResearchAPI.CORS.Common
{
    public class CurrentUser
    {
        public CurrentUser()
        {
        }
        public CurrentUser(User data)
        {
            UserId = data.Id;
            UserName = data.Name;
        }

        public long UserId { set; get; }
        public string UserName { set; get; }

        public string GetSessionId() {
            return UserId + "_" + (UserId + DateTime.Now.ToString()).ToMD5();
        }
    }
}
