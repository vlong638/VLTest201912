using System;
using System.Collections.Generic;
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
        #region 分娩管理系统001

        #region 分娩信息浏览模块001
        /// <summary>
        /// 查看用户分娩信息001
        /// </summary>
        ViewChildBirthInfo = 001001001,
        /// <summary>
        /// 搜索用户分娩信息002
        /// </summary>
        SearchChildBirthInfo = 001001002, 
        #endregion



        #endregion
    }
}