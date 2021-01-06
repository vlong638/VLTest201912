
using Dapper.Contrib.Extensions;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class Role
    {
        public const string TableName = "Role";

        public Role()
        {
        }
        public Role(string roleName)
        {
            Name = roleName;
        }

        public long Id { set; get; }
        public string Name { set; get; }
        public RoleCategory Category { set; get; }
    }

    /// <summary>
    /// 角色类型
    /// </summary>
    public enum RoleCategory
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 项目内角色
        /// </summary>
        ProjectRole = 1,
        /// <summary>
        /// 系统角色
        /// </summary>
        SystemRole = 2,
    }
}
