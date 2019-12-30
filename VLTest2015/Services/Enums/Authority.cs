using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace VLTest2015.Services
{
    /// <summary>
    /// 权限 
    /// 前三位代表业务系统,后三位表示模块内功能模块,后三位表示功能模块内细分功能
    /// 通用功能为000
    /// </summary>
    public enum Authority
    {
        #region 分娩管理系统 001

        #region 分娩信息浏览模块 001
        /// <summary>
        /// 查看用户分娩信息 001
        /// </summary>
        [Description("查看用户分娩信息")]
        ViewChildBirthInfo = 001001001,
        /// <summary>
        /// 搜索用户分娩信息 002
        /// </summary>
        [Description("搜索用户分娩信息")]
        SearchChildBirthInfo = 001001002,
        #endregion

        #endregion

        #region 账户系统 999

        #region 账户模块 001

        /// <summary>
        /// 查看用户列表 001
        /// </summary>
        [Description("查看用户列表")]
        ViewUserList = 999001001,

        /// <summary>
        /// 编辑用户角色 002
        /// </summary>
        [Description("编辑用户角色")]
        EditUserRole = 999001002,

        #endregion

        #region 角色模块 002

        /// <summary>
        /// 查看角色列表 001
        /// </summary>
        [Description("查看角色列表")]
        ViewRoleList = 999002001,

        /// <summary>
        /// 创建角色 002
        /// </summary>
        [Description("创建角色")]
        AddRole = 999002002,

        /// <summary>
        /// 编辑角色权限 003
        /// </summary>
        [Description("编辑角色权限")]
        EditRoleAuthority = 999002003,

        #endregion

        #endregion
    }
}