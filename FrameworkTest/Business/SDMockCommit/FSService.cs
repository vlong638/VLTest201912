using Dapper;
using Dapper.Contrib.Extensions;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.FileSolution;
using FrameworkTest.Common.HttpSolution;
using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FrameworkTest.Business.SDMockCommit
{
    public class FSService
    {
        /// <summary>
        /// 获取体格检查Id
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="base8"></param>
        /// <param name="issueDate"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public string GetPhysicalExaminationId(UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READResponse base8, DateTime issueDate, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var dateStr = issueDate.ToString("yyyy-MM-dd");
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_TODAY_CQJC_ID_READ&sUserID={userInfo.UserId}&sParams={base8.MainId}${dateStr}";
            var postData = "";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine($"查询-获取体格检查Id");
            logger.AppendLine(url);
            logger.AppendLine(result);
            var re2 = result.FromJson<WMH_TODAY_CQJC_ID_READ>();
            if (string.IsNullOrEmpty(re2.d1))
                return null;
            return re2.d1;
        }

        /// <summary>
        /// 获取专科检查数据
        /// </summary>
        /// <param name="physicalExaminationId"></param>
        /// <param name="userInfo"></param>
        /// <param name="base8"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal WMH_CQBJ_CQJC_READ_Data GetProfessionalExamination(string physicalExaminationId, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READResponse base8,  ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_READ&sUserID={userInfo.UserId}&sParams={physicalExaminationId}${base8.MainId}";
            var postData = "";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine($"查询-获取体格检查数据");
            logger.AppendLine(url);
            logger.AppendLine(result);
            var re2 = result.FromJson<WMH_CQBJ_CQJC_READ>();
            if (re2==null || re2.data==null || re2.data.Count==0)
                return null;
            return re2.data.First();
        }

        /// <summary>
        /// 提交专科检查数据
        /// </summary>
        /// <param name="physicalExaminationId"></param>
        /// <param name="professionalExaminationNew"></param>
        /// <param name="userInfo"></param>
        /// <param name="base8"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal string UpdateProfessionalExamination(string physicalExaminationId, WMH_CQBJ_CQJC_SAVE professionalExaminationNew, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READResponse base8, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_SAVE&sUserID={userInfo.UserId}&sParams={base8.MainId}${userInfo.OrgId}${physicalExaminationId}$1$P$%E6%99%AE%E9%80%9A%E4%BA%A7%E6%A3%80%E5%8C%BB%E7%94%9F";
            var json = new List<WMH_CQBJ_CQJC_SAVE>() { professionalExaminationNew }.ToJson();
            var postData = "data=" + HttpUtility.UrlEncode(json);
            //var postData = "data=" + json;
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine($"提交-专科检查数据");
            logger.AppendLine(url);
            logger.AppendLine(postData);
            logger.AppendLine(result);
            return result;
        }
    }
}
