using Dapper;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.HttpSolution;
using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace FrameworkTest.Business.SDMockCommit
{
    public class PregnantInfo_SyncTask_CreateOrUpdate : SyncTask<PregnantInfo_SourceData>
    {
        public PregnantInfo_SyncTask_CreateOrUpdate(ServiceContext context) : base(context)
        {
        }

        public override List<PregnantInfo_SourceData> GetSourceDatas(UserInfo userInfo)
        {
            return Context.SDService.GetPregnantInfoForCreateOrUpdate().Select(c => new PregnantInfo_SourceData(c)).ToList();
        }

        public override void DoWork(ServiceContext context, UserInfo userInfo, PregnantInfo_SourceData sourceData)
        {
            var syncOrder = new SyncOrder()
            {
                SourceId = sourceData.SourceId,
                SourceType = sourceData.SourceType,
                SyncTime = DateTime.Now,
                SyncStatus = SyncStatus.Success,
            };
            StringBuilder logger = new StringBuilder();
            try
            {
                var container = new CookieContainer();
                var postData = "";
                var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID={userInfo.UserId}&sParams=P${sourceData.IdCard}$P$P";
                var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                var re = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
                logger.AppendLine(url);
                logger.AppendLine(result);
                if (re.data.Count != 0)//已存在 更新分支
                {


                }
                else //新建分支
                {
                    //获取 患者主索引
                    var mainId = Context.FSService.GetMainId(userInfo, ref logger);
                    if (string.IsNullOrEmpty(mainId))
                    {
                        syncOrder.SyncStatus = SyncStatus.Error;
                        syncOrder.ErrorMessage = "未获取到 患者主索引";
                        context.SDService.SaveSyncOrder(syncOrder);
                        return;
                    }
                    //获取 CareId
                    int errorCount = 0;
                    int maxErrorCount = 5;
                    while (errorCount < maxErrorCount)
                    {
                        //Create 保健号
                        var careId = context.FSService.GetCareId(userInfo, ref logger);
                        if (string.IsNullOrEmpty(careId))
                        {
                            syncOrder.SyncStatus = SyncStatus.Error;
                            syncOrder.ErrorMessage = "未获取到 保健号";
                            context.SDService.SaveSyncOrder(syncOrder);
                            return;
                        }
                        ////保健号查重
                        //var careIdL8 = careId.Substring(8);
                        //var isRepeat = context.FSService.CheckExist();
                        //if (isRepeat)
                        //{
                        //    errorCount++;
                        //    continue;
                        //}


                        break;
                    }


                }


                //var conntectingStringSD = "Data Source=201.201.201.89;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sdfy;Password=sdfy123456";
                //var context = DBHelper.GetDbContext(conntectingStringSD);
                //foreach (var pregnantInfo in tempPregnantInfos)
                //{
                //    StringBuilder sb = new StringBuilder();
                //    #region mock commit
                //    try
                //    {
                //        var container = new CookieContainer();
                //        var userId = UserInfo.UserId;
                //        var userName = UserInfo.UserName;
                //        var encodeUserName = UserInfo.EncodeUserName;
                //        var orgId = UserInfo.OrgId;
                //        var orgName = UserInfo.OrgName;
                //        var url = "";
                //        var postData = "";
                //        var result = "";
                //        if (pregnantInfo == null)
                //            return;
                //        //孕妇是否已存在
                //        url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID={userId}&sParams=P${pregnantInfo.idcard}$P$P";
                //        result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                //        var re = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
                //        sb.AppendLine(result);
                //        if (re.data.Count != 0)
                //        {
                //            Console.WriteLine($"孕妇{pregnantInfo.personname}已存在");
                //            SyncOrder syncForFS = new SyncOrder()
                //            {
                //                SourceType = SourceType.PregnantInfo,
                //                SourceId = pregnantInfo.Id.ToString(),
                //                SyncTime = DateTime.Now,
                //                SyncStatus = SyncStatus.Existed
                //            };
                //            var serviceResult = context.DelegateTransaction((group) =>
                //            {
                //                return group.Connection.Insert(syncForFS, transaction: group.Transaction);
                //            });
                //            continue;
                //        }
                //        else
                //        {
                //            //Create 患者主索引
                //            url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_QHJC_ID_GET&sUserID={userId}&sParams=1";
                //            postData = "";
                //            result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                //            var mainId = result.FromJsonToAnonymousType(new { id = "" }).id;
                //            Console.WriteLine($"当前孕妇:{pregnantInfo.personname},IdCard:{pregnantInfo.idcard}");
                //            //Console.WriteLine($"mainId:{mainId}");
                //            sb.AppendLine($"当前孕妇:{pregnantInfo.personname},IdCard:{pregnantInfo.idcard},Phone:{pregnantInfo.mobilenumber}");
                //            sb.AppendLine("Create 患者主索引");
                //            sb.AppendLine(url);
                //            sb.AppendLine($"mainId:{mainId}");
                //            int errorCount = 0;
                //            int maxErrorCount = 5;
                //            string careId = "";
                //            string careIdL8 = "";
                //            while (errorCount < maxErrorCount)
                //            {
                //                //Create 保健号
                //                url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=CDH_GET_ID1&sUserID={userId}&sParams={orgId}";
                //                postData = "";
                //                result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                //                careId = result.FromJsonToAnonymousType(new { id = "" }).id;
                //                //Console.WriteLine($"careId:{careId}");
                //                sb.AppendLine("Create 保健号");
                //                sb.AppendLine(url);
                //                sb.AppendLine($"careId:{careId}");
                //                careIdL8 = careId.Substring(8);
                //                //--------查重
                //                url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_CC&sUserID={userId}&sParams={mainId}$P${careId}${pregnantInfo.idcard}&pageSize=10000&pageIndex=0";
                //                postData = "";
                //                result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                //                var repeatData = result.FromJson<WMH_CQBJ_JBXX_FORM_CC>();
                //                if (repeatData.data.Count != 0 && repeatData.data.FirstOrDefault(c => c.PersonName != pregnantInfo.personname) != null)
                //                {
                //                    sb.AppendLine($"--------查重时,出现重复,第{errorCount}次尝试");
                //                    sb.AppendLine(result);
                //                    errorCount++;
                //                    continue;
                //                }
                //                break;
                //            }
                //            if (errorCount == maxErrorCount)
                //            {
                //                Console.WriteLine($"孕妇{pregnantInfo.personname}查重时异常");
                //                context.DelegateTransaction((group) =>
                //                {
                //                    return group.Connection.Insert(new SyncOrder()
                //                    {
                //                        SourceType = SourceType.PregnantInfo,
                //                        SourceId = pregnantInfo.Id.ToString(),
                //                        SyncTime = DateTime.Now,
                //                        SyncStatus = SyncStatus.Repeated,
                //                    }, transaction: group.Transaction);
                //                });
                //                continue;
                //            }
                //            else
                //            {
                //                sb.AppendLine($"--------查重通过");
                //            }
                //            //Create 基本信息
                //            url = $@"http://19.130.211.1:8090/FSFY/disPatchJson?&clazz=READDATA&UITYPE=WCQBJ/WMH_CQBJ_JBXX_FORM_SAVE&sUserID={userId}&sParams=null${mainId}${orgId}${encodeUserName}$null$null$null$%E6%99%AE%E9%80%9A%E6%8A%A4%E5%A3%AB%E4%BA%A7%E6%A3%80";
                //            var datas = new List<WMH_CQBJ_JBXX_FORM_SAVEData>();
                //            var data = new WMH_CQBJ_JBXX_FORM_SAVEData()
                //            {
                //                D1 = careIdL8, //@保健号后8位
                //                D2 = careId, //@保健号
                //                D7 = pregnantInfo.idcard,   //身份证              
                //                D58 = DateTime.Now.ToString("yyyy-MM-dd"),//创建时间
                //                curdate1 = DateTime.Now.ToString("yyyy-MM-dd"),
                //                D59 = orgId,//创建机构Id
                //                D60 = userName, //创建人员
                //                D61 = null,//病案号
                //                D69 = orgName, //创建机构名称:佛山市妇幼保健院
                //                D3 = pregnantInfo.personname,//孕妇姓名
                //            };

                //            #region 更新用户数据
                //            data.UpdateData(pregnantInfo);
                //            #endregion

                //            datas.Add(data);
                //            var json = datas.ToJson();
                //            postData = "data=" + HttpUtility.UrlEncode(json);
                //            result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                //            sb.AppendLine("Create 基本信息");
                //            sb.AppendLine(url);
                //            sb.AppendLine(json);
                //            sb.AppendLine(result);


                //            //孕妇是否已存在
                //            url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WCQBJ_CZDH_DOCTOR_READ&sUserID={userId}&sParams=P${pregnantInfo.idcard}$P$P";
                //            result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                //            re = result.FromJson<WCQBJ_CZDH_DOCTOR_READResponse>();
                //            var syncStatus = SyncStatus.Success;
                //            var message = "";
                //            if (re.data.Count == 0)
                //            {
                //                syncStatus = SyncStatus.Error;
                //                message = "基本数据未成功创建";
                //            }
                //            SyncOrder syncForFS = new SyncOrder()
                //            {
                //                SourceType = SourceType.PregnantInfo,
                //                SourceId = pregnantInfo.Id.ToString(),
                //                SyncTime = DateTime.Now,
                //                SyncStatus = syncStatus,
                //                ErrorMessage = message,
                //            };
                //            sb.AppendLine("--------------syncForFS");
                //            sb.AppendLine(syncStatus.ToString());
                //            sb.AppendLine(message);

                //            var serviceResult = context.DelegateTransaction((group) =>
                //            {
                //                return group.Connection.Insert(syncForFS, transaction: group.Transaction);
                //            });
                //        }
            }
            catch (Exception ex)
            {
                logger.AppendLine("出现异常:" + ex.ToString());

                syncOrder.SyncStatus = SyncStatus.Error;
                syncOrder.ErrorMessage = ex.ToString();
                context.SDService.SaveSyncOrder(syncOrder);
            }

            DoLogOnWork?.Invoke(sourceData, logger);
        }
    }
}
