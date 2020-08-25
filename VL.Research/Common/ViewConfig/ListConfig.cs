using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ValuesSolution;
using VL.Research.Models;

namespace VL.Research.Common
{
    /// <summary>
    /// 页面配置
    /// </summary>
    public class ListConfig
    {
        public static string RootElementName = "Views";
        public static string NodeElementName = "View";

        /// <summary>
        /// 页面名称
        /// </summary>
        public string ViewName { set; get; }
        /// <summary>
        /// 页面数据URL
        /// </summary>
        public string ViewURL { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public ListConfigExport Export { set; get; }
        /// <summary>
        /// 页面字段
        /// </summary>
        public List<ListConfigProperty> Properties { set; get; }
        /// <summary>
        /// 页面条件项
        /// </summary>
        public List<ListConfigWhere> Wheres { set; get; }
        /// <summary>
        /// 页面排序项
        /// </summary>
        public ListConfigOrderBy OrderBys { set; get; }
        /// <summary>
        /// 行内功能栏
        /// </summary>
        public List<ListConfigToolBar> ToolBars { set; get; }
        /// <summary>
        /// 新增按钮
        /// </summary>
        public ListConfigAddButton AddButton { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public ListConfig()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ListConfig(XElement element)
        {
            ViewName = element.Attribute(nameof(ViewName)).Value;
            ViewURL = element.Attribute(nameof(ViewURL))?.Value;
            Export = element.Descendants(ListConfigExport.ElementName).Select(c => new ListConfigExport(c)).FirstOrDefault() ?? new ListConfigExport();
            Properties = element.Descendants(ListConfigProperty.ElementName).Select(c => new ListConfigProperty(c)).ToList();
            Wheres = element.Descendants(ListConfigWhere.ElementName).Select(c => new ListConfigWhere(c)).ToList();
            OrderBys = element.Descendants(ListConfigOrderBy.ElementName).Select(c => new ListConfigOrderBy(c)).FirstOrDefault() ?? new ListConfigOrderBy();
            ToolBars = element.Descendants(ListConfigToolBar.ElementName).Select(c => new ListConfigToolBar(c)).ToList();
            AddButton = element.Descendants(ListConfigAddButton.ElementName).Select(c => new ListConfigAddButton(c)).FirstOrDefault() ?? new ListConfigAddButton();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public void UpdateValues(List<Dictionary<string, object>> list)
        {
            if (list == null)
                return;
            var validProperties = Properties.Where(c => c.IsNeedOnPage);
            foreach (var property in validProperties)
            {
                switch (property.DisplayType)
                {
                    case DisplayType.None:
                        break;
                    case DisplayType.Hidden:
                        break;
                    case DisplayType.TextString:
                        break;
                    case DisplayType.TextInt:
                        break;
                    case DisplayType.TextDecimal:
                        break;
                    case DisplayType.Date:
                        foreach (var item in list)
                        {
                            var value = item[property.ColumnName];
                            var dt = value.ToDateTime();
                            item[property.ColumnName] = dt?.ToString("yyyy-MM-dd");
                        }
                        break;
                    case DisplayType.DateTime:
                        foreach (var item in list)
                        {
                            var value = item[property.ColumnName];
                            var dt = value.ToDateTime();
                            item[property.ColumnName] = dt?.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        break;
                    case DisplayType.Enum:
                        System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("VL.Research");
                        var type = assembly.ExportedTypes.FirstOrDefault(c => c.Name == property.EnumType);
                        if (type != null)
                        {
                            foreach (var item in list)
                            {
                                var value = item[property.ColumnName];
                                var description = value.ToEnumDescription(type);
                                item[property.ColumnName] = description;
                            }
                        }
                        break;
                    case DisplayType.JsonEnum:
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 更新排序项
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="field"></param>
        /// <param name="order"></param>
        internal void UpdateOrderBy(Dictionary<string, bool> orders, string field, string order)
        {
            //var orderby = this.OrderBys.FirstOrDefault(c => c.ComponentName == field);
            //if (orderby != null)
            //    orders.Add(orderby.Value, order == "asc");
        }

        /// <summary>
        /// 更新待选择显示的字段
        /// </summary>
        /// <param name="fieldNames"></param>
        internal void UpdatePropertiesToSelect(List<string> fieldNames)
        {
            var displayProperties = this.Properties.Where(c => c.IsNeedOnPage);
            fieldNames = displayProperties.Select(c => c.ColumnName).ToList();
        }

    }

    /// <summary>
    /// 比较操作
    /// </summary>
    public enum CompareOperator
    {
        None = 0,
        Equal = 1,
        GreaterThan = 2,
        LessThan = 3,
        Like = 4,
    }

    /// <summary>
    /// 组合操作
    /// </summary>
    public enum LinkOperator
    {
        None = 0,
        And = 1,
        Or = 2,
    }

    /// <summary>
    /// 
    /// </summary>
    public class ListConfigExport
    {
        public const string ElementName = "Export";

        /// <summary>
        /// 
        /// </summary>
        public ListConfigExport()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ListConfigExport(XElement element)
        {
            URL = element.Attribute(nameof(URL))?.Value;
            DefaultParams = element.Attribute(nameof(DefaultParams))?.Value?.Split(',').ToList();
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string URL { set; get; }
        /// <summary>
        /// 比较操作符
        /// </summary>
        public List<string> DefaultParams { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ListConfigWhere
    {
        public const string RootElementName = "Wheres";
        public const string ElementName = "Where";

        /// <summary>
        /// 
        /// </summary>
        public ListConfigWhere()
        {
            ComponentName = "";
            Value = "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ListConfigWhere(XElement element)
        {
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            CompareOperator = element.Attribute(nameof(CompareOperator))?.Value.ToEnum<CompareOperator>() ?? CompareOperator.None;
            Value = element.Attribute(nameof(Value))?.Value;
            #region 函数支持
            if (Value == "$GetDate()")
            {
                Value = DateTime.Now.ToString("yyyy-MM-dd");
            }
            #endregion
            LinkOperator = element.Attribute(nameof(LinkOperator))?.Value.ToEnum<LinkOperator>() ?? LinkOperator.None;
            DisplayName = element.Attribute(nameof(DisplayName))?.Value;
            DisplayType = element.Attribute(nameof(DisplayType))?.Value;
            DisplayValues = element.Attribute(nameof(DisplayValues))?.Value;
            Options = element.Attribute(nameof(Options))?.Value;
            TreeOptions = element.Attribute(nameof(TreeOptions))?.Value;
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ComponentName { set; get; }
        /// <summary>
        /// 比较操作符
        /// </summary>
        public CompareOperator CompareOperator { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { set; get; }
        /// <summary>
        /// 链接操作符
        /// </summary>
        public LinkOperator LinkOperator { set; get; }
        /// <summary>
        /// 页面 字段名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 页面 数据类型
        /// </summary>
        public string DisplayType { get; set; }
        /// <summary>
        /// 页面 下拉项配置
        /// </summary>
        public string DisplayValues { get; set; }
        /// <summary>
        /// 页面 下拉项选项
        /// </summary>
        public string Options { set; get; }
        /// <summary>
        /// 页面 下拉项选项
        /// </summary>
        public string TreeOptions { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement ToXElement()
        {
            var property = new XElement(ElementName);
            property.SetAttributeValue(nameof(ComponentName), ComponentName);
            property.SetAttributeValue(nameof(CompareOperator), CompareOperator.ToString());
            property.SetAttributeValue(nameof(Value), Value);
            property.SetAttributeValue(nameof(LinkOperator), LinkOperator.ToString());
            property.SetAttributeValue(nameof(DisplayName), DisplayName);
            property.SetAttributeValue(nameof(DisplayType), DisplayType);
            property.SetAttributeValue(nameof(DisplayValues), DisplayValues);
            property.SetAttributeValue(nameof(Options), Options);
            property.SetAttributeValue(nameof(TreeOptions), TreeOptions);
            return property;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ListConfigOrderBy
    {
        public const string ElementName = "OrderBy";

        public string DefaultName { set; get; }
        public string DefaultValue { set; get; }


        public ListConfigOrderBy()
        {
            DefaultName = "";
        }
        public ListConfigOrderBy(XElement element)
        {
            DefaultName = element.Attribute(nameof(DefaultName))?.Value;
            DefaultValue = element.Attribute(nameof(DefaultValue))?.Value;
        }

        public XElement ToXElement()
        {
            var property = new XElement(ElementName);
            property.SetAttributeValue(nameof(DefaultName), DefaultName);
            property.SetAttributeValue(nameof(DefaultValue), DefaultValue.ToString());
            return property;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ListConfigToolBar
    {
        public const string ElementName = "ToolBar";

        //"text": "<行工具栏文本>",
        public string Text { set; get; }
        //"type": "<[window-弹窗|newPage-新页面|confirm-提示]>",
        public string Type { set; get; }
        //"desc": "<提示文本>",
        public string Description { set; get; }
        //"url": "<操作URL>",
        public string URL { set; get; }
        //"params": [ "<用于url的参数>" ],
        public List<string> Params { set; get; }
        //"area": [ "<弹窗宽高> 100 or 100px" ],
        public List<string> Area { set; get; }
        //"yesFun": "<弹窗确认调用函数>",
        public string YesFun { set; get; }
        //"defaultParam": [ "<固定参数>" ]
        public List<string> DefaultParams { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public ListConfigToolBar()
        {
            Text = "";
            Type = "";
            Description = "";
            URL = "";
            Params = new List<string>();
            Area = new List<string>();
            YesFun = "";
            DefaultParams = new List<string>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ListConfigToolBar(XElement element)
        {
            Text = element.Attribute(nameof(Text))?.Value;
            Type = element.Attribute(nameof(Type))?.Value;
            Description = element.Attribute(nameof(Description))?.Value;
            URL = element.Attribute(nameof(URL))?.Value;
            Params = element.Attribute(nameof(Params))?.Value?.Split(',').ToList();
            Area = element.Attribute(nameof(Area))?.Value.Split(',').ToList();
            YesFun = element.Attribute(nameof(YesFun))?.Value;
            DefaultParams = element.Attribute(nameof(DefaultParams))?.Value?.Split(',').ToList();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ListConfigAddButton
    {
        public const string ElementName = "AddButton";

        //"text": "<按钮文本>",
        public string text { set; get; }
        //"type": "<[window-弹窗|newPage-新页面|confirm-提示]>",
        public string type { set; get; }
        //"url": "<操作URL>",
        public string url { set; get; }
        //"area": [ "<弹窗宽高> 100 or 100px" ],
        public List<string> area { set; get; }
        //"defaultParam": [ "<固定参数>" ]
        public string defaultParam { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public ListConfigAddButton()
        {
            text = "";
            type = "";
            url = "";
            area = new List<string>();
            defaultParam = "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ListConfigAddButton(XElement element)
        {
            text = element.Attribute(nameof(text))?.Value;
            type = element.Attribute(nameof(type))?.Value;
            url = element.Attribute(nameof(url))?.Value;
            area = element.Attribute(nameof(area))?.Value.Split(',').ToList();
            defaultParam = element.Attribute(nameof(defaultParam))?.Value;
        }
    }
}
