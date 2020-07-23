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

    //public class Enquiry_SyncTask_Create : SyncTask<Enquiry_SourceData>
    //{
    //    public Enquiry_SyncTask_Create(ServiceContext context) : base(context)
    //    {
    //    }

    //    public override List<Enquiry_SourceData> GetSourceDatas(UserInfo userInfo)
    //    {
    //        Context.ESBService.UpdateEnquiry();
    //        return Context.ESBService.GetEnquirysToCreate().Select(c => new Enquiry_SourceData(c)).ToList();
    //    }

    //    public override void DoWork(ServiceContext context, UserInfo userInfo, Enquiry_SourceData sourceData, ref StringBuilder logger)
    //    {
    //        var syncOrder = new SyncOrder()
    //        {
    //            SourceId = sourceData.SourceId,
    //            TargetType = sourceData.TargetType,
    //            SyncTime = DateTime.Now,
    //            SyncStatus = SyncStatus.Success,
    //        };
    //        try
    //        {
    //            //获取列表数据
    //            var listData = Context.FSService.GetPregnantInHospitalList(userInfo, sourceData.inp_no, ref logger);
    //            if (listData == null)
    //            {
    //                syncOrder.SyncStatus = SyncStatus.Error;
    //                syncOrder.ErrorMessage = "未获取到 列表数据";
    //                context.ESBService.SaveSyncOrder(syncOrder);
    //                return;
    //            }
    //            //获取住院数据
    //            var EnquiryData = Context.FSService.GetEnquiry(userInfo, listData.FMMainId, ref logger);
    //            //数据更新
    //            var EnquiryToCreate = new CQJL_WOMAN_FORM_SAVE_Data();
    //            if (EnquiryData != null)
    //            {
    //                //EnquiryToCreate.Update(EnquiryData);
    //                syncOrder.SyncStatus = SyncStatus.Existed;
    //                syncOrder.ErrorMessage = SyncStatus.Existed.GetDescription();
    //                context.ESBService.SaveSyncOrder(syncOrder);
    //                return;
    //            }
    //            else
    //            {
    //                EnquiryToCreate.Init(userInfo, sourceData, listData.FMMainId);
    //            }
    //            EnquiryToCreate.Update(sourceData);
    //            //创建住院数据
    //            var result = Context.FSService.SaveEnquiry(userInfo, listData, EnquiryToCreate, EnquiryData?.DischargeId ?? "null", ref logger);
    //            //保存同步记录
    //            context.ESBService.SaveSyncOrder(syncOrder);
    //        }
    //        catch (Exception ex)
    //        {
    //            logger.AppendLine("出现异常:" + ex.ToString());

    //            syncOrder.SyncStatus = SyncStatus.Error;
    //            syncOrder.ErrorMessage = ex.ToString();
    //            context.PregnantService.SaveSyncOrder(syncOrder);
    //        }
    //        finally
    //        {
    //            logger.AppendLine(">>>syncOrder.ErrorMessage");
    //            logger.AppendLine(syncOrder.ErrorMessage);
    //            logger.AppendLine(">>>syncOrder.ToJson()");
    //            logger.AppendLine(syncOrder.ToJson());
    //        }
    //    }
    //}


    public class Enquiry_SyncTask_Create 
    {
        public void Start_Auto_DoWork()
        {
            while (true)
            {
                var userInfo = SDBLL.UserInfo;
                var tempPregnantInfos = SDBLL.GetPregnantInfosToCreateEnquiries();
                StringBuilder sb = new StringBuilder();
                foreach (var pregnantInfo in tempPregnantInfos)
                {
                    //记录待处理的病人
                    sb.AppendLine(pregnantInfo.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\To-Create-问询病史" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                    //业务处理
                    var context = DBHelper.GetDbContext(SDBLL.ConntectingStringSD);
                    var serviceResult = context.DelegateTransaction((group) =>
                    {
                        var syncForFS = new SyncOrder()
                        {
                            TargetType = TargetType.HistoryEnquiry,
                            SourceId = pregnantInfo.Id.ToString(),
                            SyncStatus = SyncStatus.Success
                        };
                        try
                        {
                            syncForFS.SyncTime = DateTime.Now;
                            var base8 = SDBLL.GetBase8(userInfo, pregnantInfo.idcard, ref sb);
                            if (!base8.IsAvailable)
                            {
                                syncForFS.SyncStatus = SyncStatus.Error;
                                syncForFS.ErrorMessage = "No Base8 Data";
                                SDBLL.SaveSyncOrder(context.DbGroup, syncForFS);
                                return (bool)true;
                            }
                            var enquiryResponse = SDBLL.GetEnquiry(userInfo, base8, ref sb);
                            var enquiryData = enquiryResponse.data.FirstOrDefault();
                            var result = SDBLL.PostUpdateEnquiryToFS(pregnantInfo, enquiryData, userInfo, base8, ref sb);
                            if (!result.Contains((string)"处理成功"))
                            {
                                throw new NotImplementedException(result);
                            }
                            //新增处理
                            var pregnanthistorys = pregnantInfo.pregnanthistory?.FromJson<List<pregnanthistory>>();
                            //本孕
                            if (pregnantInfo.gravidity == "1")
                            {
                                if (pregnanthistorys.Count == 0)
                                {
                                    pregnanthistorys.Add(new pregnanthistory()
                                    {
                                        index = "1",
                                        pregnantage = $"本孕{DateTime.Now.Year}- ",
                                    });
                                    sb.Append("add 本孕");
                                }
                                else if (pregnanthistorys.Count == 1)
                                {
                                    if (string.IsNullOrEmpty(pregnanthistorys[0].index))
                                    {
                                        pregnanthistorys[0].index = "1";
                                    }
                                    if (string.IsNullOrEmpty(pregnanthistorys[0].pregnantage))
                                    {
                                        pregnanthistorys[0].pregnantage = $"本孕{DateTime.Now.Year}- ";
                                    }
                                    sb.Append("孕次1一记录分支");
                                }
                                else
                                {
                                    sb.Append("孕次1多记录分支");
                                }
                            }
                            else
                            {
                                pregnanthistorys.Add(new pregnanthistory()
                                {
                                    index = "",
                                    pregnantage = $"{DateTime.Now.Year}",
                                });
                                sb.Append("add 本孕2");
                            }
                            //孕次排序
                            pregnanthistorys = pregnanthistorys.OrderBy(c => c.pregnantage).ToList();
                            foreach (var pregnanthistory in pregnanthistorys)
                            {
                                pregnanthistory.PregnantageIndex = pregnanthistorys.IndexOf(pregnanthistory) + 1;
                            }
                            var enquiryPregnanthResponse = SDBLL.GetEnquiryPregnanths(userInfo, base8, ref sb);
                            sb.Append("---------------------pregnantInfo.pregnanthistory");
                            sb.Append(pregnantInfo.pregnanthistory);
                            var toAddHistories = pregnanthistorys.Where(c => enquiryPregnanthResponse.data.FirstOrDefault(d => d.IssueDate == c.pregnantage) == null);
                            foreach (var toAddHistory in toAddHistories)
                            {
                                var toAdd = new WMH_CQBJ_CQJC_PRE_SAVE();
                                toAdd.UpdateEnquiry(pregnantInfo, toAddHistory);
                                toAdd._state = "added";
                                if (toAdd.Validate(ref sb))
                                {
                                    result = SDBLL.PostAddEnquiryPregnanth(toAdd, userInfo, base8, ref sb);
                                }
                                if (!result.Contains((string)"处理成功"))
                                {
                                    throw new NotImplementedException(result);
                                }
                            }
                            foreach (var enquiryPregnanth in enquiryPregnanthResponse.data)
                            {
                                var toChange = new WMH_CQBJ_CQJC_PRE_SAVE(enquiryPregnanth);
                                var pregnanthistory = pregnanthistorys.FirstOrDefault(c => c.pregnantage == enquiryPregnanth.IssueDate);
                                if (pregnanthistory == null)
                                {
                                    //删除
                                    result = SDBLL.DeleteEnquiryPregnanth(toChange, userInfo, base8, ref sb);
                                    if (!result.Contains((string)"处理成功"))
                                    {
                                        throw new NotImplementedException(result);
                                    }
                                    continue;
                                }
                                //更改
                                toChange.UpdateEnquiry(pregnantInfo, pregnanthistory);
                                toChange._state = "modified";
                                if (toChange.Validate(ref sb))
                                {
                                    result = SDBLL.UpdateEnquiryPregnanth(toChange, userInfo, base8, ref sb);
                                }
                                if (!result.Contains((string)"处理成功"))
                                {
                                    throw new NotImplementedException(result);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            syncForFS.SyncStatus = SyncStatus.Error;
                            syncForFS.ErrorMessage = ex.ToString();
                        }
                        syncForFS.Id = SDBLL.SaveSyncOrder(context.DbGroup, syncForFS);
                        sb.AppendLine((string)syncForFS.ToJson());
                        return (bool)(syncForFS.SyncStatus != SyncStatus.Error);
                    });
                    if (!serviceResult.IsSuccess)
                    {
                        sb.Append(serviceResult.Messages);
                    }
                    file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Create-问询病史" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
                System.Threading.Thread.Sleep(1000 * 10);
            }
        }
    }
}
