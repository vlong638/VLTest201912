﻿using System.Xml.Linq;
using VL.Consolo_Core.Common.ValuesSolution;

namespace BBee.Common
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
        Enum = 4,
        JsonEnum = 5,
    }

    public class ListConfigProperty
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

        #region 显示配置
        /// <summary>
        /// 列名(显示)
        /// </summary>
        public string DisplayName { set; get; }
        /// <summary>
        /// 显示方案
        /// </summary>
        public DisplayType DisplayType { set; get; }
        /// <summary>
        /// 枚举类型
        /// </summary>
        public string EnumType { set; get; }
        /// <summary>
        /// 显示宽度
        /// </summary>
        public string DisplayWidth { set; get; }
        /// <summary>
        /// 可排序的
        /// </summary>
        public bool IsSortable { set; get; }
        /// <summary>
        /// 可选中的
        /// </summary>
        public bool IsCheckable { set; get; }
        #endregion

        #region 数据配置
        /// <summary>
        /// 是否在详情页显示
        /// </summary>
        public bool IsNeedOnPage { set; get; }
        /// <summary>
        /// 是否从数据库读取
        /// </summary>
        public bool IsNeedOnDatabase { set; get; }
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

        #region 多级结构配置
        /// <summary>
        ///支持以下结构  二级叶子需要为true
        /// ------
        /// | ---
        /// | | |
        /// </summary>
        public bool? ColGroup { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int? DisplayLevel { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int? RowSpan { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int? ColumnSpan { set; get; }

        #endregion

        public ListConfigProperty(DBListConfig dbConfig)
        {
            DisplayLevel= null;
            RowSpan = null;
            ColumnSpan = null;
            ColumnName = dbConfig.ColumnName;
            DisplayName = "";
            DisplayType = DisplayType.None;
            EnumType = "";
            DataType = dbConfig.DataType;
            DisplayWidth = "100";
            IsSortable = false;
            IsCheckable = false;
            IsNeedOnPage = false;
            IsNeedOnDatabase = false;
            Description = dbConfig.Description;
        }
        public ListConfigProperty(XElement element)
        {
            DisplayLevel= element.Attribute(nameof(DisplayLevel))?.Value.ToInt();
            RowSpan = element.Attribute(nameof(RowSpan))?.Value.ToInt();
            ColumnSpan = element.Attribute(nameof(ColumnSpan))?.Value.ToInt();
            ColGroup = element.Attribute(nameof(ColGroup))?.Value.ToBool();
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            DisplayName = element.Attribute(nameof(DisplayName))?.Value;
            DisplayType = element.Attribute(nameof(DisplayType))?.Value.ToEnum<DisplayType>() ?? DisplayType.None;
            EnumType = element.Attribute(nameof(EnumType))?.Value;
            DisplayWidth = element.Attribute(nameof(DisplayWidth))?.Value;
            IsSortable = element.Attribute(nameof(IsSortable))?.Value.ToBool() ?? false;
            IsCheckable = element.Attribute(nameof(IsCheckable))?.Value.ToBool() ?? false;
            DataType = element.Attribute(nameof(DataType))?.Value;
            IsNeedOnPage = element.Attribute(nameof(IsNeedOnPage))?.Value.ToBool() ?? false;
            IsNeedOnDatabase = element.Attribute(nameof(IsNeedOnDatabase))?.Value.ToBool() ?? false;
            Description = element.Attribute(nameof(Description))?.Value;
        }

        public XElement ToXElement()
        {
            var property = new XElement(ElementName);
            property.SetAttributeValue(nameof(IsNeedOnPage), IsNeedOnPage);
            property.SetAttributeValue(nameof(IsNeedOnDatabase), IsNeedOnDatabase);
            property.SetAttributeValue(nameof(DisplayName), DisplayName);
            property.SetAttributeValue(nameof(EnumType), EnumType);
            property.SetAttributeValue(nameof(ColumnName), ColumnName);
            property.SetAttributeValue(nameof(DisplayType), DisplayType.ToString());
            property.SetAttributeValue(nameof(DisplayWidth), DisplayWidth.ToString());
            property.SetAttributeValue(nameof(IsSortable), IsSortable.ToString());
            property.SetAttributeValue(nameof(IsCheckable), IsCheckable.ToString());
            property.SetAttributeValue(nameof(DataType), DataType);
            property.SetAttributeValue(nameof(Description), Description);
            return property;
        }
    }
}
