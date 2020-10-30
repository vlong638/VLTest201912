using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using VL.Consolo_Core.Common.ControllerSolution;
using BBee.Common;

namespace BBee.Controllers
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
            return new CurrentUser() { UserId = 1 };

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
        internal APIResult<T> Success<T>(T data, params string[] messages)
        {
            return new APIResult<T>(data, messages);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        internal APIResult<T1, T2> Success<T1, T2>(T1 data1, T2 data2, params string[] messages)
        {
            return new APIResult<T1, T2>(data1, data2, messages);
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
        internal APIResult<T1, T2> Error<T1, T2>(T1 data1, T2 data2, params string[] messages)
        {
            return new APIResult<T1, T2>(data1, data2, messages);
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