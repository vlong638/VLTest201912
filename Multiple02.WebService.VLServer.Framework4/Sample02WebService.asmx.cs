using System.Web.Services;

namespace Multiple02.WebService.VLServer.Framework4
{
    /// <summary>
    /// SampleWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://microsoft.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Sample02WebService : System.Web.Services.WebService
    {
        [WebMethod]
        public string HelloWorld(HelloRequest hello)
        {
            return hello.Name;
        }

        [WebMethod]
        public string HelloCommon()
        {
            return new ServiceReferenceCommon.MultipleCommonSoapClient().HelloWorld();
        }
    }

    public class HelloRequest
    {
        public string Name { set; get; }
    }
}
