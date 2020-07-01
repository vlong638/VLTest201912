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
    public class SDBLL
    {
        public static string ConntectingStringSD = "Data Source=201.201.201.89;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sdfy;Password=sdfy123456";
        public static DbContext GetDBContext { get { return DBHelper.GetDbContext(SDBLL.ConntectingStringSD); } }
        public static List<PregnantInfo> TempPregnantInfos = new List<PregnantInfo>();
        //BaseInfo baseInfo = new BaseInfo()
        //{
        //    UserId = "35000528",
        //    UserName = "廖凤贤",
        //    OrgId = "45608491-9",
        //    OrgName = "佛山市妇幼保健院",
        //};
        public static UserInfo UserInfo = new UserInfo()
        {
            UserId = "35021069",
            UserName = "赵卓姝",
            OrgId = "45608491-9",
            OrgName = "佛山市妇幼保健院",
            EncodeUserName = HttpUtility.UrlEncode("赵卓姝"),
        };

        private DbContext DBContext;

        public SDBLL(DbContext context)
        {
            this.DBContext = context;
        }

        public static void MockCommitUpdatePregnantInfo(bool isTestOne)
        {
            var isExecuteOne = false;
            var conntectingStringSD = "Data Source=201.201.201.89;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sdfy;Password=sdfy123456";
            var context = DBHelper.GetDbContext(conntectingStringSD);
            foreach (var pregnantInfo in TempPregnantInfos)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"当前孕妇:{pregnantInfo.personname}");
                if (isTestOne && isExecuteOne)
                    break;

                #region mock commit
                var container = new CookieContainer();
                var url = "";
                var postData = "";
                var result = "";
                if (pregnantInfo == null)
                    return;
                //查询孕妇 概要数据(各类Id) 基础8
                url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID={UserInfo.UserId}&sParams=P${pregnantInfo.idcard}$P$P";
                result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                var resultBase = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
                sb.AppendLine($"查询孕妇 概要数据(各类Id)");
                sb.AppendLine(result);
                if (resultBase.data.Count == 0)
                {
                    var message = $"待更新的孕妇{pregnantInfo.personname}缺少概要数据";
                    Console.WriteLine(message);
                    var syncForFSTemp = new SyncOrder()
                    {
                        SourceType = SourceType.PregnantInfo,
                        SourceId = pregnantInfo.Id.ToString(),
                        SyncTime = DateTime.Now,
                        SyncStatus = SyncStatus.Error,
                        ErrorMessage = message,
                    };
                    var serviceResultTemp = context.DelegateTransaction((group) =>
                    {
                        return group.Connection.Insert(syncForFSTemp, transaction: group.Transaction);
                    });
                    isExecuteOne = true;
                    continue;
                }
                //查询孕妇 基础77
                var baseMain = resultBase.data.First();
                url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_READ&sUserID={UserInfo.UserId}&sParams={baseMain.MainId}";
                result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                sb.AppendLine($"查询孕妇 基本数据");
                sb.AppendLine(result);
                var resultBaseInfo = result.FromJson<WMH_CQBJ_JBXX_FORM_READResponse>();
                if (resultBaseInfo.data.Count == 0)
                {
                    var message = $"待更新的孕妇{pregnantInfo.personname}缺少基本数据";
                    Console.WriteLine(message);
                    var syncForFSTemp = new SyncOrder()
                    {
                        SourceType = SourceType.PregnantInfo,
                        SourceId = pregnantInfo.Id.ToString(),
                        SyncTime = DateTime.Now,
                        SyncStatus = SyncStatus.Error,
                        ErrorMessage = message,
                    };
                    var serviceResultTemp = context.DelegateTransaction((group) =>
                    {
                        return group.Connection.Insert(syncForFSTemp, transaction: group.Transaction);
                    });
                    isExecuteOne = true;
                    continue;
                }
                #region 更新用户数据
                var jbxx = resultBaseInfo.data.First();
                var data = new WMH_CQBJ_JBXX_FORM_SAVEData(jbxx);
                data.UpdateData(pregnantInfo);
                #endregion

                //更新用户数据
                sb.AppendLine("--------------Mock Commit Start");
                url = $"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_SAVE&sUserID={UserInfo.UserId}&sParams={jbxx.MainIdForChange}${baseMain.MainId}${UserInfo.OrgId}${UserInfo.EncodeUserName}$null$null$null$%E6%99%AE%E9%80%9A%E6%8A%A4%E5%A3%AB%E4%BA%A7%E6%A3%80";
                var json = new List<WMH_CQBJ_JBXX_FORM_SAVEData>() { data }.ToJson();
                postData = "data=" + HttpUtility.UrlEncode(json);
                result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                sb.AppendLine("--------------Mock Commit End");
                sb.AppendLine("--------------pregnantInfo");
                sb.AppendLine(pregnantInfo.ToJson());
                sb.AppendLine("--------------url");
                sb.AppendLine(url);
                sb.AppendLine("--------------postData");
                sb.AppendLine(postData);
                sb.AppendLine("--------------result");
                sb.AppendLine(result);

                //新增同步记录
                SyncOrder syncForFS = new SyncOrder()
                {
                    SourceType = SourceType.PregnantInfo,
                    SourceId = pregnantInfo.Id.ToString(),
                    SyncTime = DateTime.Now,
                    SyncStatus = SyncStatus.Test
                };
                var serviceResult = context.DelegateTransaction((group) =>
                {
                    return group.Connection.Insert(syncForFS, transaction: group.Transaction);
                });

                isExecuteOne = true;
                #endregion
                var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Update_" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                File.WriteAllText(file, sb.ToString());
                Console.WriteLine($"result:{file}");
            }
        }


        public static List<PregnantInfo> GetPregnantInfosToUpdate()
        {
            List<PregnantInfo> tempPregnantInfos;
            var conntectingStringSD = "Data Source=201.201.201.89;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sdfy;Password=sdfy123456";
            var context = DBHelper.GetDbContext(conntectingStringSD);
            var serviceResult = context.DelegateTransaction((group) =>
            {
                return group.Connection.Query<PregnantInfo>(@"
select Top 10 s.id sid,pi.* from PregnantInfo pi
left join SyncForFS s on s.SourceType = 1 and s.SourceId = pi.Id
where s.id is not null and s.SyncStatus = 2 ", transaction: group.Transaction).ToList();
            });
            tempPregnantInfos = serviceResult.Data;
            foreach (var pregnantInfo in serviceResult.Data)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(pregnantInfo.ToJson());
                var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\ToUpdate_" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                File.WriteAllText(file, sb.ToString());
                Console.WriteLine($"result:{file}");
            }

            return tempPregnantInfos;
        }

        /// <summary>
        /// 一条来源记录对应一条同步记录
        /// 同步记录负责维护该记录的同步现状
        /// 不设置多条
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<PregnantInfo> GetPregnantInfosToUpdate2()
        {
            List<PregnantInfo> tempPregnantInfos;
            var conntectingStringSD = "Data Source=201.201.201.89;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sdfy;Password=sdfy123456";
            var context = DBHelper.GetDbContext(conntectingStringSD);
            var serviceResult = context.DelegateTransaction((group) =>
            {
                return group.Connection.Query<PregnantInfo>(@"
select Top 1 s.id sid
,s.SyncTime
,pi.createtime,pi.updatetime
,pi.* from PregnantInfo pi
left join SyncForFS s on s.SourceType = 1 and s.SourceId = pi.Id
where s.id is not null and s.SyncStatus in (2,11)
and pi.updatetime > DATEADD( SECOND,10 ,s.SyncTime)
", transaction: group.Transaction).ToList();
            });
            tempPregnantInfos = serviceResult.Data;
            foreach (var pregnantInfo in serviceResult.Data)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(pregnantInfo.ToJson());
                var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\ToUpdate_" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                File.WriteAllText(file, sb.ToString());
                Console.WriteLine($"result:{file}");
            }

            return tempPregnantInfos;
        }
        public static void MockCommitUpdatePregnantInfo2()
        {
            var isExecuteOne = false;
            var conntectingStringSD = "Data Source=201.201.201.89;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sdfy;Password=sdfy123456";
            var context = DBHelper.GetDbContext(conntectingStringSD);
            foreach (var pregnantInfo in TempPregnantInfos)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"当前孕妇:{pregnantInfo.personname}");

                #region mock commit
                var container = new CookieContainer();
                var url = "";
                var postData = "";
                var result = "";
                if (pregnantInfo == null)
                    return;
                //查询孕妇 概要数据(各类Id) 基础8
                url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID={UserInfo.UserId}&sParams=P${pregnantInfo.idcard}$P$P";
                result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                var resultBase = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
                sb.AppendLine($"查询孕妇 概要数据(各类Id)");
                sb.AppendLine(result);
                if (resultBase.data.Count == 0)
                {
                    var message = $"待更新的孕妇{pregnantInfo.personname}缺少概要数据";
                    Console.WriteLine(message);
                    var serviceResultTemp = context.DelegateTransaction((group) =>
                    {
                        var syncForFS = GetSyncOrderBySource(group, SourceType.PregnantInfo, pregnantInfo.Id.ToString()).First();
                        syncForFS.SyncTime = DateTime.Now;
                        syncForFS.SyncStatus = SyncStatus.Error;
                        syncForFS.ErrorMessage = message;
                        return group.Connection.Update(syncForFS, transaction: group.Transaction);
                    });
                    isExecuteOne = true;
                    continue;
                }
                //查询孕妇 基础77
                var baseMain = resultBase.data.First();
                url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_READ&sUserID={UserInfo.UserId}&sParams={baseMain.MainId}";
                result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                sb.AppendLine($"查询孕妇 基本数据");
                sb.AppendLine(result);
                var resultBaseInfo = result.FromJson<WMH_CQBJ_JBXX_FORM_READResponse>();
                if (resultBaseInfo.data.Count == 0)
                {
                    var message = $"待更新的孕妇{pregnantInfo.personname}缺少基本数据";
                    Console.WriteLine(message);
                    var serviceResultTemp = context.DelegateTransaction((group) =>
                    {
                        var syncForFS = GetSyncOrderBySource(group, SourceType.PregnantInfo, pregnantInfo.Id.ToString()).First();
                        syncForFS.SyncTime = DateTime.Now;
                        syncForFS.SyncStatus = SyncStatus.Error;
                        syncForFS.ErrorMessage = message;
                        return group.Connection.Update(syncForFS, transaction: group.Transaction);
                    });
                    isExecuteOne = true;
                    continue;
                }
                #region 更新用户数据
                var jbxx = resultBaseInfo.data.First();
                var data = new WMH_CQBJ_JBXX_FORM_SAVEData(jbxx);
                data.UpdateData(pregnantInfo);
                #endregion

                //更新用户数据
                sb.AppendLine("--------------Mock Commit Start");
                url = $"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_SAVE&sUserID={UserInfo.UserId}&sParams={jbxx.MainIdForChange}${baseMain.MainId}${UserInfo.OrgId}${UserInfo.EncodeUserName}$null$null$null$%E6%99%AE%E9%80%9A%E6%8A%A4%E5%A3%AB%E4%BA%A7%E6%A3%80";
                var json = new List<WMH_CQBJ_JBXX_FORM_SAVEData>() { data }.ToJson();
                postData = "data=" + HttpUtility.UrlEncode(json);
                result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                sb.AppendLine("--------------Mock Commit End");
                sb.AppendLine("--------------pregnantInfo");
                sb.AppendLine(pregnantInfo.ToJson());
                sb.AppendLine("--------------url");
                sb.AppendLine(url);
                sb.AppendLine("--------------postData");
                sb.AppendLine(postData);
                sb.AppendLine("--------------result");
                sb.AppendLine(result);
                var serviceResult = context.DelegateTransaction((group) =>
                {
                    var syncForFS = GetSyncOrderBySource(group, SourceType.PregnantInfo, pregnantInfo.Id.ToString()).First();
                    if (result.Contains("处理成功"))
                    {
                        syncForFS.SyncTime = DateTime.Now;
                    }
                    else
                    {
                        syncForFS.SyncTime = DateTime.Now;
                        syncForFS.SyncStatus = SyncStatus.Error;
                        syncForFS.ErrorMessage = result;
                    }
                    return group.Connection.Update(syncForFS, transaction: group.Transaction);
                });

                isExecuteOne = true;
                #endregion
                var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Update_" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                File.WriteAllText(file, sb.ToString());
                Console.WriteLine($"result:{file}");
            }
        }

        public static IEnumerable<SyncOrder> GetSyncOrderBySource(DbGroup group, SourceType sourceType, string sourceId)
        {
            return group.Connection.Query<SyncOrder>("select * from SyncForFS where SourceType = @SourceType and SourceId = @SourceId", new { SourceType = sourceType, SourceId = sourceId }, transaction: group.Transaction);
        }

        public static WCQBJ_CZDH_DOCTOR_READResponse GetBase8(UserInfo baseInfo, string idcard, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID={baseInfo.UserId}&sParams=P${idcard}$P$P";
            var postData = "";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            var re = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
            logger.AppendLine($"查询-Base8");
            logger.AppendLine(url);
            logger.AppendLine(result);
            return re;
        }


        /// <summary>
        /// 查询-问询病史
        /// </summary>
        /// <param name="tempPregnantInfos"></param>
        public static MQDA_READ_NEWResponse GetEnquiry(UserInfo baseInfo, WCQBJ_CZDH_DOCTOR_READResponse base8, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/MQDA_READ_NEW&sUserID={baseInfo.UserId}&sParams={base8.MainId}${baseInfo.OrgId}";
            var postData = "";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            var re = result.FromJson<MQDA_READ_NEWResponse>();
            logger.AppendLine($"查询-问询病史");
            logger.AppendLine(url);
            logger.AppendLine(result);
            return re;
        }

        /// <summary>
        /// 查询-问询病史-生育史
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="base8"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        internal static WMH_CQBJ_CQJC_PRE_READ GetEnquiryPregnanths(UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READResponse base8, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_PRE_READ&sUserID={userInfo.UserId}&sParams={base8.MainId}";
            var postData = "pageIndex=0&pageSize=1000&sortField=&sortOrder=";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            var re = result.FromJson<WMH_CQBJ_CQJC_PRE_READ>();
            logger.AppendLine($"查询-问询病史-生育史");
            logger.AppendLine(url);
            logger.AppendLine(result);
            return re;
        }

        public static void MockCommitCreatePregnantInfo(List<PregnantInfo> tempPregnantInfos)
        {
            var conntectingStringSD = "Data Source=201.201.201.89;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sdfy;Password=sdfy123456";
            var context = DBHelper.GetDbContext(conntectingStringSD);
            foreach (var pregnantInfo in tempPregnantInfos)
            {
                StringBuilder sb = new StringBuilder();
                #region mock commit
                try
                {
                    var container = new CookieContainer();
                    var userId = UserInfo.UserId;
                    var userName = UserInfo.UserName;
                    var encodeUserName = UserInfo.EncodeUserName;
                    var orgId = UserInfo.OrgId;
                    var orgName = UserInfo.OrgName;
                    var url = "";
                    var postData = "";
                    var result = "";
                    if (pregnantInfo == null)
                        return;
                    //孕妇是否已存在
                    url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID={userId}&sParams=P${pregnantInfo.idcard}$P$P";
                    result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                    var re = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
                    sb.AppendLine(result);
                    if (re.data.Count != 0)
                    {
                        Console.WriteLine($"孕妇{pregnantInfo.personname}已存在");
                        SyncOrder syncForFS = new SyncOrder()
                        {
                            SourceType = SourceType.PregnantInfo,
                            SourceId = pregnantInfo.Id.ToString(),
                            SyncTime = DateTime.Now,
                            SyncStatus = SyncStatus.Existed
                        };
                        var serviceResult = context.DelegateTransaction((group) =>
                        {
                            return group.Connection.Insert(syncForFS, transaction: group.Transaction);
                        });
                        continue;
                    }
                    else
                    {
                        //Create 患者主索引
                        url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_QHJC_ID_GET&sUserID={userId}&sParams=1";
                        postData = "";
                        result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                        var mainId = result.FromJsonToAnonymousType(new { id = "" }).id;
                        Console.WriteLine($"当前孕妇:{pregnantInfo.personname},IdCard:{pregnantInfo.idcard}");
                        Console.WriteLine($"mainId:{mainId}");
                        sb.AppendLine($"当前孕妇:{pregnantInfo.personname},IdCard:{pregnantInfo.idcard},Phone:{pregnantInfo.mobilenumber}");
                        sb.AppendLine("Create 患者主索引");
                        sb.AppendLine(url);
                        sb.AppendLine($"mainId:{mainId}");
                        //Create 保健号
                        url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=CDH_GET_ID1&sUserID={userId}&sParams={orgId}";
                        postData = "";
                        result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                        var careId = result.FromJsonToAnonymousType(new { id = "" }).id;
                        Console.WriteLine($"careId:{careId}");
                        sb.AppendLine("Create 保健号");
                        sb.AppendLine(url);
                        sb.AppendLine($"careId:{careId}");
                        var careIdL8 = careId.Substring(8);
                        //--------查重
                        url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_CC&sUserID={userId}&sParams={mainId}$P${pregnantInfo.idcard}&pageSize=10000&pageIndex=0";
                        postData = "";
                        result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                        sb.AppendLine("--------查重");
                        sb.AppendLine(result);
                        //Create 基本信息
                        url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_SAVE&sUserID={userId}&sParams=null${mainId}${orgId}${encodeUserName}$null$null$null$%E6%99%AE%E9%80%9A%E6%8A%A4%E5%A3%AB%E4%BA%A7%E6%A3%80";
                        var datas = new List<WMH_CQBJ_JBXX_FORM_SAVEData>();
                        var data = new WMH_CQBJ_JBXX_FORM_SAVEData()
                        {
                            D1 = careIdL8, //@保健号后8位
                            D2 = careId, //@保健号
                            D7 = pregnantInfo.idcard,   //身份证              
                            D58 = DateTime.Now.ToString("yyyy-MM-dd"),//创建时间
                            curdate1 = DateTime.Now.ToString("yyyy-MM-dd"),
                            D59 = orgId,//创建机构Id
                            D60 = userName, //创建人员
                            D61 = null,//病案号
                            D69 = orgName, //创建机构名称:佛山市妇幼保健院
                            D3 = pregnantInfo.personname,//孕妇姓名
                        };

                        #region 更新用户数据
                        data.UpdateData(pregnantInfo);
                        #endregion

                        datas.Add(data);
                        var json = datas.ToJson();
                        postData = "data=" + HttpUtility.UrlEncode(json);
                        result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                        sb.AppendLine("Create 基本信息");
                        sb.AppendLine(url);
                        sb.AppendLine(json);
                        sb.AppendLine(result);


                        //孕妇是否已存在
                        url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID={userId}&sParams=P${pregnantInfo.idcard}$P$P";
                        result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                        re = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
                        var syncStatus = SyncStatus.Success;
                        var message = "";
                        if (re.data.Count == 0)
                        {
                            syncStatus = SyncStatus.Error;
                            message = "基本数据未成功创建";
                        }
                        SyncOrder syncForFS = new SyncOrder()
                        {
                            SourceType = SourceType.PregnantInfo,
                            SourceId = pregnantInfo.Id.ToString(),
                            SyncTime = DateTime.Now,
                            SyncStatus = syncStatus,
                            ErrorMessage = message,
                        };
                        sb.AppendLine("--------------syncForFS");
                        sb.AppendLine(syncStatus.ToString());
                        sb.AppendLine(message);

                        var serviceResult = context.DelegateTransaction((group) =>
                        {
                            return group.Connection.Insert(syncForFS, transaction: group.Transaction);
                        });
                    }
                }
                catch (Exception ex)
                {
                    SyncOrder syncForFS = new SyncOrder()
                    {
                        SourceType = SourceType.PregnantInfo,
                        SourceId = pregnantInfo.Id.ToString(),
                        SyncTime = DateTime.Now,
                        SyncStatus = SyncStatus.Error,
                        ErrorMessage = ex.ToString(),
                    };
                    sb.AppendLine(ex.ToString());
                }
                #endregion
                var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Create-基本信息-" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                File.WriteAllText(file, sb.ToString());
                Console.WriteLine($"result:{file}");
            }
        }

        public static List<PregnantInfo> GetPregnantInfoForCreateBefore0630()
        {
            List<PregnantInfo> tempPregnantInfos;
            var conntectingStringSD = "Data Source=201.201.201.89;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sdfy;Password=sdfy123456";
            var context = DBHelper.GetDbContext(conntectingStringSD);
            var serviceResult = context.DelegateTransaction((group) =>
            {
                //                return group.Connection.Query<PregnantInfo>(@"
                //select Top 100 s.id sid,pi.* from PregnantInfo pi
                //left join SyncForFS s on s.SourceType = 1 and s.SourceId = pi.Id
                //where s.id is null ", transaction: group.Transaction).ToList();

                //0623 同步上线测试 以9点以后的数据测试
                return group.Connection.Query<PregnantInfo>(@"
select Top 100 s.id sid,
pi.createtime,
pi.* from PregnantInfo pi
left join SyncForFS s on s.SourceType = 1 and s.SourceId = pi.Id
where s.id is null and pi.createtime<'2020-06-30 09:00:00'
order by pi.createtime ", transaction: group.Transaction).ToList();
            });
            tempPregnantInfos = serviceResult.Data;
            return tempPregnantInfos;
        }

        public static List<PregnantInfo> GetPregnantInfoForCreate()
        {
            List<PregnantInfo> tempPregnantInfos;
            var conntectingStringSD = "Data Source=201.201.201.89;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sdfy;Password=sdfy123456";
            var context = DBHelper.GetDbContext(conntectingStringSD);
            var serviceResult = context.DelegateTransaction((group) =>
            {
                //                return group.Connection.Query<PregnantInfo>(@"
                //select Top 100 s.id sid,pi.* from PregnantInfo pi
                //left join SyncForFS s on s.SourceType = 1 and s.SourceId = pi.Id
                //where s.id is null ", transaction: group.Transaction).ToList();

                //0623 同步上线测试 以9点以后的数据测试
                return group.Connection.Query<PregnantInfo>(@"
select Top 1 s.id sid,
pi.createtime,
pi.* from PregnantInfo pi
left join SyncForFS s on s.SourceType = 1 and s.SourceId = pi.Id
where s.id is null and pi.createtime>'2020-06-30 09:00:00'
order by pi.createtime ", transaction: group.Transaction).ToList();
            });
            tempPregnantInfos = serviceResult.Data;
            return tempPregnantInfos;
        }

        internal static List<PregnantInfo> GetPregnantInfosToCreateEnquiries()
        {
            var context = DBHelper.GetDbContext(ConntectingStringSD);
            var serviceResult = context.DelegateTransaction((group) =>
            {
                return group.Connection.Query<PregnantInfo>(@"
select Top 1 sp.id spid,se.id seid,pi.* from PregnantInfo pi
left join SyncForFS sp on sp.SourceType = 1 and sp.SourceId = pi.Id
left join SyncForFS se on se.SourceType = 2 and se.SourceId = pi.Id
where sp.id is not null and sp.SyncStatus = 2 
and se.id is null
", transaction: group.Transaction).ToList();
            });
            return serviceResult.Data;
        }
        internal static List<PregnantInfo> GetPregnantInfosToUpdateEnquiries()
        {
            var context = DBHelper.GetDbContext(ConntectingStringSD);
            var serviceResult = context.DelegateTransaction((group) =>
            {
                return group.Connection.Query<PregnantInfo>(@"
                select Top 1 se.id seid,se.SyncTime
                ,pi.* from PregnantInfo pi
                left join SyncForFS se on se.SourceType = 2 and se.SourceId = pi.Id
                where se.id is not null 
				and se.SyncStatus = 2 
				and pi.updatetime > DATEADD( SECOND,10 ,se.SyncTime)
                ", transaction: group.Transaction).ToList();
            });
            return serviceResult.Data;
        }

        internal static List<PregnantInfo> GetPregnantInfosForTest()
        {
            var context = DBHelper.GetDbContext(ConntectingStringSD);
            var serviceResult = context.DelegateTransaction((group) =>
            {
                return group.Connection.Query<PregnantInfo>(@"
                select Top 2 pi.* from PregnantInfo pi
                where pi.idcard ='142328199610271518'
                ", transaction: group.Transaction).ToList();
            });
            return serviceResult.Data;
        }

        public static SyncOrder GetSyncOrder(DbGroup group, SourceType sourceType, string sourceId)
        {
            return SDDAL.GetSyncForFS(group, sourceType, sourceId);
        }

        public static long SaveSyncOrder(DbGroup group, SyncOrder syncForFS)
        {
            if (syncForFS.Id > 0)
            {
                SDDAL.UpdateSyncForFS(group, syncForFS);
                return syncForFS.Id;
            }
            else
            {
                return SDDAL.InsertSyncForFS(group, syncForFS);
            }
        }

        public static void PostCreateEnquiryToFS(PregnantInfo enquiry, MQDA_READ_NEWData mQDA_READ_NEWData, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READResponse base8, ref StringBuilder logger)
        {
            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/MQDA_XWBS_SAVE&sUserID={userInfo.UserId}&sParams={base8.MainId}${userInfo.OrgId}";
            var datas = new List<WMH_CQBJ_JBXX_FORM_SAVEData>();
            var data = new WMH_CQBJ_JBXX_FORM_SAVEData()
            {
            };

            datas.Add(data);
            var json = datas.ToJson();
            var container = new CookieContainer();
            var postData = "data=" + HttpUtility.UrlEncode(json);
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine("Create 问询病史");
            logger.AppendLine(url);
            logger.AppendLine(json);
            logger.AppendLine(result);
        }

        public static string PostUpdateEnquiryToFS(PregnantInfo pregnantInfo, MQDA_READ_NEWData enquiryData, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READResponse base8, ref StringBuilder logger)
        {
            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/MQDA_XWBS_SAVE&sUserID={userInfo.UserId}&sParams={base8.MainId}${userInfo.OrgId}";
            var datas = new List<Data_MQDA_XWBS_SAVE>();
            #region datas
            var data = new Data_MQDA_XWBS_SAVE(enquiryData);
            data.Update(pregnantInfo);
            datas.Add(data);
            #endregion
            var json = datas.ToJson();
            var container = new CookieContainer();
            var postData = "data=" + HttpUtility.UrlEncode(json);
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine("Update 问询病史");
            logger.AppendLine(url);
            logger.AppendLine(json);
            logger.AppendLine(result);
            return result;
        }

        internal static string PostAddEnquiryPregnanth(WMH_CQBJ_CQJC_PRE_SAVE toAdd, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READResponse base8, ref StringBuilder logger)
        {
            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_PRE_SAVE&sUserID={userInfo.UserId}&sParams={base8.MainId}${userInfo.OrgId}";
            var datas = new List<WMH_CQBJ_CQJC_PRE_SAVE>();
            datas.Add(toAdd);
            var json = datas.ToJson();
            var container = new CookieContainer();
            var postData = "data=" + HttpUtility.UrlEncode(json);
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine("Create 问询病史-生育史");
            logger.AppendLine(url);
            logger.AppendLine(json);
            logger.AppendLine(result);
            return result;
        }


        internal static string DeleteEnquiryPregnanth(WMH_CQBJ_CQJC_PRE_SAVE toChange, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READResponse base8, ref StringBuilder logger)
        {

            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_SYS_DELETE&sUserID={userInfo.UserId}&sParams=1$1";
            var datas = new List<WMH_CQBJ_CQJC_PRE_SAVE>();
            datas.Add(toChange);
            var json = datas.ToJson();
            var container = new CookieContainer();
            var postData = "data=" + HttpUtility.UrlEncode(json);
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine("Update 问询病史-生育史");
            logger.AppendLine(url);
            logger.AppendLine(json);
            logger.AppendLine(result);
            return result;
        }

        internal static string UpdateEnquiryPregnanth(WMH_CQBJ_CQJC_PRE_SAVE toChange, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READResponse base8, ref StringBuilder logger)
        {
            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_PRE_SAVE&sUserID={userInfo.UserId}&sParams={base8.MainId}${userInfo.OrgId}";
            var datas = new List<WMH_CQBJ_CQJC_PRE_SAVE>();
            datas.Add(toChange);
            var json = datas.ToJson();
            var container = new CookieContainer();
            var postData = "data=" + HttpUtility.UrlEncode(json);
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine("Update 问询病史-生育史");
            logger.AppendLine(url);
            logger.AppendLine(json);
            logger.AppendLine(result);
            return result;
        }

        internal static string GetPhysicalExaminationId(UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READResponse base8, DateTime issueDate, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var dateStr = issueDate.ToString("yyyy-MM-dd");
            //获取体格检查Id
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

        internal static WMH_CQBJ_CQJC_TGJC_NEW_READ_Data GetPhysicalExamination(string tgjcId, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READResponse base8, DateTime issueDate, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            //查询体格检查详情
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_TGJC_NEW_READ&sUserID={userInfo.UserId}&sParams={base8.MainId}${tgjcId}";
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

        internal static List<PhysicalExaminationModel> GetPhysicalExaminationDatasForCreatePhysicalExaminations()
        {
            var context = DBHelper.GetDbContext(ConntectingStringSD);
            var serviceResult = context.DelegateTransaction((group) =>
            {
                return group.Connection.Query<PhysicalExaminationModel>($@"
select 
T1.*
,pi_data.personname pi_personname
,pi_data.weight pi_weight
,pi_data.height pi_height
,pi_data.bmi pi_bmi
,vr_data.Id
,vr_data.weight
,vr_data.temperature
,vr_data.heartrate
,vr_data.dbp
,vr_data.sbp
from 
(
		SELECT 
		vr.idcard
		,max(vr.visitdate) lastestvisitdate
		,min(vr.visitdate) firstvisitdate		
		FROM (
				SELECT top 1
				pi.idcard
				FROM PregnantInfo pi 
				LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
				left join SyncForFS sp on sp.SourceType = 1 and sp.SourceId = pi.Id
				left join SyncForFS se on se.SourceType = 3 and se.SourceId = vr.Id			
				where sp.id is not null and sp.SyncStatus in (2,11) 
				and se.id is null 
				and vr.visitdate = convert(nvarchar,getdate(),23)
		)  as toCreate 
		LEFT JOIN MHC_VisitRecord vr on toCreate.idcard = vr.idcard 
		GROUP BY vr.idcard
) as T1
left join PregnantInfo pi_data on pi_data.idcard = T1.idcard
left join MHC_VisitRecord vr_data on vr_data.idcard = T1.idcard and vr_data.visitdate = T1.lastestvisitdate
", transaction: group.Transaction).ToList();
            });
            return serviceResult.Data;
        }

        internal static List<PhysicalExaminationModel> GetPhysicalExaminationDatasForUpdatePhysicalExaminations()
        {
            var context = DBHelper.GetDbContext(ConntectingStringSD);
            var serviceResult = context.DelegateTransaction((group) =>
            {
                return group.Connection.Query<PhysicalExaminationModel>($@"
select 
T1.*
,pi_data.personname pi_personname
,pi_data.weight pi_weight
,pi_data.height pi_height
,pi_data.bmi pi_bmi
,vr_data.Id
,vr_data.weight
,vr_data.temperature
,vr_data.heartrate
,vr_data.dbp
,vr_data.sbp
from 
(
		SELECT 
		vr.idcard
		,max(vr.visitdate) lastestvisitdate
		,min(vr.visitdate) firstvisitdate		
		FROM (
				SELECT top 10
				pi.idcard
				FROM PregnantInfo pi 
				LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
				left join SyncForFS se on se.SourceType = 3 and se.SourceId = vr.Id			
				where se.id is not null and se.SyncStatus in (2,11)
				and vr.updatetime > DATEADD( SECOND,10 ,se.SyncTime)
				and vr.visitdate = convert(nvarchar,getdate(),23)
		)  as toCreate 
		LEFT JOIN MHC_VisitRecord vr on toCreate.idcard = vr.idcard 
		GROUP BY vr.id,vr.idcard
) as T1
left join PregnantInfo pi_data on pi_data.idcard = T1.idcard
left join MHC_VisitRecord vr_data on vr_data.idcard = T1.idcard and vr_data.visitdate = T1.lastestvisitdate
                ", transaction: group.Transaction).ToList();
            });
            return serviceResult.Data;
        }

        internal static string CreatePhysicalExamination(List<WMH_CQBJ_CQJC_TGJC_NEW_SAVE_Data> datas, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READResponse base8, ref StringBuilder logger)
        {
            //Create 体格检查索引ID
            var container = new CookieContainer();
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_QHJC_ID_GET&sUserID={userInfo.UserId}&sParams=1";
            var postData = "";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            var newId = result.FromJsonToAnonymousType(new { id = "" }).id;
            if (string.IsNullOrEmpty(newId))
                throw new NotImplementedException("无效的ID");

            //创建提个检查
            url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_TGJC_NEW_SAVE&sUserID={userInfo.UserId}&sParams={userInfo.OrgId}${base8.MainId}$null${newId}";
            var json = datas.ToJson();
            postData = "data=" + HttpUtility.UrlEncode(json);
            result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine("Create 体格检查");
            logger.AppendLine(url);
            logger.AppendLine(json);
            logger.AppendLine(result);
            return result;
        }

        internal static string UpdatePhysicalExamination(string physicalExaminationId, List<WMH_CQBJ_CQJC_TGJC_NEW_SAVE_Data> datas, UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READResponse base8, ref StringBuilder logger)
        {
            //Create 体格检查索引ID
            var container = new CookieContainer();
            //创建提个检查
            var url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_CQJC_TGJC_NEW_SAVE&sUserID={userInfo.UserId}&sParams={userInfo.OrgId}${base8.MainId}$null${physicalExaminationId}";
            var json = datas.ToJson();
            var postData = "data=" + HttpUtility.UrlEncode(json);
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine("Update 体格检查");
            logger.AppendLine(url);
            logger.AppendLine(json);
            logger.AppendLine(result);
            return result;
        }

        internal static List<PhysicalExaminationModel> GetSourceDataForCreateProfessionalExaminations()
        {
            var context = DBHelper.GetDbContext(ConntectingStringSD);
            var serviceResult = context.DelegateTransaction((group) =>
            {
                return group.Connection.Query<PhysicalExaminationModel>($@"
select 
T1.*
,pi_data.personname pi_personname
,pi_data.weight pi_weight
,pi_data.height pi_height
,pi_data.bmi pi_bmi
,vr_data.Id
,vr_data.weight
,vr_data.temperature
,vr_data.heartrate
,vr_data.dbp
,vr_data.sbp
from 
(
		SELECT 
		vr.idcard
		,max(vr.visitdate) lastestvisitdate
		,min(vr.visitdate) firstvisitdate		
		FROM (
				SELECT top 1
				pi.idcard
				FROM PregnantInfo pi 
				LEFT JOIN MHC_VisitRecord vr on pi.idcard = vr.idcard 
				left join SyncForFS sp on sp.SourceType = 1 and sp.SourceId = pi.Id
				left join SyncForFS se on se.SourceType = 3 and se.SourceId = vr.Id			
				where sp.id is not null and sp.SyncStatus in (2,11) 
				and se.id is null 
				and vr.visitdate = convert(nvarchar,getdate(),23)
		)  as toCreate 
		LEFT JOIN MHC_VisitRecord vr on toCreate.idcard = vr.idcard 
		GROUP BY vr.idcard
) as T1
left join PregnantInfo pi_data on pi_data.idcard = T1.idcard
left join MHC_VisitRecord vr_data on vr_data.idcard = T1.idcard and vr_data.visitdate = T1.lastestvisitdate
", transaction: group.Transaction).ToList();
            });
            return serviceResult.Data;
        }
    }
}
