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
    public class PhysicalExaminationModel_SyncTask_Create : SyncTask<PhysicalExaminationModel_SourceData>
    {
        public PhysicalExaminationModel_SyncTask_Create(ServiceContext context) : base(context)
        {
        }

        public override List<PhysicalExaminationModel_SourceData> GetSourceDatas(UserInfo userInfo)
        {
            return Context.PregnantService.GetPhysicalExaminationsToCreate().Select(c => new PhysicalExaminationModel_SourceData(c)).ToList();
        }

        public override void DoWork(ServiceContext context, UserInfo userInfo, PhysicalExaminationModel_SourceData sourceDataModel, ref StringBuilder logger)
        {
            var syncOrder = new SyncOrder()
            {
                SourceId = sourceDataModel.SourceId,
                TargetType = sourceDataModel.TargetType,
                SyncTime = DateTime.Now,
                SyncStatus = SyncStatus.Success,
                OperateType = OperateType.Create,
            };
            try
            {
                //获取八项基础信息
                var base8 = Context.FSService.GetBase8(userInfo, sourceDataModel.IdCard, ref logger);
                if (base8 == null)
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "未获取到 Base8";
                    context.PregnantService.SaveSyncOrder(syncOrder);
                    return;
                }
                //获取体格检查Id
                var physicalExaminationId = Context.FSService.GetPhysicalExaminationId(userInfo, base8, DateTime.Now, ref logger);
                if (!string.IsNullOrEmpty(physicalExaminationId))
                {
                    syncOrder.SyncStatus = SyncStatus.Existed;
                    syncOrder.ErrorMessage = SyncStatus.Existed.GetDescription();
                    context.PregnantService.SaveSyncOrder(syncOrder);
                    return;
                }
                //获取 UniqueId
                var uniqueId = Context.FSService.GetUniqueId(userInfo, ref logger);
                if (string.IsNullOrEmpty(uniqueId))
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "未获取到 UniqueId";
                    context.PregnantService.SaveSyncOrder(syncOrder);
                    return;
                }
                //新建体格检查
                var datas = new List<WMH_CQBJ_CQJC_TGJC_NEW_SAVE_Data>();
                var data = new WMH_CQBJ_CQJC_TGJC_NEW_SAVE_Data();
                data.UpdateExamination(sourceDataModel.SourceData);
                datas.Add(data);
                var result = Context.FSService.CreatePhysicalExamination(datas, userInfo, uniqueId, base8, ref logger);
                if (!result)
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "新建体格检查失败";
                    context.PregnantService.SaveSyncOrder(syncOrder);
                    return;
                }
                //获取体格检查Id
                physicalExaminationId = Context.FSService.GetPhysicalExaminationId(userInfo, base8, DateTime.Now, ref logger);
                if (string.IsNullOrEmpty(physicalExaminationId))
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "未能成功新建体格检查";
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
