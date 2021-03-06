﻿using Autobots.Common.ServiceBase;
using Autobots.CommonServices.Utils;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace Autobots.InpatientService
{
    /// <summary>
    /// InpatientService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class InpatientService : System.Web.Services.WebService , IHealthCheck
    {
        #region IHealthCheck

        [WebMethod]
        public bool IsAlive()
        {
            return true;
        }

        public LoadingCheckReport GetLoadingCheckReport()
        {
            throw new NotImplementedException();
        }

        public List<ReferenceCheckReport> GetReferenceCheckReports()
        {
            throw new NotImplementedException();
        } 
        #endregion


        [WebMethod]
        public string GetPageConfig()
        {
            try
            {
                throw new NotImplementedException("888G");

                var file = @"a.txt";
                var bytes = Encoding.UTF8.GetBytes("8G");
                var fileService = new FileServiceReference.FileServiceSoapClient();
                var result1 = fileService.WriteAllBytes(file, bytes);
                var result2 = fileService.ReadAllTexts(file);
                var result3 = fileService.ReadAllBytes(file);
                return "Hello World";
            }
            catch (Exception ex)
            {
                Global.Container.Resolve<Log4NetLogger>().Error(ex);
                return ex.Message;
            }
        }
    }
}
