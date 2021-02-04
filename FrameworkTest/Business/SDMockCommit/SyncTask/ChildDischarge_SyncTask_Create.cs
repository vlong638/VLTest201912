using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameworkTest.Business.SDMockCommit
{
    public class ChildDischarge_SyncTask_Create : SyncTask<ChildDischarge_SourceData>
    {
        public ChildDischarge_SyncTask_Create(ServiceContext context) : base(context)
        {
        }


        public override List<ChildDischarge_SourceData> GetSourceDatas(UserInfo userInfo)
        {
            return Context.ESBService.GetChildDischargesToCreate().Select(c => new ChildDischarge_SourceData(c)).ToList();
        }

        public override void DoWork(ServiceContext context, UserInfo userInfo, ChildDischarge_SourceData sourceData, ref StringBuilder logger)
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
                //获取列表数据
                var listData = Context.FSService.GetPregnantInHospitalList(userInfo, sourceData.inp_no, ref logger);
                if (listData == null)
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "未获取到 列表数据";
                    syncOrder.Id = context.ESBService.SaveSyncOrder(syncOrder);
                    return;
                }
                //获得诊断信息
                var diagnosis = Context.ESBService.GetDiagnosisByPatientIdAndINPNo(sourceData.SourceData.inp_no, sourceData.SourceData.visit_id);
                //有死亡风险的不作上传
                if (VLConstraints.HasDeadDiagnosis(diagnosis))
                {
                    syncOrder.SyncStatus = SyncStatus.DeadDiagnosis;
                    syncOrder.ErrorMessage = SyncStatus.DeadDiagnosis.GetDescription();
                    syncOrder.Id = context.ESBService.SaveSyncOrder(syncOrder);
                    return;
                }
                //获得新生儿出生缺陷信息
                var birthDefects = Context.HISService.GetBirthDefects(sourceData.SourceData.shenfenzh);
                //获取住院数据
                var childDischargeData = Context.FSService.GetChildDischarge(userInfo, listData.FMMainId, ref logger);
                //数据更新
                var childDischargeToCreate = new CQJL_CHILD_FORM_SAVE_Data();
                if (childDischargeData != null)
                {
                    //ChildDischargeToCreate.Update(ChildDischargeData);
                    syncOrder.SyncStatus = SyncStatus.Existed;
                    syncOrder.ErrorMessage = SyncStatus.Existed.GetDescription();
                    syncOrder.Id = context.ESBService.SaveSyncOrder(syncOrder);
                    return;
                }
                else
                {
                    childDischargeToCreate.Init(userInfo);
                }
                childDischargeToCreate.Update(sourceData, diagnosis, birthDefects);
                //数据有效性校验
                var validResult = childDischargeToCreate.Validate();
                if (validResult.Code != ValidateResultCode.Success)
                {
                    syncOrder.SyncStatus = SyncStatus.Invalid;
                    syncOrder.ErrorMessage = validResult.Message;
                    syncOrder.Id = context.ESBService.SaveSyncOrder(syncOrder);
                    return;
                }
                //创建住院数据
                var result = Context.FSService.SaveChildDischarge(userInfo, childDischargeToCreate, listData.FMMainId, ref logger);
                //保存同步记录
                syncOrder.Id = context.ESBService.SaveSyncOrder(syncOrder);
            }
            catch (Exception ex)
            {
                logger.AppendLine("出现异常:" + ex.ToString());

                syncOrder.SyncStatus = SyncStatus.Error;
                syncOrder.ErrorMessage = ex.ToString();
                syncOrder.Id = context.ESBService.SaveSyncOrder(syncOrder);
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
