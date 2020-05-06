using CommonLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VL.WindowsFormsApp.utils;

namespace VL.WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        public VLScheduler VLScheduler { set; get; }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadtask();
            VLScheduler = new VLScheduler();
            VLScheduler.LogInfo += SetText;
            VLScheduler.Start();
        }

        private System.Action WorkingThread()
        {
            return () =>
            {
                SetText("已启动执行进程");
                while (Form1.IsWorking)
                {
                    //报告输出
                    foreach (var Message in Messages)
                    {
                        SetText(Message);
                    }
                    Messages.Clear();
                    //任务运行
                    if (Form1.WorkingTasks.FirstOrDefault(c => c.NeedWork) != null)
                    {
                        Messages.Add($"--------------------------------------------------------------");
                    }
                    for (int i = 0; i < Form1.WorkingTasks.Count; i++)
                    {
                        var item = Form1.WorkingTasks[i];
                        if (item.NeedWork)
                        {
                            try
                            {
                                item.Work();
                                Messages.Add($"下次执行时间:{item.NextExecuteTime},{DateTime.Now},{item.Name} worked");
                            }
                            catch (Exception ex)
                            {
                                Messages.Add(ex.ToString());

                                item.ErrorCount++;
                                if (item.ErrorCount > 3)
                                {
                                    Messages.Add($"任务项:{item.Name},故障次数达到上限限制,停止任务");
                                    WorkingTasks.Remove(item);
                                }
                            }
                        }
                    }

                    System.Threading.Thread.Sleep(3 * 1000);
                }
            };
        }

        private void loadtask()
        {
            DataTable dt = gettasks();
            dgv_task.DataSource = dt;
        }

        private DataTable gettasks()
        {
            XmlHelper xh = new XmlHelper(Global.taskfile);
            return xh.GetData("TaskList");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartTasks();
        }

        private void StartTasks()
        {
            var taskConfigs = GetTaskConfigs();
            VLScheduler.SetTasks(taskConfigs);
        }

        delegate void SetTextCallBack(string text);
        private void SetText(string text)
        {
            if (this.lb.InvokeRequired)
            {
                SetTextCallBack stcb = new SetTextCallBack(SetText);
                this.Invoke(stcb, new object[] { text });
            }
            else
            {
                lb.Items.Add(text);
            }
        }

        public static bool IsWorking = true;
        private static List<WorkTask> WorkingTasks= new List<WorkTask>();
        public static List<string> Messages = new List<string>();

        private List<TaskConfig> GetTaskConfigs()
        {
            var taskConfigs = new List<TaskConfig>();
            DataTable dt = (DataTable)dgv_task.DataSource;
            foreach (DataRow row in dt.Rows)
            {
                int.TryParse(row["id"] + "", out int id);
                var text = row["text"] + "";
                Enum.TryParse(row["freq"] + "", out Freqency freq);
                DateTime.TryParse(row["sendtime"] + "", out DateTime sendtime);
                bool.TryParse(row["run"] + "", out bool run);
                Enum.TryParse(row["tasktype"] + "", out TaskType tasktype);
                taskConfigs.Add(new TaskConfig()
                {
                    Id = id,
                    Name = text,
                    Freqency = freq,
                    Time = sendtime,
                    IsRun = run,
                    TaskType = tasktype,
                });
            }
            return taskConfigs;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearTasks();
        }

        private void ClearTasks()
        {
            WorkingTasks.Clear();
            SetText("任务已清理");
        }
    }

    public class VLScheduler
    {
        public static List<WorkTask> WorkingTasks = new List<WorkTask>();
        public static List<string> Messages = new List<string>();
        public bool IsWorking { set; get; }

        public void Start()
        {
            IsWorking = true;
            Task.Factory.StartNew(WorkingThread());
        }
        private System.Action WorkingThread()
        {
            return () =>
            {
                LogInfo?.Invoke("已启动执行进程");
                while (IsWorking)
                {
                    //报告输出
                    foreach (var Message in Messages)
                    {
                        LogInfo?.Invoke(Message);
                    }
                    Messages.Clear();
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
                                if (item.ErrorCount > 3)
                                {
                                    Messages.Add($"任务项:{item.Name},故障次数达到上限限制,停止任务");
                                    WorkingTasks.Remove(item);
                                }
                            }
                        }
                    }

                    System.Threading.Thread.Sleep(3 * 1000);
                }
            };
        }

        public delegate void DoLog(string message);
        public event DoLog LogInfo;

        public void Stop()
        {
            IsWorking = false;
        }

        internal void SetTasks(List<TaskConfig> taskConfigs)
        {
            var workTasks = taskConfigs.Where(c => c.IsRun)
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

    public enum CommandType
    {
        None,
        Start,
        Stop,
    }
    public class WorkTask : TaskConfig
    {
        public WorkTask(TaskConfig config)
        {
            this.Id = config.Id;
            this.Name = config.Name;
            this.Freqency = config.Freqency;
            this.Time = config.Time;
            this.IsRun = config.IsRun;
            this.TaskType = config.TaskType;

            DateTime nextTime = GetNextExecuteTime(Freqency == Freqency.间隔 ? DateTime.Now : Time.Value);
            this.NextExecuteTime = nextTime;
        }

        public bool NeedWork { get { return NextExecuteTime < DateTime.Now; } }
        private DateTime GetNextExecuteTime(DateTime pre)
        {
            var nextTime = pre;
            switch (Freqency)
            {
                case Freqency.None:
                    break;
                case Freqency.每一天:
                    nextTime = nextTime.AddDays(1);
                    break;
                case Freqency.间隔:
                    nextTime = nextTime.AddHours(Time.Value.Hour);
                    nextTime = nextTime.AddMinutes(Time.Value.Minute);
                    nextTime = nextTime.AddSeconds(Time.Value.Second);
                    break;
                default:
                    nextTime = DateTime.MaxValue;
                    break;
            }
            return nextTime;
        }

        public DateTime NextExecuteTime { set; get; }
        public int ErrorCount { get; internal set; }

        internal List<string> Validate()
        {
            List<string> messages = new List<string>();
            if (string.IsNullOrEmpty(Name))
            {
                messages.Add($"无效的任务名称:{Name}");
            }
            if (Freqency==Freqency.None)
            {
                messages.Add($"任务名称:{Name},无效的周期:{Freqency.ToString()}");
            }
            switch (Freqency)
            {
                case Freqency.None:
                    break;
                case Freqency.每一天:
                case Freqency.每小时:
                case Freqency.每分钟:
                    if (!Time.HasValue)
                    {
                        messages.Add($"任务名称:{Name},每x任务,使用执行时间作为间隔,格式{{HH:mm:ss}}");
                    }
                    break;
                case Freqency.间隔:
                    if (!Time.HasValue)
                    {
                        messages.Add($"任务名称:{Name},间隔型任务,使用执行时间作为间隔,格式{{HH:mm:ss}}");
                    }
                    break;
                default:
                    break;
            }
            if (TaskType == TaskType.None)
            {
                messages.Add($"任务名称:{Name},无效的任务类型:{TaskType.ToString()}");
            }
            return messages;
        }

        internal void Work(System.Action work = null)
        {
            NextExecuteTime = GetNextExecuteTime(DateTime.Now);
            if (work != null)
                work();
        }
    }
    public class TaskConfig
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public Freqency Freqency { set; get; }
        /// <summary>
        /// 混用并不好
        /// 对于每天的,它是首次启动时间
        /// 对于间隔的,它是间隔时间
        /// </summary>
        public DateTime? Time { set; get; }
        public bool IsRun { set; get; }
        public TaskType TaskType { set; get; }
    }
    public enum Freqency
    {
        None = 0,
        每一天,
        每小时,
        每分钟,
        间隔,
    }
    public enum TaskType
    {
        None = 0,
        定时任务,
        队列,
    }
}



