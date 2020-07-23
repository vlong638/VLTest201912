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
    public class PhysicalExaminationModel_SyncTask_Update : SyncTask<PhysicalExaminationModel_SourceData>
    {
        public PhysicalExaminationModel_SyncTask_Update(ServiceContext context) : base(context)
        {
        }

        public override List<PhysicalExaminationModel_SourceData> GetSourceDatas(UserInfo userInfo)
        {
            return Context.PregnantService.GetPhysicalExaminationsToUpdate().Select(c => new PhysicalExaminationModel_SourceData(c)).ToList();
        }

        public override void DoWork(ServiceContext context, UserInfo userInfo, PhysicalExaminationModel_SourceData sourceDataModel, ref StringBuilder logger)
        {
            var syncOrder = Context.PregnantService.GetSyncOrder(sourceDataModel.TargetType, sourceDataModel.SourceId);
            syncOrder.SyncTime = DateTime.Now;
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
                if (string.IsNullOrEmpty(physicalExaminationId))
                {
                    syncOrder.SyncStatus = SyncStatus.NotExisted;
                    syncOrder.ErrorMessage = SyncStatus.NotExisted.GetDescription();
                    context.PregnantService.SaveSyncOrder(syncOrder);
                    return;
                }
                //获取体格检查
                var physicalExamination = Context.FSService.GetPhysicalExamination(physicalExaminationId, userInfo, base8, DateTime.Now, ref logger);
                if (physicalExamination == null)
                {
                    syncOrder.SyncStatus = SyncStatus.NotExisted;
                    syncOrder.ErrorMessage = SyncStatus.NotExisted.GetDescription();
                    context.PregnantService.SaveSyncOrder(syncOrder);
                    return;
                }
                //新建体格检查
                var datas = new List<WMH_CQBJ_CQJC_TGJC_NEW_SAVE_Data>();
                var data = new WMH_CQBJ_CQJC_TGJC_NEW_SAVE_Data(physicalExamination);
                data.UpdateExamination(sourceDataModel.SourceData);
                datas.Add(data);
                var result = Context.FSService.UpdatePhysicalExamination(physicalExaminationId, datas, userInfo, base8, ref logger);
                if (!result)
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "更新体格检查失败";
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
