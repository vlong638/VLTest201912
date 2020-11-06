using Autobots.CommonServices.Utils;
using Autofac;
using System;

namespace Autobots.InpatientService
{
    public class Global : System.Web.HttpApplication
    {
        public static IContainer Container { set; get; }

        protected void Application_Start(object sender, EventArgs e)
        {
            //配置AutoFac
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new Log4NetLogger());
            Container = builder.Build();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}