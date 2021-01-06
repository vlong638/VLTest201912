using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { set; get; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { set; get; }
        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { set; get; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { set; get; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public List<long> RoleIds { set; get; }
    }
}
