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

        /// <summary>
        /// 获取Base8 8项概要信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="idCard"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取 Base18 18项概要信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="idCard"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal WMH_WCQBJ_CZDH_JBXX_READ_Data GetBase18(UserInfo userInfo, string mainId, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var postData = "";
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_WCQBJ_CZDH_JBXX_READ&sUserID={userInfo.UserId}&sParams={mainId}${userInfo.OrgId}";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            var resultBase = result.FromJson<WMH_WCQBJ_CZDH_JBXX_READ>();
            logger.AppendLine($">>>查询 Base18");
            logger.AppendLine(url);
            logger.AppendLine(result);
            logger.AppendLine(resultBase.ToJson());
            if (resultBase == null || resultBase.data == null || resultBase.data.Count == 0)
                return null;
            return resultBase.data.First();
        }

        /// <summary>
        /// 获取Base 77 孕妇档案详情
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="mainId"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
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

        #endregion

        #region 孕妇档案

        internal bool IsExistByMainIdOrIdCard(UserInfo userInfo, string mainId, PregnantInfo_SourceData sourceData, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_CC&sUserID={userInfo.UserId}&sParams={mainId}$P$null${sourceData.Data.idcard}&pageSize=10000&pageIndex=0";
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

        internal bool IsExistByCareId(UserInfo userInfo, string mainId, string careId, PregnantInfo_SourceData sourceData, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_CC&sUserID={userInfo.UserId}&sParams=null$P${careId}$null&pageSize=10000&pageIndex=0";
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
            logger.AppendLine(">>>Update 基本信息");
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

        /// <summary>
        /// 查询体格检查
        /// </summary>
        /// <param name="physicalExaminationId"></param>
        /// <param name="userInfo"></param>
        /// <param name="base8"></param>
        /// <param name="now"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal WMH_CQBJ_CQJC_TGJC_NEW_READ_Data GetPhysicalExamination(string physicalExaminationId, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READData base8, DateTime now, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            //查询体格检查详情
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_TGJC_NEW_READ&sUserID={userInfo.UserId}&sParams={base8.MainId}${physicalExaminationId}";
            var postData = "";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine($"查询-查询体格检查详情");
            logger.AppendLine(url);
            logger.AppendLine(result);
            var re3 = result.FromJson<WMH_CQBJ_CQJC_TGJC_NEW_READ>();
            if (re3.data.Count == 0)
                return null;
            return re3.data.FirstOrDefault();
        }

        /// <summary>
        /// 新建体格检查
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="userInfo"></param>
        /// <param name="uniqueId"></param>
        /// <param name="base8"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal bool CreatePhysicalExamination(List<WMH_CQBJ_CQJC_TGJC_NEW_SAVE_Data> datas, UserInfo userInfo, string uniqueId, WCQBJ_CZDH_DOCTOR_READData base8, ref StringBuilder logger)
        {
            //创建体格检查
            var container = new CookieContainer();
            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_TGJC_NEW_SAVE&sUserID={userInfo.UserId}&sParams={userInfo.OrgId}${base8.MainId}$null${uniqueId}";
            var json = datas.ToJson();
            var postData = "data=" + HttpUtility.UrlEncode(json);
            logger.AppendLine("Create 体格检查");
            logger.AppendLine(url);
            logger.AppendLine(json);
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine(result);
            return result.Contains("处理成功");
        }

        /// <summary>
        /// 更新体格检查
        /// </summary>
        /// <param name="physicalExaminationId"></param>
        /// <param name="datas"></param>
        /// <param name="userInfo"></param>
        /// <param name="base8"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal bool UpdatePhysicalExamination(string physicalExaminationId, List<WMH_CQBJ_CQJC_TGJC_NEW_SAVE_Data> datas, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READData base8, ref StringBuilder logger)
        {
            //Create 体格检查索引ID
            var container = new CookieContainer();
            //创建提个检查
            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_TGJC_NEW_SAVE&sUserID={userInfo.UserId}&sParams={userInfo.OrgId}${base8.MainId}$null${physicalExaminationId}";
            var json = datas.ToJson();
            var postData = "data=" + HttpUtility.UrlEncode(json); 
            logger.AppendLine("Update 体格检查");
            logger.AppendLine(url);
            logger.AppendLine(json);
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine(result);
            return result.Contains("处理成功");
        }

        #endregion

        #region 问询病史

        /// <summary>
        /// 查询问询病史
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="base8"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal MQDA_READ_NEWData GetEnquiry(UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READData base8, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/MQDA_READ_NEW&sUserID={userInfo.UserId}&sParams={base8.MainId}${userInfo.OrgId}";
            var postData = "";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine($"查询-问询病史");
            logger.AppendLine(url);
            logger.AppendLine(result);
            var re2 = result.FromJson<MQDA_READ_NEWResponse>();
            if (re2 == null || re2.data == null || re2.data.Count == 0)
                return null;
            return re2.data.First();
        }

        /// <summary>
        /// 更新问询病史
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="datas"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal bool UpdateEnquiry(UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READData base8, List<Data_MQDA_XWBS_SAVE> datas, ref StringBuilder logger)
        {
            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/MQDA_XWBS_SAVE&sUserID={userInfo.UserId}&sParams={base8.MainId}${userInfo.OrgId}$%E6%97%A0$%E6%97%A0$%E6%97%A0$%E6%97%A0";
            var json = datas.ToJson();
            var container = new CookieContainer();
            var postData = "data=" + HttpUtility.UrlEncode(json);
            logger.AppendLine("Update 问询病史");
            logger.AppendLine(url);
            logger.AppendLine(json);
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine(result);
            return result.Contains((string)"处理成功");
        }

        /// <summary>
        /// 查询生育史
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="base8"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        internal List<WMH_CQBJ_CQJC_PRE_READ_Data> GetEnquiryPregnanths(UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READData base8, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_PRE_READ&sUserID={userInfo.UserId}&sParams={base8.MainId}";
            var postData = "pageIndex=0&pageSize=1000&sortField=&sortOrder=";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            var re = result.FromJson<WMH_CQBJ_CQJC_PRE_READ>();
            logger.AppendLine($"查询-问询病史-生育史");
            logger.AppendLine(url);
            logger.AppendLine(result);
            return re.data;
        }

        /// <summary>
        /// 新增生育史
        /// </summary>
        /// <param name="toAdd"></param>
        /// <param name="userInfo"></param>
        /// <param name="base8"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal bool AddEnquiryPregnanth(WMH_CQBJ_CQJC_PRE_SAVE toAdd, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READData base8, ref StringBuilder logger)
        {
            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_PRE_SAVE&sUserID={userInfo.UserId}&sParams={base8.MainId}${userInfo.OrgId}";
            var datas = new List<WMH_CQBJ_CQJC_PRE_SAVE>();
            datas.Add(toAdd);
            var json = datas.ToJson();
            var container = new CookieContainer();
            var postData = "data=" + HttpUtility.UrlEncode(json);
            logger.AppendLine("Add 问询病史-生育史");
            logger.AppendLine(url);
            logger.AppendLine(json);
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine(result);
            return result.Contains("处理成功");
        }

        internal bool DeleteEnquiryPregnanth(WMH_CQBJ_CQJC_PRE_SAVE toChange, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READData base8, ref StringBuilder logger)
        {
            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_SYS_DELETE&sUserID={userInfo.UserId}&sParams=1$1";
            var datas = new List<WMH_CQBJ_CQJC_PRE_SAVE>();
            datas.Add(toChange);
            var json = datas.ToJson();
            var container = new CookieContainer();
            var postData = "data=" + HttpUtility.UrlEncode(json); 
            logger.AppendLine("Delete 问询病史-生育史");
            logger.AppendLine(url);
            logger.AppendLine(json);
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine(result);
            return result.Contains("处理成功");
        }

        internal bool UpdateEnquiryPregnanth(WMH_CQBJ_CQJC_PRE_SAVE toChange, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READData base8, ref StringBuilder logger)
        {
            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_PRE_SAVE&sUserID={userInfo.UserId}&sParams={base8.MainId}${userInfo.OrgId}";
            var datas = new List<WMH_CQBJ_CQJC_PRE_SAVE>();
            datas.Add(toChange);
            var json = datas.ToJson();
            var container = new CookieContainer();
            var postData = "data=" + HttpUtility.UrlEncode(json);
            logger.AppendLine("Update 问询病史-生育史");
            logger.AppendLine(url);
            logger.AppendLine(json);
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine(result);
            return result.Contains("处理成功");
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
            result = result.Replace("\\", "");//针对特殊值异常
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
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine($">>>提交-专科检查数据");
            logger.AppendLine(url);
            logger.AppendLine(json);
            logger.AppendLine(result);
            return result.Contains("处理成功");
        }

        #endregion

        #region 孕妇住院数据

        /// <summary>
        /// 获取 孕妇住院数据列表
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="idCard"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal CQJL_LIST_Data GetPregnantInHospitalList(UserInfo userInfo, string inp_no, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/CQJL_LIST&sUserID={userInfo.UserId}&sParams=null${userInfo.OrgId}$1${inp_no}$P$P$4406";
            var postData = "pageIndex=0&pageSize=20&sortField=&sortOrder=";
            var contentType = "application/x-www-form-urlencoded; charset=UTF-8";
            var result = HttpHelper.Post(url, postData, ref container, contentType);
            logger.AppendLine($">>>查询-获取孕妇住院数据列表");
            logger.AppendLine(url);
            logger.AppendLine(contentType);
            logger.AppendLine(postData);
            logger.AppendLine(result);
            var re2 = result.FromJson<CQJL_LIST>();
            if (re2 == null || re2.data == null || re2.data.Count == 0)
                return null;
            return re2.data.FirstOrDefault(c => Convert.ToDateTime(c.D5) == re2.data.Select(d => Convert.ToDateTime(d.D5)).Max());
        }

        #endregion

        #region 孕妇出院登记

        /// <summary>
        /// 获取 孕妇出院登记
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="fmMainId"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal CQJL_WOMAN_FORM_READ_Data GetPregnantDischarge(UserInfo userInfo, string fmMainId, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/CQJL_WOMAN_FORM_READ&sUserID={userInfo.UserId}&sParams=P${userInfo.OrgId}${fmMainId}$null";
            var postData = "";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine($">>>查询-获取孕妇出院登记详情");
            logger.AppendLine(url);
            logger.AppendLine(result);
            var re2 = result.FromJson<CQJL_WOMAN_FORM_READ>();
            if (re2 == null || re2.data == null || re2.data.Count == 0 || re2.data.Count > 1)
                return null;
            return re2.data.First();
        }

        /// <summary>
        /// 创建 孕妇出院登记
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="pregnantDischargeToCreate"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal bool SavePregnantDischarge(UserInfo userInfo, CQJL_LIST_Data listData, CQJL_WOMAN_FORM_SAVE_Data pregnantDischargeToCreate, string dischargeId, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/CQJL_WOMAN_FORM_SAVE&sUserID={userInfo.UserId}&sParams=P${userInfo.OrgId}${listData.FMMainId}${dischargeId}${userInfo.UserId}$%E8%93%9D%E8%89%B3%E4%BA%91";
            var json = new List<CQJL_WOMAN_FORM_SAVE_Data>() { pregnantDischargeToCreate }.ToJson();
            var postData = "data=" + HttpUtility.UrlEncode(json);
            logger.AppendLine($">>>保存-孕妇出院登记");
            logger.AppendLine(url);
            logger.AppendLine(json);
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine(result);
            return result.Contains("处理成功");
        }

        #endregion

        #region 婴儿出院登记

        /// <summary>
        /// 获取婴儿出院登记详情
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="fMMainId"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal CQJL_CHILD_FORM_READ_Data GetChildDischarge(UserInfo userInfo, string fMMainId, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/CQJL_CHILD_FORM_READ&sUserID={userInfo.UserId}&sParams=P${userInfo.OrgId}${fMMainId}$null$1";
            var postData = "";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine($">>>查询-获取婴儿出院登记详情");
            logger.AppendLine(url);
            logger.AppendLine(result);
            var re2 = result.FromJson<CQJL_CHILD_FORM_READ>();
            if (re2 == null || re2.data == null || re2.data.Count == 0 || re2.data.Count > 1)
                return null;
            return re2.data.First();
        }
        /// <summary>
        /// 保存 婴儿出院登记详情
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="childDischargeToCreate"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal object SaveChildDischarge(UserInfo userInfo, CQJL_CHILD_FORM_SAVE_Data childDischargeToCreate, string fMMainId, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/CQJL_CHILD_FORM_SAVE&sUserID={userInfo.UserId}&sParams=P${userInfo.OrgId}${fMMainId}$null${userInfo.UserId}$1$ET_CYB1";
            var json = new List<CQJL_CHILD_FORM_SAVE_Data>() { childDischargeToCreate }.ToJson();
            var postData = "data=" + HttpUtility.UrlEncode(json);
            logger.AppendLine($">>>保存-婴儿出院登记");
            logger.AppendLine(url);
            logger.AppendLine(json);
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine(result);
            return result.Contains("处理成功");
        }

        #endregion

        public static string RemoveUnacceptableString(string text)
        {
            text = text.Replace("\\n", "");
            text = text.Replace("\\", "");
            return text;
        }
    }
}