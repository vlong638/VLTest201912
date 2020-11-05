using Autobots.Common.ServiceExchange;
using Autobots.EMRServices.FileSolution;
using System.IO;
using System.Web.Services;

namespace Autobots.CommonServices.Services
{
    /// <summary>
    /// 文件系统服务
    /// 提供文件的维护
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class FileService : System.Web.Services.WebService
    {
        const string FileDirectory = "Files";

        [WebMethod]
        public APIResult<byte[]> ReadAllBytes(string fileName)
        {
            try
            {
                var fullPath = Path.Combine(FileHelper.GetDirectoryToOutput(FileDirectory), fileName);
                if (!File.Exists(fullPath))
                {
                    return new APIResult<byte[]>(null, 501, "文件不存在");
                }
                var data = File.ReadAllBytes(fullPath);
                return new APIResult<byte[]>(data);
            }
            catch (System.Exception ex)
            {
                return new APIResult<byte[]>(null, 500, ex.Message);
            }
        }
        [WebMethod]
        public APIResult<string> ReadAllTexts(string fileName)
        {
            try
            {
                var fullPath = Path.Combine(FileHelper.GetDirectoryToOutput(FileDirectory), fileName);
                if (!File.Exists(fullPath))
                {
                    return new APIResult<string>(null, 501, "文件不存在");
                }
                var data = File.ReadAllText(fullPath);
                return new APIResult<string>(data);
            }
            catch (System.Exception ex)
            {
                return new APIResult<string>(null, 500, ex.Message);
            }
        }

        [WebMethod]
        public APIResult<bool> WriteAllBytes(string fileName, byte[] bytes)
        {
            try
            {
                var fullPath = Path.Combine(FileHelper.GetDirectoryToOutput(FileDirectory), fileName);
                File.WriteAllBytes(fullPath, bytes);
                return new APIResult<bool>(true);
            }
            catch (System.Exception ex)
            {
                return new APIResult<bool>(false, 500, ex.Message);
            }
        }
    }
}
