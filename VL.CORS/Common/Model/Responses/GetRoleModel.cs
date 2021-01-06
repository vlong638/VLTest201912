using Autobots.Infrastracture.Common.ValuesSolution;
using System;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetRoleModel
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
