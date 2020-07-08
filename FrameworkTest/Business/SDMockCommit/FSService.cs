using FrameworkTest.Common.HttpSolution;
using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace FrameworkTest.Business.SDMockCommit
{
    public class FSService
    {
        #region Common

        /// <summary>
        /// 获取 UniqueId
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="base8"></param>
        /// <param name="issueDate"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public string GetUniqueId(UserInfo userInfo, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_QHJC_ID_GET&sUserID={userInfo.UserId}&sParams=1";
            var postData = "";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            var mainId = result.FromJsonToAnonymousType(new { id = "" }).id;
            logger.AppendLine(">>>获取 UniqueId");
            logger.AppendLine(url);
            logger.AppendLine(result);
            return mainId;
        }

        /// <summary>
        /// 获取 保健号
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="base8"></param>
        /// <param name="issueDate"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public string GetCareId(UserInfo userInfo, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=CDH_GET_ID1&sUserID={userInfo.UserId}&sParams={userInfo.OrgId}";
            var postData = "";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            var careId = result.FromJsonToAnonymousType(new { id = "" }).id;
            logger.AppendLine($"查询-保健号");
            logger.AppendLine(url);
            logger.AppendLine(result);
            return careId;
        }

        internal WCQBJ_CZDH_DOCTOR_READData GetBase8(UserInfo userInfo, string idCard, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var postData = "";
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID={userInfo.UserId}&sParams=P${idCard}$P$P";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            var resultBase = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
            logger.AppendLine($">>>查询 孕妇档案 Base8");
            logger.AppendLine(url);
            logger.AppendLine(result);
            logger.AppendLine(resultBase.ToJson());
            if (resultBase == null || resultBase.data == null || resultBase.data.Count == 0)
                return null;
            return resultBase.data.First();
        }

        #endregion

        #region 孕妇档案

        internal WMH_CQBJ_JBXX_FORM_READData GetBase77(UserInfo userInfo, string mainId, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var postData = "";
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_READ&sUserID={userInfo.UserId}&sParams={mainId}";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            var resultBase = result.FromJson<WMH_CQBJ_JBXX_FORM_READResponse>();
            logger.AppendLine($">>>查询 孕妇档案77项数据");
            logger.AppendLine(result);
            logger.AppendLine(resultBase.ToJson());
            if (resultBase == null || resultBase.data == null || resultBase.data.Count == 0)
                return null;
            return resultBase.data.First();
        }

        internal WCQBJ_CZDH_DOCTOR_READData GetPregnantInfo(UserInfo userInfo, string idCard, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var postData = "";
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID={userInfo.UserId}&sParams=P${idCard}$P$P";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            var re = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
            logger.AppendLine($">>>查询-获取孕妇档案");
            logger.AppendLine(url);
            logger.AppendLine(result);
            logger.AppendLine(re?.ToJson());
            return re?.data?.FirstOrDefault();
        }

        /// <summary>
        /// 查询-获取全部高危列表
        /// </summary>
        /// <param name="newHighRiskId"></param>
        /// <param name="userInfo"></param>
        /// <param name="base8"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal List<WMH_GWYCF_GW_LIST1_Data> GetAllHighRisks(string newHighRiskId, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READData base8, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var postData = "";
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_GWYCF_GW_LIST1&sUserID={userInfo.UserId}&sParams={base8.MainId}${newHighRiskId}";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            var re = result.FromJson<WMH_GWYCF_GW_LIST1>();
            logger.AppendLine($">>>查询-获取全部高危列表");
            logger.AppendLine(url);
            logger.AppendLine(result);
            logger.AppendLine(re?.ToJson());
            return re?.data;
        }
        /// <summary>
        /// 查询-获取变动的高危列表
        /// </summary>
        /// <param name="newHighRiskId"></param>
        /// <param name="userInfo"></param>
        /// <param name="base8"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal List<WMH_GWYCF_LIST_Data> GetCurrentHighRisks(string newHighRiskId, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READData base8, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var postData = "";
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_GWYCF_LIST&sUserID={userInfo.UserId}&sParams={base8.MainId}${newHighRiskId}";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            var re = result.FromJson<WMH_GWYCF_LIST>();
            logger.AppendLine($">>>查询-获取变动的高危列表");
            logger.AppendLine(url);
            logger.AppendLine(result);
            logger.AppendLine(re?.ToJson());
            return re?.data;
        }

        /// <summary>
        /// 保存基本信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="mainId"></param>
        /// <param name="datas"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal bool CreatePregnantInfo(UserInfo userInfo, string mainId, List<WMH_CQBJ_JBXX_FORM_SAVEData> datas, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_SAVE&sUserID={userInfo.UserId}&sParams=null${mainId}${userInfo.OrgId}${userInfo.EncodeUserName}$null$null$null$%E6%99%AE%E9%80%9A%E6%8A%A4%E5%A3%AB%E4%BA%A7%E6%A3%80";
            var json = datas.ToJson();
            var postData = "data=" + HttpUtility.UrlEncode(json);
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine(">>>CreatePregnantInfo 保存基本信息");
            logger.AppendLine(url);
            logger.AppendLine(json);
            logger.AppendLine(result);
            return result.Contains("处理成功");
        }

        /// <summary>
        /// 保存高危信息
        /// </summary>
        /// <param name="physicalExaminationId"></param>
        /// <param name="datas"></param>
        /// <param name="userInfo"></param>
        /// <param name="base8"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal bool SaveCurrentHignRisks(string physicalExaminationId, WMH_WCQBJ_GWYCF_SCORE_SAVERequest datas, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READData base8, ref StringBuilder logger)
        {
            //logger.AppendLine(">>>SaveCurrentHignRisks");
            //logger.AppendLine(highRisksToSave.ToJson());

            var container = new CookieContainer();
            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_GWYCF_GW_LIST_SAVE&sUserID={userInfo.UserId}&sParams={base8.MainId}${userInfo.OrgId}${physicalExaminationId}";
            var json = datas.ToJson();
            var postData = "data=" + HttpUtility.UrlEncode(json);
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine(">>>SaveCurrentHignRisks 保存高危信息");
            logger.AppendLine(url);
            logger.AppendLine(json);
            logger.AppendLine(result);
            return result.Contains("处理成功");
        }

        internal bool UpdatePregnantInfo(UserInfo userInfo, string mainId, string mainIdForChange, List<WMH_CQBJ_JBXX_FORM_SAVEData> datas, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_SAVE&sUserID={userInfo.UserId}&sParams={mainIdForChange}${mainId}${userInfo.OrgId}${userInfo.EncodeUserName}$null$null$null$%E6%99%AE%E9%80%9A%E6%8A%A4%E5%A3%AB%E4%BA%A7%E6%A3%80";
            var json = datas.ToJson();
            var postData = "data=" + HttpUtility.UrlEncode(json);
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine(">>>Create 基本信息");
            logger.AppendLine(url);
            logger.AppendLine(json);
            logger.AppendLine(result);
            return result.Contains("处理成功");
        }

        #endregion

        #region 体格检查
        /// <summary>
        /// 获取体格检查Id
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="base8"></param>
        /// <param name="issueDate"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public string GetPhysicalExaminationId(UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READData base8, DateTime issueDate, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var dateStr = issueDate.ToString("yyyy-MM-dd");
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_TODAY_CQJC_ID_READ&sUserID={userInfo.UserId}&sParams={base8.MainId}${dateStr}";
            var postData = "";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine($">>>查询-获取体格检查Id");
            logger.AppendLine(url);
            logger.AppendLine(result);
            var re2 = result.FromJson<WMH_TODAY_CQJC_ID_READ>();
            if (string.IsNullOrEmpty(re2.d1))
                return null;
            return re2.d1;
        }

        internal bool IsExist(UserInfo userInfo, string mainId, string careId, PregnantInfo_SourceData sourceData, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_CC&sUserID={userInfo.UserId}&sParams={mainId}$P${careId}${sourceData.IdCard}&pageSize=10000&pageIndex=0";
            var postData = "";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            var repeatData = result.FromJson<WMH_CQBJ_JBXX_FORM_CC>();
            if (repeatData.data.Count != 0 && repeatData.data.FirstOrDefault(c => c.PersonName != sourceData.PersonName) != null)
            {
                logger.AppendLine($">>>查重时,出现重复");
                logger.AppendLine(result);
                return true;
            }
            return false;
        }
        #endregion

        #region 专科检查
        /// <summary>
        /// 获取专科检查数据
        /// </summary>
        /// <param name="physicalExaminationId"></param>
        /// <param name="userInfo"></param>
        /// <param name="base8"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal WMH_CQBJ_CQJC_READ_Data GetProfessionalExamination(string physicalExaminationId, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READData base8, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_READ&sUserID={userInfo.UserId}&sParams={physicalExaminationId}${base8.MainId}";
            var postData = "";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine($">>>查询-获取体格检查数据");
            logger.AppendLine(url);
            logger.AppendLine(result);
            var re2 = result.FromJson<WMH_CQBJ_CQJC_READ>();
            if (re2 == null || re2.data == null || re2.data.Count == 0)
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
        internal bool UpdateProfessionalExamination(string physicalExaminationId, WMH_CQBJ_CQJC_SAVE professionalExaminationNew, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READData base8, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_SAVE&sUserID={userInfo.UserId}&sParams={base8.MainId}${userInfo.OrgId}${physicalExaminationId}$1$P$%E6%99%AE%E9%80%9A%E4%BA%A7%E6%A3%80%E5%8C%BB%E7%94%9F";
            var json = new List<WMH_CQBJ_CQJC_SAVE>() { professionalExaminationNew }.ToJson();
            var postData = "data=" + HttpUtility.UrlEncode(json);
            //var postData = "data=" + json;
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine($">>>提交-专科检查数据");
            logger.AppendLine(url);
            logger.AppendLine(postData);
            logger.AppendLine(result);
            return result.Contains("处理成功");
        } 
        #endregion
    }
}