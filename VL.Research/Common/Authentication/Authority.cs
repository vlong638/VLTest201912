using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VL.Research.Common
{
    /// <summary>
    /// 权限 
    /// 前三位代表业务系统,后三位表示模块内功能模块,后三位表示功能模块内细分功能
    /// 通用功能为000
    /// </summary>
    public enum Authority
    {
        /// <summary>
        /// 无效权限
        /// </summary>
        None =  0,

        #region 分娩管理系统 101

        #region 分娩信息浏览模块 101
        /// <summary>
        /// 查看用户分娩信息 101
        /// </summary>
        查看用户分娩信息 = 101001001,
        /// <summary>
        /// 搜索用户分娩信息 102
        /// </summary>
        搜索用户分娩信息 = 101001002,
        #endregion

        #endregion

        #region Pregnant 102

        查看孕妇档案列表 = 102001001,
        查看孕妇档案详情 = 102001002,
        查看产检列表 = 102002001,

        查看检查列表 = 102003001,
        查看检查详情 = 102003002,

        #endregion


        #region 账户系统 999

        #region 账户模块 001

        /// <summary>
        /// 查看用户列表 001
        /// </summary>
        [Description("查看用户列表")]
        查看用户列表 = 999001001,

        /// <summary>
        /// 编辑用户角色 002
        /// </summary>
        [Description("编辑用户角色")]
        编辑用户角色 = 999001002,

        #endregion

        #region 角色模块 002

        /// <summary>
        /// 查看角色列表 001
        /// </summary>
        [Description("查看角色列表")]
        查看角色列表 = 999002001,

        /// <summary>
        /// 创建角色 002
        /// </summary>
        [Description("创建角色")]
        创建角色 = 999002002,

        /// <summary>
        /// 编辑角色权限 003
        /// </summary>
        [Description("编辑角色权限")]
        编辑角色权限 = 999002003,

        #endregion

        #endregion

    }
}