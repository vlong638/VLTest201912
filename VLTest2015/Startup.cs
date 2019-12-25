using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VLTest2015.Startup))]
namespace VLTest2015
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
