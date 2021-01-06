using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetUserModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { set; get; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { set; get; }
        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDeleted { set; get; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public List<GetUserRoleModel> Roles { set; get; }

    }
    /// <summary>
    /// 
    /// </summary>
    public class GetUserRoleModel
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { set; get; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { set; get; }

    }
}
