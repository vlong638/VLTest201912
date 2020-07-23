using Dapper;
using Dapper.Contrib.Extensions;
using FrameworkTest.Business.GJPredeliveryAsync;
using FrameworkTest.Business.SDMockCommit;
using FrameworkTest.Business.TaskScheduler;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.FileSolution;
using FrameworkTest.Common.HttpSolution;
using FrameworkTest.Common.ValuesSolution;
using FrameworkTest.Common.XMLSolution;
using FrameworkTest.ConfigurableEntity;
using FrameworkTest.Kettle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;
using System.Xml.Linq;
using VL.Consoling.SemiAutoExport;

namespace FrameworkTest
{
    class AutoTasks
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"任务启动=>孕妇档案-新建");
            Task.Factory.StartNew(() =>
            {
                var context = new ServiceContext();
                var syncTask = new PregnantInfo_SyncTask_Create(context);
                syncTask.DoLogOnGetSource = (sourceData) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(sourceData.ToJson());
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\To-Create-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Create-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfo);
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
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\To-Update-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Update-孕妇档案-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfo);
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
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\To-Create-问询病史-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Create-问询病史-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfo);
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
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\To-Update-问询病史-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Update-问询病史-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfo);
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
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\To-Create-体格检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.pi_personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Create-体格检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.pi_personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfo);
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
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\To-Update-体格检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.pi_personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Update-体格检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.SourceData.pi_personname + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfo);
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
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\To-Create-专科检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Create-专科检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfo);
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
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\To-Update-专科检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.DoLogOnWork = (sourceData, sb) =>
                {
                    var file = Path.Combine(FileHelper.GetDirectoryToOutput("SyncLog\\Update-专科检查-" + DateTime.Now.ToString("yyyy_MM_dd")), sourceData.PersonName + "_" + sourceData.IdCard + ".txt");
                    File.WriteAllText(file, sb.ToString());
                    Console.WriteLine($"result:{file}");
                };
                syncTask.Start_Auto_DoWork(context, SDBLL.UserInfo);
            });

            Console.ReadLine();
        }
    }
}