using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetUsersRequest
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int Page { set; get; }
        /// <summary>
        /// 页面显示数量
        /// </summary>
        public int Limit { set; get; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { set; get; }
    }
}
