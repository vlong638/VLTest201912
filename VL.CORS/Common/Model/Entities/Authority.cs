using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchAPI.CORS.Common
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
        None = 0,

        #region 项目管理 101

        CreateProject = 101001001,
        EditProject = 101001002,
        DeleteProject = 101001003,

        #endregion

        #region Pregnant 102

        查看孕妇档案列表 = 102001001,
        查看孕妇档案详情 = 102001002,
        查看产检列表 = 102002001,

        查看检查列表 = 102003001,
        查看检查详情 = 102003002,

        #endregion

    }
}
