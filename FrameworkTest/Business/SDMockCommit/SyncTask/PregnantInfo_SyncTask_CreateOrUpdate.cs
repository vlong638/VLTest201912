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
    public class PregnantInfo_SyncTask_CreateOrUpdate : SyncTask<PregnantInfo_SourceData>
    {
        public PregnantInfo_SyncTask_CreateOrUpdate(ServiceContext context) : base(context)
        {
        }

        public override List<PregnantInfo_SourceData> GetSourceDatas(UserInfo userInfo)
        {
            return Context.SDService.GetPregnantInfoForCreateOrUpdate().Select(c => new PregnantInfo_SourceData(c)).ToList();
        }

        //public override SyncOrder DoCommit(WCQBJ_CZDH_DOCTOR_READResponse base8, UserInfo userInfo, PregnantInfo_SourceData sourceData, StringBuilder logger)
        //{
        //    var syncOrder = new SyncOrder()
        //    {
        //        SourceId = sourceData.SourceId,
        //        SourceType = SourceType.ProfessionalExamination,
        //        SyncTime = DateTime.Now,
        //        SyncStatus = SyncStatus.Success,
        //    };

        //    //获取体格检查Id
        //    var physicalExaminationId = Context.FSService.GetPhysicalExaminationId(userInfo, base8, DateTime.Now, ref logger);
        //    if (string.IsNullOrEmpty(physicalExaminationId))
        //    {
        //        syncOrder.SyncStatus = SyncStatus.Error;
        //        syncOrder.ErrorMessage = "未获取到体格检查Id";
        //        return syncOrder;
        //    }

        //    //获取专科检查
        //    var professionalExaminationOld = Context.FSService.GetProfessionalExamination(physicalExaminationId, userInfo, base8, ref logger);
        //    if (professionalExaminationOld != null)
        //    {
        //        syncOrder.SyncStatus = SyncStatus.Existed;
        //        syncOrder.ErrorMessage = "已存在专科检查数据";
        //        return syncOrder;
        //    }
        //    //更新数据
        //    var professionalExaminationToCreate = new WMH_CQBJ_CQJC_SAVE();
        //    professionalExaminationToCreate.Update(sourceData);

        //    //提交专科检查
        //    var result = Context.FSService.UpdateProfessionalExamination(physicalExaminationId,professionalExaminationToCreate, userInfo, base8, ref logger);
        //    if (!result.Contains("处理成功"))
        //    {
        //        syncOrder.SyncStatus = SyncStatus.Error;
        //        syncOrder.ErrorMessage = result;
        //        return syncOrder;
        //    }

        //    //获取专科检查
        //    var professionalExaminationCreated = Context.FSService.GetProfessionalExamination(physicalExaminationId, userInfo, base8, ref logger);
        //    if (professionalExaminationCreated == null)
        //    {
        //        syncOrder.SyncStatus = SyncStatus.Error;
        //        syncOrder.ErrorMessage = "未成功创建数据";
        //        return syncOrder;
        //    }

        //    return syncOrder;


        //    //if (IsExist(base8, userInfo, sourceData, logger, ref errorMessage))
        //    //{
        //    //    logger.AppendLine("待新建数据已存在");
        //    //    context.SDService.SaveSyncOrder(new SyncOrder()
        //    //    {
        //    //        SourceId = sourceData.SourceId,
        //    //        SourceType = sourceData.SourceType,
        //    //        SyncTime = DateTime.Now,
        //    //        SyncStatus = SyncStatus.Existed,
        //    //    });
        //    //    return;
        //    //}
        //}

        public override void DoWork(ServiceContext context, UserInfo userInfo, PregnantInfo_SourceData sourceData)
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
            DoLogOnWork?.Invoke(sourceData, logger);
        }

        public SyncOrder DoCommit(WCQBJ_CZDH_DOCTOR_READResponse base8, UserInfo userInfo, PregnantInfo_SourceData sourceData, StringBuilder logger)
        {
            throw new NotImplementedException();
        }
    }
}
