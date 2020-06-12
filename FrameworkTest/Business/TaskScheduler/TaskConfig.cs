using FrameworkTest.Common.ValuesSolution;
using FrameworkTest.Common.XMLSolution;
using FrameworkTest.ConfigurableEntity;
using System;
using System.Xml.Linq;

namespace FrameworkTest.Business.TaskScheduler
{
    public class TaskConfig:IXMLConfig
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public FreqencyType FrequencyType { set; get; }
        public DateTime? LastExecuteTime { set; get; }
        public string LastExecuteTimeStr
        {
            set
            {
                DateTime dt;
                DateTime.TryParse(value, out dt);
                LastExecuteTime = dt;
            }
            get { return LastExecuteTime.ToString(); }
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActivated { set; get; }
        public TaskType TaskType { set; get; }

        /// <summary>
        /// 时间间隔
        /// (适用于`间隔`周期类型)
        /// (单位:秒)
        /// </summary>
        public int Interval { set; get; }

        #region IXMLConfig

        public const string RootElementName = "Tasks";
        public const string NodeElementName = "Task";

        public TaskConfig()
        {
            
        }
        public TaskConfig(XElement element)
        {
            Id = element.Attribute(nameof(Id)).Value.ToInt().Value;
            Name = element.Attribute(nameof(Name)).Value;
            FrequencyType = element.Attribute(nameof(FrequencyType)).Value.ToEnum<FreqencyType>();
            LastExecuteTime = element.Attribute(nameof(LastExecuteTime)).Value.ToDateTime();
            IsActivated = element.Attribute(nameof(IsActivated)).Value.ToBool();
            TaskType = element.Attribute(nameof(TaskType)).Value.ToEnum<TaskType>();
            Interval = element.Attribute(nameof(Interval)).Value.ToInt().Value;
        }

        public XElement ToXElement()
        {
            var node = new XElement(NodeElementName);
            node.SetAttributeValue(nameof(Id), Id);
            node.SetAttributeValue(nameof(Name), Name);
            node.SetAttributeValue(nameof(FrequencyType), FrequencyType.ToString());
            node.SetAttributeValue(nameof(LastExecuteTime), LastExecuteTime.ToString());
            node.SetAttributeValue(nameof(IsActivated), IsActivated.ToString());
            node.SetAttributeValue(nameof(TaskType), TaskType.ToString());
            node.SetAttributeValue(nameof(Interval), Interval);
            return node;
        } 

        #endregion
    }

    /// <summary>
    /// 周期类型
    /// </summary>
    public enum FreqencyType
    {
        None = 0,
        每一天,
        每小时,
        每分钟,
        间隔,
    }

    /// <summary>
    /// 任务类型
    /// </summary>
    public enum TaskType
    {
        None = 0,
        定时任务,
        队列,
    }
}
