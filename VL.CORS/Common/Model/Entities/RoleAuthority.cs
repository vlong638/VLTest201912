
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
        //查看指标列表 = 101002001,
        //查看队列列表 = 101003001,

        #endregion

        #region 账户系统 999

        查看用户列表 = 999001001,
        查看角色列表 = 999002001,
        //查看菜单列表 = 999003001,
        //查看登陆日志 = 999004001,

        #endregion
    }
}
