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
    public class PhysicalExamination_SyncTask_Update
    {
        public void Start_Auto_DoWork()
        {
            while (true)
            {
                var userInfo = SDBLL.UserInfo;
                var examinations = SDBLL.GetPhysicalExaminationDatasForUpdatePhysicalExaminations();
                foreach (var examination in examinations)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(examination.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\To-Update-体格检查" + DateTime.Now.ToString("yyyy_MM_dd")), examination.pi_personname + "_" + examination.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
                var context = DBHelper.GetDbContext(SDBLL.ConntectingStringSD);
                foreach (var examination in examinations)
                {
                    StringBuilder sb = new StringBuilder();
                    var serviceResult = context.DelegateTransaction((Func<DbGroup, bool>)((group) =>
                    {
                        var syncForFS = SDBLL.GetSyncOrder((DbGroup)context.DbGroup, (TargetType)TargetType.PhysicalExamination, examination.Id.ToString());
                        try
                        {
                            syncForFS.SyncTime = DateTime.Now;
                            //获取Base8信息
                            var base8 = SDBLL.GetBase8(userInfo, examination.idcard, ref sb);
                            if (!base8.IsAvailable)
                            {
                                syncForFS.SyncStatus = SyncStatus.Error;
                                syncForFS.ErrorMessage = "No Base8 Data";
                                SDBLL.SaveSyncOrder(context.DbGroup, syncForFS);
                                return (bool)true;
                            }
                            //获取体格检查
                            var physicalExaminationId = SDBLL.GetPhysicalExaminationId(userInfo, base8, DateTime.Now, ref sb);
                            if (string.IsNullOrEmpty(physicalExaminationId))
                            {
                                syncForFS.SyncStatus = SyncStatus.NotExisted;
                                SDBLL.SaveSyncOrder(context.DbGroup, syncForFS);
                                return (bool)true;
                            }
                            var physicalExamination = SDBLL.GetPhysicalExamination(physicalExaminationId, userInfo, base8, DateTime.Now, ref sb);
                            if (physicalExamination == null)
                            {
                                syncForFS.SyncStatus = SyncStatus.NotExisted;
                                SDBLL.SaveSyncOrder(context.DbGroup, syncForFS);
                                return (bool)true;
                            }
                            //更新体格检查
                            var datas = new List<WMH_CQBJ_CQJC_TGJC_NEW_SAVE_Data>();
                            var data = new WMH_CQBJ_CQJC_TGJC_NEW_SAVE_Data(physicalExamination);
                            data.UpdateExamination(examination);
                            datas.Add(data);
                            var result = SDBLL.UpdatePhysicalExamination(physicalExaminationId, datas, userInfo, base8, ref sb);
                            if (!result.Contains("处理成功"))
                            {
                                syncForFS.SyncStatus = SyncStatus.Error;
                                SDBLL.SaveSyncOrder(context.DbGroup, syncForFS);
                                return (bool)true;
                            }
                        }
                        catch (Exception ex)
                        {
                            syncForFS.SyncStatus = SyncStatus.Error;
                            syncForFS.ErrorMessage = ex.ToString();
                            sb.Append(ex.ToString());
                        }
                        syncForFS.Id = SDBLL.SaveSyncOrder(context.DbGroup, syncForFS);
                        sb.AppendLine((string)syncForFS.ToJson());
                        return (bool)(syncForFS.SyncStatus != SyncStatus.Error);
                    }));
                    if (!serviceResult.IsSuccess)
                    {
                        sb.Append(serviceResult.Messages);
                    }
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Update-体格检查" + DateTime.Now.ToString("yyyy_MM_dd")), examination.pi_personname + "_" + examination.idcard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                }
                System.Threading.Thread.Sleep(1000 * 10);
            }
        }
    }
}
