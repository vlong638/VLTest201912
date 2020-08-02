using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using VL.Consolo_Core.Common.ControllerSolution;
using VL.Research.Common;

namespace VL.Research.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    //[VLAuthentication]
    public class APIBaseController : Controller
    {
        #region Auth

        //public void SetCurrentUser(CurrentUser currentUser, bool isRemeberMe = false)
        //{
        //    CurrentUser.SetCurrentUser(currentUser, isRemeberMe, Response);
        //}

        internal CurrentUser GetCurrentUser()
        {
            return new CurrentUser();

            //var httpContext = HttpContext;
            //return CurrentUser.GetCurrentUser(httpContext);
        }
        #endregion

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