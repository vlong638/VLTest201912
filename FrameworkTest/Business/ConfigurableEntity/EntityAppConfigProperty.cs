using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FrameworkTest.ConfigurableEntity
{
    /// <summary>
    /// 显示类型
    /// </summary>
    public enum DisplayType
    {
        None = 0,
        Hidden = 1,
        TextString = 2,
        TextInt = 21,
        TextDecimal = 22,
        Date = 3,
        DateTime = 31,
    }

    class EntityAppConfigProperty
    {
        public static string ElementName = "Property";

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 说明(显示,编辑时易理解)
        /// </summary>
        public string Description { set; get; }

        #region 显示设置
        /// <summary>
        /// 列名(显示)
        /// </summary>
        public string DisplayName { set; get; }
        /// <summary>
        /// 显示方案
        /// </summary>
        public DisplayType DisplayType { set; get; } 
        #endregion

        #region 页面控制开关
        /// <summary>
        /// 是否在列表页显示
        /// </summary>
        public bool IsNeedOnList { set; get; }
        /// <summary>
        /// 是否在详情页显示
        /// </summary>
        public bool IsNeedOnDetail { set; get; }
        #endregion

        #region 数据格式及校验
        /// <summary>
        /// 数据类型(显示,校验)
        /// </summary>
        public string DataType { set; get; }
        ///// <summary>
        ///// 最大值(校验)
        ///// </summary>
        //public long? MaxValue { set; get; }
        ///// <summary>
        ///// 最小值(校验)
        ///// </summary>
        //public long? MinValue { set; get; }
        ///// <summary>
        ///// 是否允许空(校验)
        ///// </summary>
        //public bool IsRequired { set; get; } 
        #endregion

        public EntityAppConfigProperty(EntityDBConfig dbConfig)
        {
            ColumnName = dbConfig.ColumnName;
            DisplayName = "";
            DisplayType = DisplayType.None;
            DataType = dbConfig.DataType;
            IsNeedOnList = false;
            IsNeedOnDetail = false;
            Description = dbConfig.Description;
        }
        public EntityAppConfigProperty(XElement element)
        {
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            DisplayName = element.Attribute(nameof(DisplayName))?.Value;
            DisplayType = element.Attribute(nameof(DisplayType))?.Value.ToEnum<DisplayType>() ?? DisplayType.None;
            DataType = element.Attribute(nameof(DataType))?.Value;
            IsNeedOnList = element.Attribute(nameof(IsNeedOnList))?.Value.ToBool() ?? false;
            IsNeedOnDetail = element.Attribute(nameof(IsNeedOnDetail))?.Value.ToBool() ?? false;
            Description = element.Attribute(nameof(Description))?.Value;
        }

        public XElement ToXElement()
        {
            var property = new XElement(ElementName);
            property.SetAttributeValue(nameof(ColumnName), ColumnName);
            property.SetAttributeValue(nameof(DisplayName), DisplayName);
            property.SetAttributeValue(nameof(IsNeedOnList), IsNeedOnList);
            property.SetAttributeValue(nameof(IsNeedOnDetail), IsNeedOnDetail);
            property.SetAttributeValue(nameof(DataType), DataType);
            property.SetAttributeValue(nameof(Description), Description);
            return property;
        }
    }
}
