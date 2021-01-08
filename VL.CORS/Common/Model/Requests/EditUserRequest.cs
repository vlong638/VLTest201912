using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class EditUserRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public long UserId { set; get; }
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
        /// <summary>
        /// 机构Id
        /// </summary>
        public List<long> DepartmentIds { set; get; }
    }
}
