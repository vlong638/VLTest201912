using System.Web.Mvc;
using VLTest2015.Attributes;

namespace VLTest2015
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new VLAuthenticationAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
