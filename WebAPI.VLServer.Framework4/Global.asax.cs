using System;

namespace WebAPI.VLServer.Framework4
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            WebApiConfig.Register(System.Web.Http.GlobalConfiguration.Configuration);
        }
    }
}