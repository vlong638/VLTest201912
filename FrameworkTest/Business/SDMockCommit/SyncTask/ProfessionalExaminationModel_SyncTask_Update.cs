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

        public override void DoWork(ServiceContext context, UserInfo userInfo, ProfessionalExaminationModel_SourceData sourceDataModel)
        {
            StringBuilder logger = new StringBuilder();
            var syncOrder = Context.SDService.GetSyncOrder(TargetType.ProfessionalExamination, sourceDataModel.SourceId);
            syncOrder.SyncTime = DateTime.Now;
            try
            {
                //获取八项基础信息
                var base8 = Context.FSService.GetBase8(userInfo, sourceDataModel.IdCard, ref logger);
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

                #region 高危处理

                ////获取高危数据Id
                //var newHighRiskId = Context.FSService.GetUniqueId(userInfo, ref logger);
                //if (newHighRiskId == null)
                //{
                //    syncOrder.SyncStatus = SyncStatus.Error;
                //    syncOrder.ErrorMessage = "未获取到高危Id";
                //    context.SDService.SaveSyncOrder(syncOrder);
                //    return;
                //}

                //获取高危数据全量
                var allHighRisksResponse = Context.FSService.GetAllHighRisks(physicalExaminationId, userInfo, base8, ref logger);
                if (allHighRisksResponse == null || allHighRisksResponse.Count == 0)
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "未获取到高危数据全量列表";
                    context.SDService.SaveSyncOrder(syncOrder);
                    return;
                }

                ////获取高危数据变量
                //var currentHighRisks = Context.FSService.GetCurrentHighRisks(physicalExaminationId, userInfo, base8, ref logger);
                //if (currentHighRisks == null)
                //{
                //    syncOrder.SyncStatus = SyncStatus.Error;
                //    syncOrder.ErrorMessage = "未获取到高危数据变量";
                //    context.SDService.SaveSyncOrder(syncOrder);
                //    return;
                //}

                //更新高危数据
                var heleHighRisks = sourceDataModel.SourceData.highriskdic?.FromJson<List<HighRiskEntity>>() ?? new List<HighRiskEntity>();
                var highRisksToSave = new WMH_WCQBJ_GWYCF_SCORE_SAVERequest();
                //logger.AppendLine(">>>currentHighRisks");
                //logger.AppendLine(currentHighRisks.ToJson());
                //logger.AppendLine(">>>heleHighRisks");
                //logger.AppendLine(heleHighRisks.ToJson());
                //logger.AppendLine($">>>bmi:{sourceDataModel.SourceData.BMI}");
                var age = VLConstraints.DateTime.GetYearsBy(sourceDataModel.SourceData.birthday, sourceDataModel.SourceData.dateofprenatal.ToDateTime());
                highRisksToSave.Update(allHighRisksResponse, heleHighRisks, sourceDataModel.SourceData.BMI?.ToDecimal(), age, ref logger);
                //更新高危数据
                if (highRisksToSave.Count > 0)
                {
                    var isSuccess2 = Context.FSService.SaveCurrentHignRisks(physicalExaminationId, highRisksToSave, userInfo, base8, ref logger);
                    if (!isSuccess2)
                    {
                        syncOrder.SyncStatus = SyncStatus.Error;
                        syncOrder.ErrorMessage = "更新高危数据时出错";
                        context.SDService.SaveSyncOrder(syncOrder);
                        return;
                    }
                }

                #endregion

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
                professionalExaminationToUpdate.Update(userInfo, sourceDataModel, highRisksToSave);
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
                logger.AppendLine(">>>syncOrder.ErrorMessage");
                logger.AppendLine(syncOrder.ErrorMessage);
                logger.AppendLine(">>>syncOrder.ToJson()");
                logger.AppendLine(syncOrder.ToJson());
                DoLogOnWork?.Invoke(sourceDataModel, logger);
            }
        }
    }
}
