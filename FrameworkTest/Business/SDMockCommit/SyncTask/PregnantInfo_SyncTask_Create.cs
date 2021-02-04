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
    public class PregnantInfo_SyncTask_Create : SyncTask<PregnantInfo_SourceData>
    {
        public int RetryAmount { get; set; } = 5;

        public PregnantInfo_SyncTask_Create(ServiceContext context) : base(context)
        {
        }

        public override List<PregnantInfo_SourceData> GetSourceDatas(UserInfo userInfo)
        {
            return Context.PregnantService.GetPregnantInfoForCreate().Select(c => new PregnantInfo_SourceData(c)).ToList();
        }

        public override void DoWork(ServiceContext context, UserInfo userInfo, PregnantInfo_SourceData sourceData, ref StringBuilder logger)
        {
            var syncOrder = new SyncOrder()
            {
                SourceId = sourceData.SourceId,
                TargetType = sourceData.TargetType,
                SyncTime = DateTime.Now,
                SyncStatus = SyncStatus.Success,
                OperateType = OperateType.Create,
            };
            try
            {
                //获取`基本信息概要`
                var base8 = context.FSService.GetBase8(userInfo, sourceData.IdCard, ref logger);
                if (base8 != null)//已存在 更新分支
                {
                    var base18 = context.FSService.GetBase18(userInfo, sourceData.IdCard, ref logger);
                    if (base18 == null)
                    {
                        logger.Append("用户数据已存在");
                        syncOrder.SyncStatus = SyncStatus.Existed;
                        syncOrder.ErrorMessage = "无`base18`数据";
                        context.PregnantService.SaveSyncOrder(syncOrder);
                        return;
                    }
                    else
                    {
                        var lastmenstrualperiod1 = sourceData.Data.lastmenstrualperiod;
                        var lastmenstrualperiod2 = base18.D4.ToDateTime();
                        if (lastmenstrualperiod1.HasValue && lastmenstrualperiod2.HasValue)
                        {
                            if (lastmenstrualperiod1.Value.Date != lastmenstrualperiod2.Value.Date)
                            {
                                syncOrder.SyncStatus = SyncStatus.Conflict;
                                syncOrder.ErrorMessage = SyncStatus.Conflict.GetDescription();
                                context.PregnantService.SaveSyncOrder(syncOrder);
                                return;
                            }
                            else
                            {
                                syncOrder.SyncStatus = SyncStatus.Existed;
                                syncOrder.ErrorMessage = SyncStatus.Existed.GetDescription();
                                context.PregnantService.SaveSyncOrder(syncOrder);
                                return;
                            }
                        }
                        else
                        {
                            syncOrder.SyncStatus = SyncStatus.Existed;
                            syncOrder.ErrorMessage = "无`预产期`数据";
                            context.PregnantService.SaveSyncOrder(syncOrder);
                            return;
                        }
                    }
                }
                else //新建分支
                {
                    syncOrder.ErrorMessage = "新建";
                    //获取 患者主索引
                    string mainId = Context.FSService.GetUniqueId(userInfo, ref logger);
                    if (string.IsNullOrEmpty(mainId))
                    {
                        syncOrder.SyncStatus = SyncStatus.Error;
                        syncOrder.ErrorMessage = "未获取到 患者主索引";
                        context.PregnantService.SaveSyncOrder(syncOrder);
                        return;
                    }
                    //身份证,主索引查重出现重复
                    var isRepeat = context.FSService.IsExistByMainIdOrIdCard(userInfo, mainId, sourceData, ref logger);
                    if (isRepeat)
                    {
                        syncOrder.SyncStatus = SyncStatus.Error;
                        syncOrder.ErrorMessage = "身份证,主索引查重出现重复";
                        context.PregnantService.SaveSyncOrder(syncOrder);
                        return;
                    }
                    //获取 CareId
                    string careId = "";
                    string careIdL8 = "";
                    int errorCount = 0;
                    int maxErrorCount = Math.Max(5, RetryAmount);
                    while (errorCount < maxErrorCount)
                    {
                        //Create 保健号
                        careId = context.FSService.GetCareId(userInfo, ref logger);
                        if (string.IsNullOrEmpty(careId))
                        {
                            syncOrder.SyncStatus = SyncStatus.Error;
                            syncOrder.ErrorMessage = "未获取到 保健号";
                            context.PregnantService.SaveSyncOrder(syncOrder);
                            return;
                        }
                        //保健号查重
                        careIdL8 = careId.Substring(8);
                        var isRepeatByCareId = context.FSService.IsExistByCareId(userInfo, mainId, careId, sourceData, ref logger);
                        if (isRepeatByCareId)
                        {
                            errorCount++;
                            continue;
                        }
                        break;
                    }
                    //保健号查重出现重复
                    if (errorCount == maxErrorCount)
                    {
                        Console.WriteLine($"保健号查重出现重复");
                        syncOrder.SyncStatus = SyncStatus.Repeated;
                        context.PregnantService.SaveSyncOrder(syncOrder);
                        return;
                    }
                    else
                    {
                        logger.AppendLine($"--------查重通过");
                    }
                    //提交孕妇信息 
                    var datas = new List<WMH_CQBJ_JBXX_FORM_SAVEData>();
                    var data = new WMH_CQBJ_JBXX_FORM_SAVEData()
                    {
                        D1 = careIdL8, //@保健号后8位
                        D2 = careId, //@保健号
                        D7 = sourceData.IdCard,   //身份证              
                        D58 = DateTime.Now.ToString("yyyy-MM-dd"),//创建时间
                        curdate1 = DateTime.Now.ToString("yyyy-MM-dd"),
                        D59 = userInfo.OrgId,//创建机构Id
                        D60 = userInfo.UserName, //创建人员
                        D61 = null,//病案号
                        D69 = userInfo.OrgName, //创建机构名称:佛山市妇幼保健院
                        D3 = sourceData.PersonName,//孕妇姓名
                    };
                    data.UpdateData(sourceData.Data);
                    datas.Add(data);
                    var isSuccess = context.FSService.CreatePregnantInfo(userInfo, mainId, datas, ref logger);
                    //这里的isSuccess不足以判断后续的成功
                    base8 = context.FSService.GetBase8(userInfo, sourceData.IdCard, ref logger);
                    if (base8 == null)
                    {
                        syncOrder.SyncStatus = SyncStatus.Error;
                        syncOrder.ErrorMessage = "基本数据未成功创建";
                        context.PregnantService.SaveSyncOrder(syncOrder);
                    }
                    syncOrder.ErrorMessage = $"{{ mainId:'{mainId}',careId:'{careId}'}}";
                }
                //保存同步记录
                context.PregnantService.SaveSyncOrder(syncOrder);
            }
            catch (Exception ex)
            {
                logger.AppendLine("出现异常:" + ex.ToString());

                syncOrder.SyncStatus = SyncStatus.Error;
                syncOrder.ErrorMessage = ex.ToString();
                context.PregnantService.SaveSyncOrder(syncOrder);
            }
            finally
            {
                logger.AppendLine(">>>syncOrder.ErrorMessage");
                logger.AppendLine(syncOrder.ErrorMessage);
                logger.AppendLine(">>>syncOrder.ToJson()");
                logger.AppendLine(syncOrder.ToJson());
            }
        }
    }
}
