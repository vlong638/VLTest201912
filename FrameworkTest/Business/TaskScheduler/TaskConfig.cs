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
        public FreqencyType FreqencyType { set; get; }
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
            FreqencyType = element.Attribute(nameof(FreqencyType)).Value.ToEnum<FreqencyType>();
            IsActivated = element.Attribute(nameof(IsActivated)).Value.ToBool();
            TaskType = element.Attribute(nameof(TaskType)).Value.ToEnum<TaskType>();
        }

        public XElement ToXElement()
        {
            var node = new XElement(NodeElementName);
            node.SetAttributeValue(nameof(Id), Id);
            node.SetAttributeValue(nameof(Name), Name);
            node.SetAttributeValue(nameof(FreqencyType), FreqencyType);
            node.SetAttributeValue(nameof(IsActivated), IsActivated);
            node.SetAttributeValue(nameof(TaskType), TaskType);
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
