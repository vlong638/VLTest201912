using System.Web;
using System.Web.Mvc;

namespace WebAPI.VLServer.Framework452
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
