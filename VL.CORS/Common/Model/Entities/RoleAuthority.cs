
using Dapper.Contrib.Extensions;
using System.ComponentModel;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class RoleAuthority
    {
        public const string TableName = "RoleAuthority";

        public long RoleId { set; get; }
        public long AuthorityId { set; get; }
    }

    /// <summary>
    /// 权限 
    /// 前三位代表业务系统,后三位表示模块内功能模块,后三位表示功能模块内细分功能
    /// 通用功能为000
    /// </summary>
    public enum SystemAuthority
    {
        //None = 0,

        #region 项目管理 101
        查看项目列表 = 101001001,

        #endregion

        #region 账户系统 999

        查看用户列表 = 999001001,
        新增用户 = 999001002,
        修改用户信息 = 999001003,
        锁定用户状态 = 999001004,

        查看角色列表 = 999002001,
        新增角色 = 999002002,
        删除角色 = 999002003,
        修改角色信息 = 999002004,
        编辑角色权限 = 999002005,

        #endregion
    }
}
