using Autobots.Common.ServiceExchange;
using System;

namespace Autobots.CommonServices.Utils
{
    public static class WebServiceHelper
    {
        public static APIResult DelegateWebService(Func<APIResult> doSomeThing)
        {
            try
            {
                return doSomeThing();
            }
            catch (Exception ex)
            {
                Log4NetLogger.SystemError(ex);
                return new APIResult(500, ex.Message);
            }
        }

        public static APIResult<T> DelegateWebService<T>(Func<APIResult<T>> doSomeThing)
        {
            try
            {
                return doSomeThing();
            }
            catch (Exception ex)
            {
                Log4NetLogger.SystemError(ex);
                return new APIResult<T>(default, 500, ex.Message);
            }
        }
    }
}