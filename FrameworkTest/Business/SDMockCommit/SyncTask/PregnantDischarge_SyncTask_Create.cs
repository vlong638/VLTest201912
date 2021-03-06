﻿using FrameworkTest.Common.ValuesSolution;
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
                OperateType = OperateType.Create,
            };
            try
            {
                var listData = Context.FSService.GetPregnantInHospitalList(userInfo, sourceData.inp_no, ref logger);//获取列表数据
                if (listData == null)
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "未获取到 列表数据";
                    syncOrder.Id = context.ESBService.SaveSyncOrder(syncOrder);
                    return;
                }
                var diagnosis = Context.ESBService.GetDiagnosisByPatientIdAndINPNo(sourceData.SourceData.inp_no, sourceData.SourceData.visit_id);//获得诊断信息
                if (VLConstraints.HasDeadDiagnosis(diagnosis))//有死亡风险的不作上传
                {
                    syncOrder.SyncStatus = SyncStatus.DeadDiagnosis;
                    syncOrder.ErrorMessage = SyncStatus.DeadDiagnosis.GetDescription();
                    syncOrder.Id = context.ESBService.SaveSyncOrder(syncOrder);
                    return;
                }
                var pregnantDischargeData = Context.FSService.GetPregnantDischarge(userInfo, listData.FMMainId, ref logger);//获取住院数据
                var highRisks = Context.PregnantService.GetLatestHighRisksByIdCard(sourceData.SourceData.idcard);//获取高危因素
                var advices = Context.ESBService.GetAdvicesByPatientId(sourceData.SourceData.inp_no);//获取医嘱信息
                var inspections = Context.ESBService.GetInspectionsByPatientId(sourceData.SourceData.inp_no);//获取检验结果
                //数据更新
                var pregnantDischargeToCreate = new CQJL_WOMAN_FORM_SAVE_Data();
                if (pregnantDischargeData != null)
                {
                    syncOrder.SyncStatus = SyncStatus.Existed;
                    syncOrder.ErrorMessage = SyncStatus.Existed.GetDescription();
                    syncOrder.Id = context.ESBService.SaveSyncOrder(syncOrder);
                    return;
                }
                else
                {
                    pregnantDischargeToCreate.Init(userInfo, sourceData, listData.FMMainId);
                }
                pregnantDischargeToCreate.Update(sourceData, highRisks, diagnosis, advices, inspections);
                //数据有效性校验
                var validResult = pregnantDischargeToCreate.Validate();
                if (validResult.Code != ValidateResultCode.Success)
                {
                    syncOrder.SyncStatus = SyncStatus.Invalid;
                    syncOrder.ErrorMessage = validResult.Message;
                    syncOrder.Id = context.ESBService.SaveSyncOrder(syncOrder);
                    return;
                }
                //提交佛山
                var result = Context.FSService.SavePregnantDischarge(userInfo, listData, pregnantDischargeToCreate, pregnantDischargeData?.DischargeId ?? "null", ref logger);//创建住院数据
                syncOrder.Id = context.ESBService.SaveSyncOrder(syncOrder);//保存同步记录2
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
