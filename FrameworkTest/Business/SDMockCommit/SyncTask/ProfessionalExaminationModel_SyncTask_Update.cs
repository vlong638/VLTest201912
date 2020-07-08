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
    public class ProfessionalExaminationModel_SyncTask_Update : SyncTask<ProfessionalExaminationModel_SourceData>
    {
        public ProfessionalExaminationModel_SyncTask_Update(ServiceContext context) : base(context)
        {
        }

        public override List<ProfessionalExaminationModel_SourceData> GetSourceDatas(UserInfo userInfo)
        {
            return Context.SDService.GetProfessionalExaminationsToUpdate().Select(c => new ProfessionalExaminationModel_SourceData(c)).ToList();
        }

        public SyncOrder DoCommit(WCQBJ_CZDH_DOCTOR_READResponse base8, UserInfo userInfo, ProfessionalExaminationModel_SourceData sourceData, StringBuilder logger)
        {
            var syncOrder = new SyncOrder();
            syncOrder.SyncStatus = SyncStatus.Success;

            return syncOrder;
        }

        public override void DoWork(ServiceContext context, UserInfo userInfo, ProfessionalExaminationModel_SourceData sourceData)
        {
            StringBuilder logger = new StringBuilder();
            var syncOrder = Context.SDService.GetSyncOrder(SourceType.ProfessionalExamination, sourceData.SourceId);
            syncOrder.SyncTime = DateTime.Now;
            try
            {
                //获取八项基础信息
                var base8 = Context.FSService.GetBase8(userInfo, sourceData.IdCard, ref logger);
                if (base8 == null)
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "未获取到 Base8";
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
                if (professionalExaminationOld == null)
                {
                    syncOrder.SyncStatus = SyncStatus.NotExisted;
                    syncOrder.ErrorMessage = "未获取到专科检查数据";
                    context.SDService.SaveSyncOrder(syncOrder);
                    return;
                }
                //更新数据
                var professionalExaminationToUpdate = new WMH_CQBJ_CQJC_SAVE(professionalExaminationOld);
                professionalExaminationToUpdate.Update(userInfo, sourceData, null);
                //提交专科检查
                var isSuccess = Context.FSService.UpdateProfessionalExamination(physicalExaminationId, professionalExaminationToUpdate, userInfo, base8, ref logger);
                if (!isSuccess)
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "保存专科检查时,未返回成功";
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
