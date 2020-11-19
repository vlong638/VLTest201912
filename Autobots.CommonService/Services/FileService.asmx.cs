using Autobots.Common.ServiceBase;
using Autobots.Common.ServiceExchange;
using Autobots.CommonServices.Utils;
using Autobots.EMRServices.FileSolution;
using System;
using System.Collections.Generic;
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
        #region CommonFiles

        const string FileDirectory = "Files";

        [WebMethod]
        public APIResult<byte[]> ReadAllBytes(string fileName)
        {
            return WebServiceHelper.DelegateWebService(() =>
            {
                var fullPath = Path.Combine(FileHelper.GetDirectoryToOutput(FileDirectory), fileName);
                if (!File.Exists(fullPath))
                {
                    return new APIResult<byte[]>(null, 501, "文件不存在");
                }
                var data = File.ReadAllBytes(fullPath);
                return new APIResult<byte[]>(data);
            });
        }

        [WebMethod]
        public APIResult<string> ReadAllTexts(string fileName)
        {
            return WebServiceHelper.DelegateWebService(() =>
            {
                throw new NotImplementedException("error0949");

                var fullPath = Path.Combine(FileHelper.GetDirectoryToOutput(FileDirectory), fileName);
                if (!File.Exists(fullPath))
                {
                    return new APIResult<string>(null, 501, "文件不存在");
                }
                var data = File.ReadAllText(fullPath);
                return new APIResult<string>(data);
            });
        }

        [WebMethod]
        public APIResult<bool> WriteAllBytes(string fileName, byte[] bytes)
        {
            return WebServiceHelper.DelegateWebService(() =>
            {
                var fullPath = Path.Combine(FileHelper.GetDirectoryToOutput(FileDirectory), fileName);
                File.WriteAllBytes(fullPath, bytes);
                return new APIResult<bool>(true);
            });
        }
        [WebMethod]
        public APIResult<bool> WriteAllTexts(string fileName, string texts)
        {
            return WebServiceHelper.DelegateWebService(() =>
            {
                var fullPath = Path.Combine(FileHelper.GetDirectoryToOutput(FileDirectory), fileName);
                File.AppendAllText(fullPath, texts);
                return new APIResult<bool>(true);
            });
        } 

        #endregion
    }
}
