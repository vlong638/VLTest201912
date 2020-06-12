using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FrameworkTest.Business.TaskScheduler
{
    public class VLScheduler
    {
        public static List<TaskConfig> TaskConfigs = new List<TaskConfig>();
        public static List<WorkTask> WorkingTasks = new List<WorkTask>();
        public static List<string> Messages = new List<string>();
        public bool IsWorking { set; get; }

        public void Start()
        {
            IsWorking = true;

            LoadTasks();
            Task.Factory.StartNew(DoWorkTask());
        }
        private System.Action DoWorkTask()
        {
            return () =>
            {
                LogInfo?.Invoke("已启动执行进程");
                while (IsWorking)
                {
                    //任务运行
                    if (WorkingTasks.FirstOrDefault(c => c.NeedWork) != null)
                    {
                        Messages.Add($"--------------------------------------------------------------");
                    }
                    for (int i = 0; i < WorkingTasks.Count; i++)
                    {
                        var item = WorkingTasks[i];
                        if (item.NeedWork)
                        {
                            try
                            {
                                item.Work();
                                Messages.Add($"{DateTime.Now},{item.Name}:下次执行时间:{item.NextExecuteTime}");
                            }
                            catch (Exception ex)
                            {
                                Messages.Add(ex.ToString());

                                item.ErrorCount++;
                                if (item.ErrorCount > WorkTask.MaxErrorCount)
                                {
                                    Messages.Add($"任务项:{item.Name},故障次数达到上限限制,停止任务");
                                    WorkingTasks.Remove(item);
                                }
                            }
                        }
                    }
                    //报告输出
                    foreach (var Message in Messages)
                    {
                        LogInfo?.Invoke(Message);
                    }
                    Messages.Clear();

                    System.Threading.Thread.Sleep(1000);
                }
            };
        }

        public delegate void DoLog(string message);
        public event DoLog LogInfo;

        public void Stop()
        {
            IsWorking = false;
        }

        internal void LoadTasks()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Configs\\TaskConfigs.xml");
            XDocument doc = XDocument.Load(path);
            var nodes = doc.Descendants(TaskConfig.NodeElementName);
            var configs = nodes.Select(c => new TaskConfig(c)).ToList();
            TaskConfigs = configs;
            var workTasks = configs.Where(c => c.IsActivated)
                .Select(c => new WorkTask(c))
                .ToList();
            foreach (var workTask in workTasks)
            {
                var messages = workTask.Validate();
                if (messages.Count > 0)
                {
                    foreach (var message in messages)
                    {
                        LogInfo(message);
                        return;
                    }
                }
            }
            WorkingTasks = workTasks;
            foreach (var WorkingTask in WorkingTasks)
            {
                LogInfo($@"首次执行时间:{WorkingTask.NextExecuteTime},{WorkingTask.Name}");
            }
        }
    }
}
