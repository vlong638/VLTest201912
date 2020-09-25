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
    public class Enquiry_SyncTask_Update2
    {
        public void Start_Auto_DoWork()
        {
            while (true)
            {
                var userInfo = SDBLL.UserInfo;
                var tempPregnantInfos = SDBLL.GetPregnantInfosToUpdateEnquiries();
                foreach (var pregnantInfo in tempPregnantInfos)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(pregnantInfo.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\ToUpdate_问询病史" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
                var context = DBHelper.GetSqlDbContext(SDBLL.ConntectingStringSD);
                foreach (var pregnantInfo in tempPregnantInfos)
                {
                    StringBuilder sb = new StringBuilder();
                    var serviceResult = context.DelegateTransaction((Func<DbGroup, bool>)((group) =>
                    {
                        var syncForFS = SDBLL.GetSyncOrder((DbGroup)context.DbGroup, (TargetType)TargetType.HistoryEnquiry, (string)pregnantInfo.Id.ToString());
                        try
                        {
                            syncForFS.SyncTime = DateTime.Now;
                            var base8 = SDBLL.GetBase8((UserInfo)userInfo, (string)pregnantInfo.idcard, ref sb);
                            if (!base8.IsAvailable)
                            {
                                syncForFS.SyncStatus = SyncStatus.Error;
                                syncForFS.ErrorMessage = "No Base8 Data";
                                SDBLL.SaveSyncOrder((DbGroup)context.DbGroup, (SyncOrder)syncForFS);
                                return (bool)true;
                            }
                            var enquiryResponse = SDBLL.GetEnquiry((UserInfo)userInfo, (WCQBJ_CZDH_DOCTOR_READResponse)base8, ref sb);
                            var enquiryData = enquiryResponse.data.FirstOrDefault();
                            var result = SDBLL.PostUpdateEnquiryToFS((PregnantInfo)pregnantInfo, (MQDA_READ_NEWData)enquiryData, (UserInfo)userInfo, (WCQBJ_CZDH_DOCTOR_READResponse)base8, ref sb);
                            if (!result.Contains((string)"处理成功"))
                            {
                                syncForFS.SyncStatus = SyncStatus.Error;
                                syncForFS.ErrorMessage = result;
                            }
                            //新增处理
                            var pregnanthistorys = pregnantInfo.pregnanthistory?.FromJson<List<pregnanthistory>>();//本孕
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
                        syncForFS.Id = SDBLL.SaveSyncOrder((DbGroup)context.DbGroup, (SyncOrder)syncForFS);
                        sb.AppendLine((string)syncForFS.ToJson());
                        return (bool)(syncForFS.SyncStatus != SyncStatus.Error);
                    }));
                    if (!serviceResult.IsSuccess)
                    {
                        sb.Append(serviceResult.Messages);
                    }
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Update_问询病史" + DateTime.Now.ToString("yyyy_MM_dd")), pregnantInfo.personname + "_" + pregnantInfo.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
                System.Threading.Thread.Sleep(1000 * 10);
            }
        }
    }
}
