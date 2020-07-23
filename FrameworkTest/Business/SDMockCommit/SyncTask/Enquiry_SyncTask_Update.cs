using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.FileSolution;
using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FrameworkTest.Business.SDMockCommit
{
    public class Enquiry_SyncTask_Update : SyncTask<Enquiry_SourceData>
    {
        public Enquiry_SyncTask_Update(ServiceContext context) : base(context)
        {
        }

        public override List<Enquiry_SourceData> GetSourceDatas(UserInfo userInfo)
        {
            return Context.PregnantService.GetPregnantInfosToUpdateEnquiries().Select(c => new Enquiry_SourceData(c)).ToList();
        }

        public override void DoWork(ServiceContext context, UserInfo userInfo, Enquiry_SourceData sourceData, ref StringBuilder logger)
        {
            var syncOrder = Context.PregnantService.GetSyncOrder(sourceData.TargetType, sourceData.SourceId);
            syncOrder.SyncTime = DateTime.Now;
            syncOrder.SyncStatus = SyncStatus.Success;
            syncOrder.ErrorMessage = SyncStatus.Success.GetDescription();
            try
            {
                //获取 关键Id
                var base8 = context.FSService.GetBase8(userInfo, sourceData.IdCard, ref logger);
                if (base8 == null)
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "未获取到 Base8";
                    context.PregnantService.SaveSyncOrder(syncOrder);
                    return;
                }
                //获取 问询病史
                var enquiry = context.FSService.GetEnquiry(userInfo, base8, ref logger);
                if (enquiry == null)
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "未获取到 enquiry";
                    context.PregnantService.SaveSyncOrder(syncOrder);
                    return;
                }
                //更新 问询病史数据
                var datas = new List<Data_MQDA_XWBS_SAVE>();
                var data = new Data_MQDA_XWBS_SAVE(enquiry);
                data.Update(sourceData.SourceData);
                datas.Add(data);
                //提交 问询病史
                var updateResult = context.FSService.UpdateEnquiry(userInfo, base8, datas, ref logger);
                if (!updateResult)
                {
                    syncOrder.SyncStatus = SyncStatus.Error;
                    syncOrder.ErrorMessage = "更新问询病史 失败";
                    context.PregnantService.SaveSyncOrder(syncOrder);
                    return;
                }
                //生育史
                var pregnantInfo = sourceData.SourceData;
                var pregnanthistorys = new pregnanthistories(sourceData.SourceData.pregnanthistory?.FromJson<List<pregnanthistory>>());
                //本孕修正处理
                pregnanthistorys.FixCurrentHistory(pregnantInfo, ref logger);
                //孕次排序
                pregnanthistorys.FixPregnantageIndex();
                //查询 生育史
                var pregnanthistoriesResponse = context.FSService.GetEnquiryPregnanths(userInfo, base8, ref logger);
                //新增 生育史
                var toAddHistories = pregnanthistorys.Data.Where(c => pregnanthistoriesResponse.FirstOrDefault(d => d.IssueDate == c.pregnantage) == null);
                foreach (var toAddHistory in toAddHistories)
                {
                    var toAdd = new WMH_CQBJ_CQJC_PRE_SAVE();
                    toAdd.UpdateEnquiry(pregnantInfo, toAddHistory);
                    toAdd._state = "added";
                    if (toAdd.Validate(ref logger))
                    {
                        var subAddResult = context.FSService.AddEnquiryPregnanth(toAdd, userInfo, base8, ref logger);
                        if (!subAddResult)
                        {
                            syncOrder.SyncStatus = SyncStatus.Error;
                            syncOrder.ErrorMessage = "处理 AddEnquiryPregnanth 失败";
                            context.PregnantService.SaveSyncOrder(syncOrder);
                            return;
                        }
                    }
                }
                //更新 or 删除 生育史
                foreach (var enquiryPregnanth in pregnanthistoriesResponse)
                {
                    var toChange = new WMH_CQBJ_CQJC_PRE_SAVE(enquiryPregnanth);
                    var pregnanthistory = pregnanthistorys.Data.FirstOrDefault(c => c.pregnantage == enquiryPregnanth.IssueDate);
                    if (pregnanthistory == null)
                    {
                        var subDeleteResult = context.FSService.DeleteEnquiryPregnanth(toChange, userInfo, base8, ref logger);
                        if (!subDeleteResult)
                        {
                            syncOrder.SyncStatus = SyncStatus.Error;
                            syncOrder.ErrorMessage = "处理 DeleteEnquiryPregnanth 失败";
                            context.PregnantService.SaveSyncOrder(syncOrder);
                            return;
                        }
                        continue;
                    }
                    //更改
                    toChange.UpdateEnquiry(pregnantInfo, pregnanthistory);
                    toChange._state = "modified";
                    if (toChange.Validate(ref logger))
                    {
                        var subUpdateResult = context.FSService.UpdateEnquiryPregnanth(toChange, userInfo, base8, ref logger);
                        if (!subUpdateResult)
                        {
                            syncOrder.SyncStatus = SyncStatus.Error;
                            syncOrder.ErrorMessage = "处理 UpdateEnquiryPregnanth 失败";
                            context.PregnantService.SaveSyncOrder(syncOrder);
                            return;
                        }
                    }
                }
                //更新同步计划
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
