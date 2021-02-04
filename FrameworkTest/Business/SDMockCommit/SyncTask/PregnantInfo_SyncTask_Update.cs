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
    public class PregnantInfo_SyncTask_Update : SyncTask<PregnantInfo_SourceData>
    {
        public PregnantInfo_SyncTask_Update(ServiceContext context) : base(context)
        {
        }

        public override List<PregnantInfo_SourceData> GetSourceDatas(UserInfo userInfo)
        {
            return Context.PregnantService.GetPregnantInfoForUpdate().Select(c => new PregnantInfo_SourceData(c)).ToList();
        }

        public override void DoWork(ServiceContext context, UserInfo userInfo, PregnantInfo_SourceData sourceData, ref StringBuilder logger)
        {
            var syncOrder = Context.PregnantService.GetSyncOrder(sourceData.TargetType, sourceData.SourceId);
            syncOrder.SyncTime = DateTime.Now;
            syncOrder.OperateType = OperateType.Edit;
            syncOrder.SyncStatus = SyncStatus.Success;
            try
            {
                var pregnantInfo = context.FSService.GetBase8(userInfo, sourceData.IdCard, ref logger);
                if (pregnantInfo != null)//已存在 更新分支
                {
                    syncOrder.ErrorMessage = "更新";
                    var base8 = Context.FSService.GetBase8(userInfo, sourceData.IdCard, ref logger);
                    if (base8 == null)
                    {
                        syncOrder.SyncStatus = SyncStatus.Error;
                        syncOrder.ErrorMessage = "未获取到 Base8";
                        context.PregnantService.SaveSyncOrder(syncOrder);
                        return;
                    }
                    //获取用户信息
                    var base77 = Context.FSService.GetBase77(userInfo, base8.MainId, ref logger);
                    if (base77 == null)
                    {
                        syncOrder.SyncStatus = SyncStatus.Error;
                        syncOrder.ErrorMessage = "未获取到 Base77";
                        context.PregnantService.SaveSyncOrder(syncOrder);
                        return;
                    }
                    //更新用户数据
                    var data = new WMH_CQBJ_JBXX_FORM_SAVEData(base77);
                    data.UpdateData(sourceData.Data);
                    var datas = new List<WMH_CQBJ_JBXX_FORM_SAVEData>() { data };
                    var isSuccess = context.FSService.UpdatePregnantInfo(userInfo, base8.MainId, base77.MainIdForChange, datas, ref logger);
                    if (!isSuccess)
                    {
                        syncOrder.SyncStatus = SyncStatus.Error;
                        syncOrder.ErrorMessage = "更新未返回成功标识";
                        context.PregnantService.SaveSyncOrder(syncOrder);
                        return;
                    }
                }
                else //新建分支
                {
                    syncOrder.SyncStatus = SyncStatus.NotExisted;
                    syncOrder.ErrorMessage = "待更新用户数据不存在";
                    context.PregnantService.SaveSyncOrder(syncOrder);
                    return;
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