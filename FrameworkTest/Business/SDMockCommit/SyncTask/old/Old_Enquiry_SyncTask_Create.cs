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
    public class Enquiry_SyncTask_Create2 
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
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-问询病史" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                    //业务处理
                    var context = DBHelper.GetSqlDbContext(SDBLL.ConntectingStringSD);
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
                    file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-问询病史" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
                System.Threading.Thread.Sleep(1000 * 10);
            }
        }
    }
}
