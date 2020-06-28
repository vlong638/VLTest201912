using Dapper.Contrib.Extensions;
using System;

namespace FrameworkTest.Business.SDMockCommit
{
    public class UserInfo
    {
        public string UserId  { set; get; }
        public string UserName  { set; get; }
        public string OrgId  { set; get; }
        public string OrgName  { set; get; }
        public object EncodeUserName { get; internal set; }
    }
}
