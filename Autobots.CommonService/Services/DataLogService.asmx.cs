using Autobots.Common.ServiceCommon;
using System.Collections.Generic;
using System.Web.Services;

namespace Autobots.CommonServices.Services
{
    /// <summary>
    /// DataLogService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class DataLogService : System.Web.Services.WebService
    {

        [WebMethod]
        public bool SaveDataLog(string oldContent, string newContent,string operateBy)
        {
            return true;
        }

        [WebMethod]
        public List<DataLogChange> CompareDataLog(string oldContent, string newContent)
        {
            return new List<DataLogChange>() {
                new DataLogChange("姓名","王武","王五"),
                new DataLogChange("年龄","25","15"),
            };
        }
    }
}
