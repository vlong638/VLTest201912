using Autobots.Infrastracture.Common.ControllerSolution;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Common
{

    /// <summary>
    /// 
    /// </summary>
    public class APIBaseController : Controller
    {
        #region APIResult,便捷方法
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        internal APIResult<T> Success<T>(T data)
        {
            return new APIResult<T>(data);
        }
        /// <summary>
        /// 
        /// </summary>
        internal APIResult<T> Success<T>(T data, params string[] messages)
        {
            return new APIResult<T>(data, messages);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal APIResult<T> Error<T>(T data, IList<string> messages)
        {
            return new APIResult<T>(data, messages.ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal APIResult<T> Error<T>(T data, int code, IList<string> messages)
        {
            return new APIResult<T>(data, code, messages.ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal APIResult<T> Error<T>(params string[] messages)
        {
            return new APIResult<T>(default(T), messages);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal APIResult<T> Error<T>(T data, params string[] messages)
        {
            return new APIResult<T>(data, messages);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal APIResult<T> Error<T>(T data, int code, params string[] messages)
        {
            return new APIResult<T>(data, code, messages);
        }
        #endregion
    }
}
