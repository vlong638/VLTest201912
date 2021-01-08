using System;
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
        /// 性别
        /// </summary>
        public Sex Sex { set; get; }
        /// <summary>
        /// 性别文本
        /// </summary>
        public string Sex_Text { set; get; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { set; get; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public List<GetUserRoleModel> Roles { set; get; }
        /// <summary>
        /// 用户机构
        /// </summary>
        public List<GetUserDepartmentModel> Departments { set; get; }
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

    /// <summary>
    /// 
    /// </summary>
    public class GetUserDepartmentModel
    {
        /// <summary>
        /// 机构Id
        /// </summary>
        public long DepartmentId { set; get; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string DepartmentName { set; get; }
    }
}
