using System;
using System.Collections.Generic;

namespace FrameworkTest.Business.TaskScheduler
{
    public class WorkTask : TaskConfig
    {
        public WorkTask(TaskConfig config)
        {
            this.Id = config.Id;
            this.Name = config.Name;
            this.FreqencyType = config.FreqencyType;
            this.Interval = config.Interval;
            this.IsActivated = config.IsActivated;
            this.TaskType = config.TaskType;

            DateTime nextTime = GetNextExecuteTime(DateTime.Now);
            this.NextExecuteTime = nextTime;
        }

        public bool NeedWork { get { return NextExecuteTime < DateTime.Now; } }
        private DateTime GetNextExecuteTime(DateTime pre)
        {
            var nextTime = pre;
            switch (FreqencyType)
            {
                case FreqencyType.None:
                    break;
                case FreqencyType.每一天:
                    nextTime = nextTime.AddDays(1);
                    break;
                case FreqencyType.间隔:
                    nextTime = nextTime.AddSeconds(Interval);
                    break;
                default:
                    nextTime = DateTime.MaxValue;
                    break;
            }
            return nextTime;
        }

        public DateTime NextExecuteTime { set; get; }
        public int ErrorCount { get; internal set; }
        public const int MaxErrorCount = 3;

        internal List<string> Validate()
        {
            List<string> messages = new List<string>();
            if (string.IsNullOrEmpty(Name))
            {
                messages.Add($"无效的任务名称:{Name}");
            }
            if (FreqencyType == FreqencyType.None)
            {
                messages.Add($"任务名称:{Name},无效的周期:{FreqencyType.ToString()}");
            }
            switch (FreqencyType)
            {
                case FreqencyType.None:
                    break;
                case FreqencyType.每一天:
                case FreqencyType.每小时:
                case FreqencyType.每分钟:
                    break;
                case FreqencyType.间隔:
                    messages.Add($"任务名称:{Name},间隔型任务,执行间隔{Interval}秒");
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
            work?.Invoke();
        }
    }
}
