using FrameworkTest.Business.SDMockCommit;
using FrameworkTest.Common.FileSolution;
using FrameworkTest.Common.ValuesSolution;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest
{
    class AutoTasks
    {
        //static void DelegateEvent(string taskName, ref DateTime lastWorkTime, Action doSomething)
        //{
        //    if (lastWorkTime < DateTime.Now.AddSeconds(-10))
        //    {
        //        Console.WriteLine(taskName + DateTime.Now);
        //        Task.Factory.StartNew(() =>
        //        {
        //            try
        //            {
        //                doSomething();
        //            }
        //            catch (Exception ex)
        //            {
        //                Log4NetLogger.Warn("任务出现异常", ex);
        //            }
        //        });
        //    }
        //}

        //static DateTime LastWorkTimeOf_PregnantInfo_SyncTask_Create;
        //static DateTime LastEditTimeOf_PregnantInfo_SyncTask_Create;
        //static DateTime LastEditTimeOf_PregnantInfo_SyncTask_Create;
        //static DateTime LastEditTimeOf_PregnantInfo_SyncTask_Create;
        //static DateTime LastEditTimeOf_PregnantInfo_SyncTask_Create;
        //static DateTime LastEditTimeOf_PregnantInfo_SyncTask_Create;
        //static DateTime LastEditTimeOf_PregnantInfo_SyncTask_Create;
        //static DateTime LastEditTimeOf_PregnantInfo_SyncTask_Create;
        //static DateTime LastEditTimeOf_PregnantInfo_SyncTask_Create;


        static void Main(string[] args)
        {
            //LastWorkTimeOf_PregnantInfo_SyncTask_Create = DateTime.Now.AddSeconds(-10);
            //while (true)
            //{
            //    Console.WriteLine("任务存活检测");

            //    DelegateEvent("孕妇档案-新建", ref LastWorkTimeOf_PregnantInfo_SyncTask_Create, () =>
            //    {
            //        LastWorkTimeOf_PregnantInfo_SyncTask_Create = DateTime.Now;
            //        Console.WriteLine("异常终止,{0}", LastWorkTimeOf_PregnantInfo_SyncTask_Create);
            //        throw new NotImplementedException("222");

            //        var context = new ServiceContext();
            //        var syncTask = new PregnantInfo_SyncTask_Create(context);
            //        syncTask.DoLogOnGetSource = (sourceData) =>
            //        {
            //            StringBuilder sb = new StringBuilder();
            //            sb.AppendLine(sourceData.ToJson());
            //            var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
            //            File.WriteAllText(file, sb.ToString());
            //            Console.WriteLine($"result:{file}");
            //        };
            //        syncTask.DoLogOnWork = (sourceData, sb) =>
            //        {
            //            var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
            //            File.WriteAllText(file, sb.ToString());
            //            Console.WriteLine($"result:{file}");
            //        };
            //        syncTask.Start_Auto_DoWork(context, SDBLL.UserInfos, 30 * 1000);
            //    });

            //    System.Threading.Thread.Sleep(3 * 1000);
            //}
            //Console.ReadLine();
            //return;

            Console.WriteLine($"任务启动=>孕妇档案-新建");
            Task.Factory.StartNew(() =>
            {
                var context = new ServiceContext();
                var syncTask = new PregnantInfo_SyncTask_Create(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfos, 30 * 1000);
            });

            Console.WriteLine($"任务启动=>孕妇档案-更新");
            Task.Factory.StartNew(() =>
            {
                var context = new ServiceContext();
                var syncTask = new PregnantInfo_SyncTask_Update(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfos, 30 * 1000);
            });

            Console.WriteLine($"任务启动=>问询病史-新建");
            Task.Factory.StartNew(() =>
            {
                var context = new ServiceContext();
                var syncTask = new Enquiry_SyncTask_Create(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-问询病史-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-问询病史-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfos, 30 * 1000);
            });

            Console.WriteLine($"任务启动=>问询病史-更新");
            Task.Factory.StartNew(() =>
            {
                var context = new ServiceContext();
                var syncTask = new Enquiry_SyncTask_Update(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-问询病史-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-问询病史-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfos, 30 * 1000);
            });

            Console.WriteLine($"任务启动=>体格检查-新建");
            Task.Factory.StartNew(() =>
            {
                var context = new ServiceContext();
                var syncTask = new PhysicalExaminationModel_SyncTask_Create(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-体格检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.pi_personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-体格检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.pi_personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfos, 30 * 1000);
            });

            Console.WriteLine($"任务启动=>体格检查-更新");
            Task.Factory.StartNew(() =>
            {
                var context = new ServiceContext();
                var syncTask = new PhysicalExaminationModel_SyncTask_Update(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-体格检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.pi_personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-体格检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.pi_personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfos, 30 * 1000);
            });

            Console.WriteLine($"任务启动=>专科检查-新建");
            Task.Factory.StartNew(() =>
            {
                var context = new ServiceContext();
                var syncTask = new ProfessionalExaminationModel_SyncTask_Create(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-专科检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-专科检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfos, 30 * 1000);
            });

            Console.WriteLine($"任务启动=>专科检查-更新");
            Task.Factory.StartNew(() =>
            {
                var context = new ServiceContext();
                var syncTask = new ProfessionalExaminationModel_SyncTask_Update(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-专科检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-专科检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfos, 30 * 1000);
            });

            Console.WriteLine($"任务启动=>孕妇出院-新增");
            Task.Factory.StartNew(() =>
            {
                var context = new ServiceContext();
                var syncTask = new PregnantDischarge_SyncTask_Create(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-孕妇出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-孕妇出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfos, 30 * 1000);
            });

            Console.WriteLine($"任务启动=>孕妇出院-更新");
            Task.Factory.StartNew(() =>
            {
                var context = new ServiceContext();
                var syncTask = new PregnantDischarge_SyncTask_Update(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-孕妇出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.inp_no + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-孕妇出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.inp_no + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfos, 30 * 1000);
            });

            Console.WriteLine($"任务启动=>婴儿出院-新增");
            Task.Factory.StartNew(() =>
            {
                var context = new ServiceContext();
                var syncTask = new ChildDischarge_SyncTask_Create(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Create-婴儿出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Create-婴儿出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfos, 30 * 1000);
            });

            Console.WriteLine($"任务启动=>婴儿出院-更新");
            Task.Factory.StartNew(() =>
            {
                var context = new ServiceContext();
                var syncTask = new ChildDischarge_SyncTask_Update(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\To-Update-婴儿出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectory("SyncLog\\Update-婴儿出院-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.idcard + "_" + sourceData.inp_no + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfos, 30 * 1000);
            });
            Console.ReadLine();
        }
    }
}




