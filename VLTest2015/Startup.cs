using Microsoft.Owin;
using NPOI.OpenXml4Net.OPC.Internal;
using Owin;
using System.IO;

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
