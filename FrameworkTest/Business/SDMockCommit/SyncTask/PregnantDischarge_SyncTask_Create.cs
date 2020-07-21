using Dapper;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.ValuesSolution;
using FrameworkTest.ConfigurableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace FrameworkTest.Business.SDMockCommit
{
    public class PregnantDischargeModel_SyncTask_Create : SyncTask<PregnantDischarge_SourceData>
    {
        public PregnantDischargeModel_SyncTask_Create(ServiceContext context) : base(context)
        {
        }


        public override List<PregnantDischarge_SourceData> GetSourceDatas(UserInfo userInfo)
        {
            return Context.ESBService.GetPregnantDischargesToCreate().Select(c => new PregnantDischarge_SourceData(c)).ToList();
        }

        public override void DoWork(ServiceContext context, UserInfo userInfo, PregnantDischarge_SourceData sourceData)
        {
            StringBuilder logger = new StringBuilder();
            var syncOrder = new SyncOrder()
            {
                SourceId = sourceData.SourceId,
                TargetType = sourceData.TargetType,
                SyncTime = DateTime.Now,
                SyncStatus = SyncStatus.Success,
            };
            try
            {
                //获取列表数据
                var listData = Context.FSService.GetPregnantDischargeList(userInfo, sourceData.inp_no,ref logger);
                if (listData==null)
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "未获取到 列表数据";
                    context.ESBService.SaveSyncOrder(syncOrder);
                    return;
                }
                //获取住院数据
                var pregnantDischargeData = Context.FSService.GetPregnantDischarge(userInfo, listData.FMMainId, ref logger);
                //数据更新
                var pregnantDischargeToCreate = new CQJL_WOMAN_FORM_SAVE_Data();
                if (pregnantDischargeData != null)
                {
                    //pregnantDischargeToCreate.Update(pregnantDischargeData);
                    syncOrder.SyncStatus = SyncStatus.Existed;
                    syncOrder.ErrorMessage = SyncStatus.Existed.GetDescription();
                    context.ESBService.SaveSyncOrder(syncOrder);
                    return;
                }
                else
                    pregnantDischargeToCreate.Init(userInfo, sourceData, listData.FMMainId);
                pregnantDischargeToCreate.Update(sourceData);
                //创建住院数据
                var result = Context.FSService.CreatePregnantDischarge(userInfo, listData, pregnantDischargeToCreate, pregnantDischargeData?.DischargeId ?? "null", ref logger);
                //保存同步记录
                context.ESBService.SaveSyncOrder(syncOrder);
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
                DoLogOnWork?.Invoke(sourceData, logger);
            }
        }
    }
}
