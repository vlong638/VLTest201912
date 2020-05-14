using System.Collections.Generic;

namespace VLTest2015.Services
{
    public class GetUserPagedListResponse
    {
        public long UserId { set; get; }
        public string UserName { set; get; }
        public IEnumerable<string> RoleNames { set; get; }
        public string RoleNamesStr { get { return string.Join(",", RoleNames); } }
    }
}