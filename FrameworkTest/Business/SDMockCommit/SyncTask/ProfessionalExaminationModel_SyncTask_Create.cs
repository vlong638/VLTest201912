using Dapper;
using FrameworkTest.Common.DBSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace FrameworkTest.Business.SDMockCommit
{
    public class ProfessionalExaminationModel_SyncTask_Create : SyncTask<ProfessionalExaminationModel_SourceData>
    {
        public ProfessionalExaminationModel_SyncTask_Create(ServiceContext context) : base(context)
        {
        }

        public override List<ProfessionalExaminationModel_SourceData> GetSourceDatas(UserInfo userInfo)
        {
            return Context.SDService.GetProfessionalExaminationsToCreate().Select(c => new ProfessionalExaminationModel_SourceData(c)).ToList();
        }

        public override void DoWork(ServiceContext context, UserInfo userInfo, ProfessionalExaminationModel_SourceData sourceData)
        {
            StringBuilder logger = new StringBuilder();
            var syncOrder = new SyncOrder()
            {
                SourceId = sourceData.SourceId,
                SourceType = sourceData.SourceType,
                SyncTime = DateTime.Now,
                SyncStatus = SyncStatus.Success,
            };
            try
            {
                //获取八项基础信息
                var base8 = GetBase8(userInfo, sourceData.IdCard, logger);
                if (!base8.IsAvailable)
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "No Base8 Data";
                    context.SDService.SaveSyncOrder(syncOrder);
                    return;
                }
                //获取体格检查Id
                var physicalExaminationId = Context.FSService.GetPhysicalExaminationId(userInfo, base8, DateTime.Now, ref logger);
                if (string.IsNullOrEmpty(physicalExaminationId))
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "未获取到体格检查Id";
                    context.SDService.SaveSyncOrder(syncOrder);
                    return;
                }
                //获取专科检查
                var professionalExaminationOld = Context.FSService.GetProfessionalExamination(physicalExaminationId, userInfo, base8, ref logger);
                if (professionalExaminationOld != null)
                {
                    syncOrder.SyncStatus = SyncStatus.Existed;
                    syncOrder.ErrorMessage = "已存在专科检查数据";
                    context.SDService.SaveSyncOrder(syncOrder);
                    return;
                }
                //更新数据
                var professionalExaminationToCreate = new WMH_CQBJ_CQJC_SAVE();
                professionalExaminationToCreate.Update(userInfo, sourceData);
                //提交专科检查
                var result = Context.FSService.UpdateProfessionalExamination(physicalExaminationId, professionalExaminationToCreate, userInfo, base8, ref logger);
                if (!result.Contains("处理成功"))
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = result;
                    context.SDService.SaveSyncOrder(syncOrder);
                    return;
                }
                //获取专科检查
                var professionalExaminationCreated = Context.FSService.GetProfessionalExamination(physicalExaminationId, userInfo, base8, ref logger);
                if (professionalExaminationCreated == null)
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "未成功创建数据";
                    context.SDService.SaveSyncOrder(syncOrder);
                    return;
                }

                context.SDService.SaveSyncOrder(syncOrder);
            }
            catch (Exception ex)
            {
                logger.AppendLine("出现异常:" + ex.ToString());

                syncOrder.SyncStatus = SyncStatus.Error;
                syncOrder.ErrorMessage = ex.ToString();
                context.SDService.SaveSyncOrder(syncOrder);
            }
            finally
            {
                DoLogOnWork?.Invoke(sourceData, logger);
            }
        }
    }
}
