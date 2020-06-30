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
        public static List<PregnantInfo> TempPregnantInfos = new List<PregnantInfo>();


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
                UpdateData(pregnantInfo, data);
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
select Top 10 s.id sid
,s.SyncTime
,pi.createtime,pi.updatetime
,pi.* from PregnantInfo pi
left join SyncForFS s on s.SourceType = 1 and s.SourceId = pi.Id
where s.id is not null and s.SyncStatus in (2,3)
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
        public static void MockCommitUpdatePregnantInfo2(bool isTestOne)
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
                UpdateData(pregnantInfo, data);
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
                var serviceResult = context.DelegateTransaction((Func<DbGroup, bool>)((group) =>
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
                }));

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
                        SyncStatus = SyncStatus.Error
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
                        D70 = "",//健康码
                        D71 = "",//TODO 之前模拟的时候填了别人用过的 //健康码ID
                        D3 = pregnantInfo.personname,//孕妇姓名
                        D4 = "", //孕妇国籍 对照表 2) 1)  国籍代码GB/T 2659
                        D5 = "", //孕妇民族 1)  民族代码GB/T 3304
                        D6 = "", //孕妇证件类型1)   证件类型CV02.01.101
                        D8 = "",//生日
                        D9 = "", //孕妇年龄
                        D10 = "", //孕妇文化程度 1)  文化程度STD_CULTURALDEG
                        D11 = "", //手机号码
                        D12 = "",//孕妇职业 1)  职业STD_OCCUPATION
                        D13 = "", //孕妇工作单位
                        D14 = "", //孕妇籍贯
                        D15 = "", //孕妇户籍地址 [TODO 对照表] 省2位,市2位,县/区2位,乡镇街道3位,社区/村3位
                        D16 = "", //孕妇户籍地址 [TODO 对照表]
                        D17 = "",//孕妇户籍地址 [TODO 对照表]
                        D18 = "", //孕妇户籍地址 [TODO 对照表]
                        D19 = "", //孕妇户籍地址 [TODO 对照表]
                        D20 = "", //户籍详细地址
                        D21 = "", //孕妇现住地址 [TODO 对照表]
                        D22 = "", //孕妇现住地址 [TODO 对照表]
                        D23 = "", //孕妇现住地址 [TODO 对照表]
                        D24 = "", //孕妇现住地址 [TODO 对照表]
                        D25 = "", //孕妇现住地址 [TODO 对照表]
                        D26 = "", //产后休养地址
                        D27 = "", //产后休养地址 [TODO 对照表]
                        D28 = "", //产后休养地址 [TODO 对照表]
                        D29 = "", //产后休养地址 [TODO 对照表]
                        D30 = "", //产后休养地址 [TODO 对照表]
                        D31 = "", //产后休养地址 [TODO 对照表]
                        D32 = "", //产后详细地址
                        D33 = "", //孕妇户籍类型 1)  户籍类型STD_REGISTERT2PE
                        D34 = "", //孕妇户籍分类 非户籍:2 ,户籍:1
                        D35 = "", //来本地居住时间 
                        D36 = "", //近亲结婚  [TODO 对照表]
                        D37 = "", //孕妇结婚年龄 
                        D38 = "", //丈夫结婚年龄 
                        D39 = "", //丈夫姓名
                        D40 = "", //丈夫国籍  [TODO 对照表]
                        D41 = "", //丈夫民族  [TODO 对照表]
                        D42 = "", //丈夫证件类型  [TODO 对照表]
                        D43 = "", //丈夫证件号码
                        D44 = "", //丈夫出生日期
                        D45 = "", //丈夫登记年龄
                        D46 = "", //丈夫职业  [TODO 对照表]
                        D47 = "",  //丈夫工作单位
                        D48 = "", //丈夫联系电话
                        D49 = "", //丈夫健康状况   [TODO 对照表]
                        D50 = "", //丈夫嗜好   [TODO 对照表]
                        D51 = "", //丈夫现在地址   [TODO 对照表]
                        D52 = "", //丈夫现在地址
                        D53 = "", //丈夫现在地址
                        D54 = "", //丈夫现在地址
                        D55 = "", //丈夫现在地址
                        D56 = "", //现住详细地址
                        D57 = "",
                        D62 = "", //婚姻状况  [TODO 对照表]
                        D63 = "", //医疗费用支付方式  [TODO 对照表]
                        D64 = "", //厨房排风设施 PASS
                        D65 = "", //燃料类型 PASS
                        D66 = "", //饮水 PASS
                        D67 = "", //厕所  PASS
                        D68 = "", //禽畜栏 PASS
                    };

                    #region 更新用户数据
                    UpdateData(pregnantInfo, data);
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
                #endregion
                var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Create_" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
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
select Top 100 s.id sid,
pi.createtime,
pi.* from PregnantInfo pi
left join SyncForFS s on s.SourceType = 1 and s.SourceId = pi.Id
where s.id is null and pi.createtime>'2020-06-30 09:00:00'
order by pi.createtime ", transaction: group.Transaction).ToList();
            });
            tempPregnantInfos = serviceResult.Data;
            return tempPregnantInfos;
        }

        public static void UpdateData(PregnantInfo pregnantInfo, WMH_CQBJ_JBXX_FORM_SAVEData data)
        {
            //public string D1 { set; get; } //登录用户Id
            //public string D2 { set; get; } //@保健号
            //public string D3 { set; get; } //孕妇姓名
            data.D3 = pregnantInfo.personname ?? "";
            //public string D4 { set; get; } //孕妇国籍 对照表 2) 1)  国籍代码GB/T 2659
            data.D4 = VLConstraints.GetCountry_GB_T_2659ByCountry_GB_T_2659_2000(pregnantInfo.nationalitycode) ?? "";
            //public string D5 { set; get; } //孕妇民族 1)  民族代码GB/T 3304
            data.D5 = pregnantInfo.nationcode ?? "";
            //public string D6 { set; get; } //孕妇证件类型1)   证件类型CV02.01.101
            data.D6 = VLConstraints.GetCardType_CV02_01_101ByCardType_Hele(pregnantInfo.idtype);
            //public string D7 { set; get; } //身份证
            data.D7 = pregnantInfo.idcard;
            //public string D8 { set; get; } //生日
            data.D8 = pregnantInfo.birthday?.ToString("yyyy-MM-dd");
            //public string D9 { set; get; } //孕妇年龄
            data.D9 = pregnantInfo.createage ?? "";
            //public string D10 { set; get; } //孕妇文化程度 1)  文化程度STD_CULTURALDEG
            data.D10 = VLConstraints.GetDegree_STD_CULTURALDEGByDegree_Hele(pregnantInfo.educationcode) ?? "";
            //public string D11 { set; get; } //手机号码
            data.D11 = pregnantInfo.mobilenumber ?? "";
            //public string D12 { set; get; } //孕妇职业 1)  职业STD_OCCUPATION PASS(未有在用)
            //data.D12 = VLConstraints.GetOccupation_STD_OCCUPATIONByOccupation_Hele(pregnantInfo.workcode);
            //public string D13 { set; get; } //孕妇工作单位
            data.D13 = pregnantInfo.workplace ?? "";
            //public string D14 { set; get; } //孕妇籍贯 PASS
            //public string D15 { set; get; } //孕妇户籍地址 省2位,市2位,县/区2位,乡镇街道3位,社区/村3位
            data.D15 = pregnantInfo.homeaddress.GetSubStringOrEmpty(0, 2) ?? "";
            //public string D16 { set; get; } //孕妇户籍地址
            data.D16 = pregnantInfo.homeaddress.GetSubStringOrEmpty(0, 4) ?? "";
            //public string D17 { set; get; } //孕妇户籍地址
            data.D17 = pregnantInfo.homeaddress.GetSubStringOrEmpty(0, 6) ?? "";
            //public string D18 { set; get; } //孕妇户籍地址
            data.D18 = pregnantInfo.homeaddress.GetSubStringOrEmpty(0, 9) ?? "";
            //public string D19 { set; get; } //孕妇户籍地址
            data.D19 = pregnantInfo.homeaddress ?? "";
            //public string D20 { set; get; } //户籍详细地址
            data.D20 = pregnantInfo.homeaddress_text ?? "";
            //public string D21 { set; get; } //孕妇现住地址
            data.D21 = pregnantInfo.liveplace.GetSubStringOrEmpty(0, 2) ?? "";
            //public string D22 { set; get; } //孕妇现住地址
            data.D22 = pregnantInfo.liveplace.GetSubStringOrEmpty(0, 4) ?? "";
            //public string D23 { set; get; } //孕妇现住地址
            data.D23 = pregnantInfo.liveplace.GetSubStringOrEmpty(0, 6) ?? "";
            //public string D24 { set; get; } //孕妇现住地址
            data.D24 = pregnantInfo.liveplace.GetSubStringOrEmpty(0, 9) ?? "";
            //public string D25 { set; get; } //孕妇现住地址
            data.D25 = pregnantInfo.liveplace ?? "";
            //public string D26 { set; get; } //孕妇现住地址-详细
            data.D26 = pregnantInfo.liveplace_text ?? "";
            //public string D27 { set; get; } //产后休养地址
            data.D27 = pregnantInfo.restregioncode.GetSubStringOrEmpty(0, 2) ?? "";
            //public string D28 { set; get; } //产后休养地址
            data.D28 = pregnantInfo.restregioncode.GetSubStringOrEmpty(0, 4) ?? "";
            //public string D29 { set; get; } //产后休养地址
            data.D29 = pregnantInfo.restregioncode.GetSubStringOrEmpty(0, 6) ?? "";
            //public string D30 { set; get; } //产后休养地址
            data.D30 = pregnantInfo.restregioncode.GetSubStringOrEmpty(0, 9) ?? "";
            //public string D31 { set; get; } //产后休养地址
            data.D31 = pregnantInfo.restregioncode ?? "";
            //public string D32 { set; get; } //产后详细地址
            data.D32 = pregnantInfo.restregiontext ?? "";

            //public string D33 { set; get; } //孕妇户籍类型 1)  户籍类型STD_REGISTERT2PE
            data.D33 = VLConstraints.GetRegisterType_STD_REGISTERT2PE_By_RegisterType_HELE(pregnantInfo.isagrregister) ?? "";
            //public string D34 { set; get; } //孕妇户籍分类 非户籍:2 ,户籍:1   PASS
            //public string D35 { set; get; } //来本地居住时间  PASS
            //public string D36 { set; get; } //近亲结婚        PASS
            //public string D37 { set; get; } //孕妇结婚年龄    PASS
            //public string D38 { set; get; } //丈夫结婚年龄    PASS

            //public string D39 { set; get; } //丈夫姓名
            data.D39 = pregnantInfo.husbandname ?? "";
            //public string D40 { set; get; } //丈夫国籍 PASS(无数据)
            //data.D40 = VLConstraints.GetCountry_GB_T_2659ByCountry_GB_T_2659_2000(pregnantInfo.husbandnationalitycode);
            //public string D41 { set; get; } //丈夫民族 PASS(无数据)
            //data.D41 = pregnantInfo.husbandnationcode;
            //public string D42 { set; get; } //丈夫证件类型
            data.D42 = VLConstraints.GetCardType_CV02_01_101ByCardType_Hele(pregnantInfo.husbandidtype) ?? "";
            //public string D43 { set; get; } //丈夫证件号码
            data.D43 = pregnantInfo.husbandidcard ?? "";
            //public string D44 { set; get; } //丈夫出生日期
            data.D44 = pregnantInfo.husbandbirthday?.ToString("yyyy-MM-dd") ?? "";
            //public string D45 { set; get; } //丈夫登记年龄
            data.D45 = pregnantInfo.husbandage ?? "";
            //public string D46 { set; get; } //丈夫职业  [TODO 对照表] PASS(未有在用)
            //data.D12 = VLConstraints.GetOccupation_STD_OCCUPATIONByOccupation_Hele(pregnantInfo.husbandworkcode);
            //public string D47 { set; get; }  //丈夫工作单位 PASS(未有在用)
            //public string D48 { set; get; } //丈夫联系电话
            data.D48 = pregnantInfo.husbandmobile ?? "";
            //public string D49 { set; get; } //丈夫健康状况   PASS
            //public string D50 { set; get; } //丈夫嗜好       PASS
            //public string D51 { set; get; } //丈夫现在地址 由于我方系统录入的是丈夫的户籍地址,经确认采用孕妇的现住地址
            data.D51 = pregnantInfo.liveplace.GetSubStringOrEmpty(0, 2) ?? "";
            //public string D52 { set; get; } //丈夫现在地址
            data.D52 = pregnantInfo.liveplace.GetSubStringOrEmpty(0, 4) ?? "";
            //public string D53 { set; get; } //丈夫现在地址
            data.D53 = pregnantInfo.liveplace.GetSubStringOrEmpty(0, 6) ?? "";
            //public string D54 { set; get; } //丈夫现在地址
            data.D54 = pregnantInfo.liveplace.GetSubStringOrEmpty(0, 9) ?? "";
            //public string D55 { set; get; } //丈夫现在地址
            data.D55 = pregnantInfo.liveplace ?? "";
            //public string D56 { set; get; } //现住详细地址
            data.D56 = pregnantInfo.liveplace_text ?? "";
            //public string D57 { set; get; }
            //public string D58 { set; get; } //创建时间
            //public string D59 { set; get; } //创建机构
            //public string D60 { set; get; } //创建人员
            //public string D61 { set; get; } //病案号
            //public string D62 { set; get; } //婚姻状况
            data.D62 = VLConstraints.GetMaritalStatus_STD_MARRIAGEByMaritalStatus_HELE(pregnantInfo.maritalstatuscode) ?? "";
            //public string D63 { set; get; } //医疗费用支付方式 PASS
            //public string D64 { set; get; } //厨房排风设施 PASS
            //public string D65 { set; get; } //燃料类型 PASS
            //public string D66 { set; get; } //饮水 PASS
            //public string D67 { set; get; } //厕所  PASS
            //public string D68 { set; get; } //禽畜栏 PASS
            //public string D69 { set; get; } //创建机构名称:佛山市妇幼保健院
            //public string D70 { set; get; } //健康码
            //public string D71 { set; get; } //健康码ID 
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
            data.D1 = pregnantInfo.lastmenstrualperiod?.ToString(DateFormat) ?? ""; //末次月经
            data.D2 = pregnantInfo.dateofprenatal?.ToString(DateFormat) ?? "";
            data.D3 = VLConstraints.GetPregnancyMannerByPregnancyManner_Hele(pregnantInfo.tpregnancymanner);
            data.D4 = pregnantInfo.implanttime?.ToString(DateFormat) ?? "";  //"D4":"2020-06-02", //受精或移植日期 //植入时间
            //data.D5   //"D5":"尿妊娠试验,B超检查",//妊娠确认方法
            //data.D6   //"D6":"4",//B超确认孕周
            //data.D7   //"D7":"有",//早孕反应
            //data.D8   //"D8":"5",//开始孕周
            //data.D9   //"D9":"6",//胎动开始孕周
            //data.D10  //"D10":"头痛（开始孕周    ）,//自觉症状
            data.D11 = pregnantInfo.pasthistory ?? "";
            //data.D12  //"D12":"无",//
            data.D13 = pregnantInfo.operationhistory ?? ""; //"D13":"剖宫产术,腹腔镜子宫肌瘤剔除术",//手术史
            //data.D14 = pregnantInfo.D14; //"D14":"未发现",//
            data.D15 = pregnantInfo.allergichistory ?? ""; //"D15":"青霉素,喹诺酮类",//过敏史
            data.D16 = pregnantInfo.bloodtransfution ?? ""; //"D16":"有",//输血史
            //data.D17 = pregnantInfo.D17; //"D17":"",//
            //data.D18 = pregnantInfo.familyhistory; //"D18":"地中海贫血,G6PD缺乏症",//本人家族史
            //data.D19 = pregnantInfo.D19; //"D19":"2",//亲缘关系
            //data.D20 = pregnantInfo.D20; //"D20":"地中海贫血,G6PD缺乏症",//配偶家族史
            //data.D21 = pregnantInfo.D21; //"D21":"2",//亲缘关系
            //data.D22 = pregnantInfo.D22; //"D22":"",//
            data.D23 = pregnantInfo.menarcheage ?? ""; //"D23":"11",//月经初潮年龄
            data.D24 = pregnantInfo.menstrualperiodmin + "-" + pregnantInfo.menstrualperiodmax; //"D24":"22",//月经持续时间
            data.D25 = pregnantInfo.cyclemin + "-" + pregnantInfo.cyclemax; //"D25":"33",//间歇时间
            data.D26 = pregnantInfo.menstrualblood.GetDescription() ?? ""; //"D26":"中",//经量
            data.D27 = VLConstraints.GetDysmenorrheaByDysmenorrhea_Hele(pregnantInfo.dysmenorrhea); //"D27":"有",//痛经
            //data.D28 = pregnantInfo.D28; //"D28":"",//
            //data.D29 = pregnantInfo.D29; //"D29":""，//
            //data.D30 = pregnantInfo.D30; //"D30":"风疹病毒,流感病毒",//病毒感染
            //data.D31 = pregnantInfo.D31; //"D31":"无",//发热
            data.D32 = pregnantInfo.poisontouchhis ?? ""; //"D32":"有害化学物质,有害生物物质",//接触有害物质
            //data.D33 = pregnantInfo.D33; //"D33":"2",//饮酒      //  饮酒
            //data.D34 = pregnantInfo.D34; //"D34":"2",//两/天、
            //data.D35 = pregnantInfo.D35; //"D35":"无",//服用药物  // 预防接种
            //data.D36 = pregnantInfo.D36; //"D36":"无",//服用药物
            //data.D37 = pregnantInfo.D37; //"D37":"33",//
            //data.D38 = pregnantInfo.D38; //"D38":"22",//
            //data.D39 = pregnantInfo.D39; //"D39":"",//
            //data.D40 = pregnantInfo.D40; //"D40":"",//
            //data.D41 = pregnantInfo.D41; //"D41":"2020-06-03",//早期B超时间
            //data.D42 = pregnantInfo.D42; //"D42":"1",//胎数
            //data.D43 = pregnantInfo.D43; //"D43":"2",//吸毒
            //data.D44 = pregnantInfo.D44; //"D44":"2",//
            data.D45 = pregnantInfo.eggretrievaltime?.ToString(DateFormat) ?? ""; //"D45":"2020-06-01",//取卵日期
            data.D46 = "1"; //"D46":"1",//本孕首次产检地点
            //data.D47 = pregnantInfo.D47;  //"D47":"外伤史",//外伤史
            data.D48 = pregnantInfo.heredityfamilyhistory ?? ""; //"D48":"遗传病史",//遗传病史
            //data.D49 = pregnantInfo.D49; //"D49":"2,3",//残疾情况
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

        static string DateFormat = "yyyy-MM-dd";

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

        internal static List<PhysicalExaminationData> GetPhysicalExaminationDatasForCreatePhysicalExaminations()
        {
            var context = DBHelper.GetDbContext(ConntectingStringSD);
            var serviceResult = context.DelegateTransaction((group) =>
            {
                return group.Connection.Query<PhysicalExaminationData>($@"
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

        internal static List<PhysicalExaminationData> GetPhysicalExaminationDatasForUpdatePhysicalExaminations()
        {
            var context = DBHelper.GetDbContext(ConntectingStringSD);
            var serviceResult = context.DelegateTransaction((group) =>
            {
                return group.Connection.Query<PhysicalExaminationData>($@"
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
    }
}
