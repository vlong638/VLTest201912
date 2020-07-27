using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameworkTest.Business.SDMockCommit
{
    public class PregnantDischarge_SyncTask_Create : SyncTask<PregnantDischarge_SourceData>
    {
        public PregnantDischarge_SyncTask_Create(ServiceContext context) : base(context)
        {
        }

        public override List<PregnantDischarge_SourceData> GetSourceDatas(UserInfo userInfo)
        {
            Context.ESBService.UpdatePregnantDischarge();
            return Context.ESBService.GetPregnantDischargesToCreate().Select(c => new PregnantDischarge_SourceData(c)).ToList();
        }

        public override void DoWork(ServiceContext context, UserInfo userInfo, PregnantDischarge_SourceData sourceData, ref StringBuilder logger)
        {
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
                var listData = Context.FSService.GetPregnantInHospitalList(userInfo, sourceData.inp_no,ref logger);
                if (listData==null)
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "未获取到 列表数据";
                    context.ESBService.SaveSyncOrder(syncOrder);
                    return;
                }
                //获得诊断信息
                var diagnosis = Context.ESBService.GetDiagnosisByPatientIdAndINPNo(sourceData.SourceData.inp_no, sourceData.SourceData.visit_id);
                //有死亡风险的不作上传
                if (VLConstraints.HasDeadDiagnosis(diagnosis))
                {
                    syncOrder.SyncStatus = SyncStatus.DeadDiagnosis;
                    syncOrder.ErrorMessage = SyncStatus.DeadDiagnosis.GetDescription();
                    context.ESBService.SaveSyncOrder(syncOrder);
                    return;
                }
                //获取住院数据
                var pregnantDischargeData = Context.FSService.GetPregnantDischarge(userInfo, listData.FMMainId, ref logger);
                //获取高危因素
                var highRisks = Context.PregnantService.GetLatestHighRisksByIdCard(sourceData.SourceData.idcard);
                //数据更新
                var pregnantDischargeToCreate = new CQJL_WOMAN_FORM_SAVE_Data();
                if (pregnantDischargeData != null)
                {
                    syncOrder.SyncStatus = SyncStatus.Existed;
                    syncOrder.ErrorMessage = SyncStatus.Existed.GetDescription();
                    context.ESBService.SaveSyncOrder(syncOrder);
                    return;
                }
                else
                {
                    pregnantDischargeToCreate.Init(userInfo, sourceData, listData.FMMainId);
                }
                pregnantDischargeToCreate.Update(sourceData, highRisks, diagnosis);
                //创建住院数据
                var result = Context.FSService.SavePregnantDischarge(userInfo, listData, pregnantDischargeToCreate, pregnantDischargeData?.DischargeId ?? "null", ref logger);
                //保存同步记录2
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
            }
        }
    }
}
