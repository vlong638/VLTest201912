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
    public class SourceData_ProfessionalExaminationModel : SourceData
    {
        public ProfessionalExaminationModel SourceData;

        public SourceData_ProfessionalExaminationModel(ProfessionalExaminationModel pe)
        {
            this.SourceData = pe;
        }

        public string IdCard => SourceData.idcard;
        public string PersonName => SourceData.personname;
        public string SourceId => SourceData.id.ToString();
        public SourceType SourceType => SourceType.PregnantInfo;
    }

    public class SyncTask_Create_ProfessionalExaminationModel : SyncTask<SourceData_ProfessionalExaminationModel>
    {
        public SyncTask_Create_ProfessionalExaminationModel(ServiceContext context) : base(context)
        {
        }

        public override List<SourceData_ProfessionalExaminationModel> GetSourceDatas(UserInfo userInfo)
        {
            return Context.SDService.GetProfessionalExaminationsToCreate().Select(c => new SourceData_ProfessionalExaminationModel(c)).ToList();
        }

        public override SyncOrder DoCommit(WCQBJ_CZDH_DOCTOR_READResponse base8, UserInfo userInfo, SourceData_ProfessionalExaminationModel sourceData, StringBuilder logger)
        {
            var syncOrder = new SyncOrder()
            {
                SourceId = sourceData.SourceId,
                SourceType = SourceType.ProfessionalExamination,
                SyncTime = DateTime.Now,
                SyncStatus = SyncStatus.Success,
            };

            //获取体格检查Id
            var physicalExaminationId = Context.FSService.GetPhysicalExaminationId(userInfo, base8, DateTime.Now, ref logger);
            if (string.IsNullOrEmpty(physicalExaminationId))
            {
                syncOrder.SyncStatus = SyncStatus.Error;
                syncOrder.ErrorMessage = "未获取到体格检查Id";
                return syncOrder;
            }

            //获取专科检查
            var professionalExaminationOld = Context.FSService.GetProfessionalExamination(physicalExaminationId, userInfo, base8, ref logger);
            if (professionalExaminationOld != null)
            {
                syncOrder.SyncStatus = SyncStatus.Existed;
                syncOrder.ErrorMessage = "已存在专科检查数据";
                return syncOrder;
            }
            //更新数据
            var professionalExaminationToCreate = new WMH_CQBJ_CQJC_SAVE();
            professionalExaminationToCreate.Update(sourceData);

            //提交专科检查
            var result = Context.FSService.UpdateProfessionalExamination(physicalExaminationId,professionalExaminationToCreate, userInfo, base8, ref logger);
            if (!result.Contains("处理成功"))
            {
                syncOrder.SyncStatus = SyncStatus.Error;
                syncOrder.ErrorMessage = result;
                return syncOrder;
            }

            //获取专科检查
            var professionalExaminationCreated = Context.FSService.GetProfessionalExamination(physicalExaminationId, userInfo, base8, ref logger);
            if (professionalExaminationCreated == null)
            {
                syncOrder.SyncStatus = SyncStatus.Error;
                syncOrder.ErrorMessage = "未成功创建数据";
                return syncOrder;
            }

            return syncOrder;


            //if (IsExist(base8, userInfo, sourceData, logger, ref errorMessage))
            //{
            //    logger.AppendLine("待新建数据已存在");
            //    context.SDService.SaveSyncOrder(new SyncOrder()
            //    {
            //        SourceId = sourceData.SourceId,
            //        SourceType = sourceData.SourceType,
            //        SyncTime = DateTime.Now,
            //        SyncStatus = SyncStatus.Existed,
            //    });
            //    return;
            //}
        }

        public override void DoWork(ServiceContext context, UserInfo userInfo, SourceData_ProfessionalExaminationModel sourceData)
        {
            StringBuilder logger = new StringBuilder();
            try
            {
                var base8 = GetBase8(userInfo, sourceData.IdCard, logger);
                if (!base8.IsAvailable)
                {
                    context.SDService.SaveSyncOrder(new SyncOrder()
                    {
                        SourceId = sourceData.SourceId,
                        SourceType = sourceData.SourceType,
                        SyncTime = DateTime.Now,
                        SyncStatus = SyncStatus.Error,
                        ErrorMessage = "No Base8 Data",
                    });
                    return;
                }
                var syncResult = DoCommit(base8, userInfo, sourceData, logger);
                context.SDService.SaveSyncOrder(syncResult);
            }
            catch (Exception ex)
            {
                logger.AppendLine("出现异常:" + ex.ToString());
                context.SDService.SaveSyncOrder(new SyncOrder()
                {
                    SourceId = sourceData.SourceId,
                    SourceType = sourceData.SourceType,
                    SyncTime = DateTime.Now,
                    SyncStatus = SyncStatus.Error,
                    ErrorMessage = ex.ToString(),
                });
            }
            DoLogCreate?.Invoke(sourceData, logger);
        }
    }
    public class SyncTask_Update_ProfessionalExaminationModel : SyncTask<SourceData_ProfessionalExaminationModel>
    {
        public SyncTask_Update_ProfessionalExaminationModel(ServiceContext context) : base(context)
        {
        }

        public override List<SourceData_ProfessionalExaminationModel> GetSourceDatas(UserInfo userInfo)
        {
            return Context.SDService.GetProfessionalExaminationsToUpdate().Select(c => new SourceData_ProfessionalExaminationModel(c)).ToList();
        }

        public override SyncOrder DoCommit(WCQBJ_CZDH_DOCTOR_READResponse base8, UserInfo userInfo, SourceData_ProfessionalExaminationModel sourceData, StringBuilder logger)
        {
            var syncOrder = new SyncOrder();

            //获取体格检查Id
            var physicalExaminationId = Context.FSService.GetPhysicalExaminationId(userInfo, base8, DateTime.Now, ref logger);
            if (string.IsNullOrEmpty(physicalExaminationId))
            {
                syncOrder.SyncStatus = SyncStatus.Error;
                syncOrder.ErrorMessage = "未获取到体格检查Id";
                return syncOrder;
            }

            //获取专科检查
            var professionalExaminationOld = Context.FSService.GetProfessionalExamination(physicalExaminationId, userInfo, base8, ref logger);
            if (professionalExaminationOld == null)
            {
                syncOrder.SyncStatus = SyncStatus.NotExisted;
                syncOrder.ErrorMessage = "未获取到专科检查数据";
                return syncOrder;
            }

            //更新数据
            var professionalExaminationNew = new WMH_CQBJ_CQJC_SAVE(professionalExaminationOld);
            professionalExaminationNew.Update(sourceData);

            //提交专科检查
            var result = Context.FSService.UpdateProfessionalExamination(physicalExaminationId, professionalExaminationNew, userInfo, base8, ref logger);
            if (!result.Contains("处理成功"))
            {
                syncOrder.SyncStatus = SyncStatus.Error;
                syncOrder.ErrorMessage = result;
                return syncOrder;
            }

            return syncOrder;
        }

        public override void DoWork(ServiceContext context, UserInfo userInfo, SourceData_ProfessionalExaminationModel sourceData)
        {
            StringBuilder logger = new StringBuilder();
            var syncOrder = Context.SDService.GetSyncOrder(SourceType.ProfessionalExamination, sourceData.SourceId);
            syncOrder.SyncTime = DateTime.Now;
            try
            {

                var base8 = GetBase8(userInfo, sourceData.IdCard, logger);
                if (!base8.IsAvailable)
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "No Base8 Data";
                    context.SDService.SaveSyncOrder(syncOrder);
                    return;
                }
                var syncResult = DoCommit(base8, userInfo, sourceData, logger);
                syncOrder.SyncStatus = syncResult.SyncStatus;
                syncOrder.ErrorMessage = syncResult.ErrorMessage;
                context.SDService.SaveSyncOrder(syncOrder);
            }
            catch (Exception ex)
            {
                logger.AppendLine("出现异常:" + ex.ToString());

                syncOrder.SyncStatus = SyncStatus.Error;
                syncOrder.ErrorMessage = ex.ToString();
                context.SDService.SaveSyncOrder(syncOrder);
            }
            DoLogCreate?.Invoke(sourceData, logger);
        }
    }
}
