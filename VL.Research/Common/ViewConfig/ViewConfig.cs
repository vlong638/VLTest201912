using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Research.Common
{
    /// <summary>
    /// 页面配置
    /// </summary>
    public class ViewConfig
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
        /// 页面字段
        /// </summary>
        public List<ViewConfigProperty> Properties { set; get; } 
        /// <summary>
        /// 页面条件项
        /// </summary>
        public List<ViewConfigWhere> Wheres { set; get; }
        /// <summary>
        /// 页面排序项
        /// </summary>
        public ViewConfigOrderBys OrderBys { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public ViewConfig()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ViewConfig(XElement element)
        {
            ViewName = element.Attribute(nameof(ViewName)).Value;
            ViewURL = element.Attribute(nameof(ViewURL))?.Value;
            Properties = element.Descendants(ViewConfigProperty.ElementName).Select(c => new ViewConfigProperty(c)).ToList();
            Wheres = element.Descendants(ViewConfigWhere.ElementName).Select(c => new ViewConfigWhere(c)).ToList();
            OrderBys = element.Descendants(ViewConfigOrderBys.ElementName).Select(c => new ViewConfigOrderBys(c)).FirstOrDefault() ?? new ViewConfigOrderBys();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public void UpdateValues(IEnumerable<Dictionary<string, object>> list)
        {
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
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement ToXElement()
        {
            //view
            var node = new XElement(NodeElementName);
            node.SetAttributeValue(nameof(ViewName), ViewName);
            node.SetAttributeValue(nameof(ViewURL), ViewURL);
            //properties
            if (Properties != null && Properties.Count > 0)
            {
                var properties = Properties.Select(p => p.ToXElement());
                node.Add(properties);
            }
            //wheres
            if (Wheres != null && Wheres.Count > 0)
            {
                var wheresRoot = new XElement(ViewConfigWhere.RootElementName);
                var wheres = Wheres.Select(p => p.ToXElement());
                wheresRoot.Add(wheres);
                node.Add(wheresRoot);
            }
            //orderby
            if (OrderBys != null)
            {
                node.Add(OrderBys.ToXElement());
            }
            return node;
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
    public class ViewConfigWhere
    {
        public const string RootElementName = "Wheres";
        public const string ElementName = "Where";

        /// <summary>
        /// 
        /// </summary>
        public ViewConfigWhere()
        {
            ComponentName = "";
            Value = "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ViewConfigWhere(XElement element)
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
            return property;
        }
    }

    public class ViewConfigOrderBys
    {
        public const string ElementName = "OrderBys";

        public string DefaultName { set; get; }
        public string DefaultValue { set; get; }
        public List<ViewConfigOrderBy> OrderByList { set; get; }


        public ViewConfigOrderBys()
        {
            DefaultName = "";
        }
        public ViewConfigOrderBys(XElement element)
        {
            DefaultName = element.Attribute(nameof(DefaultName))?.Value;
            DefaultValue = element.Attribute(nameof(DefaultValue))?.Value;
            OrderByList = element.Descendants(ViewConfigOrderBy.ElementName).Select(c => new ViewConfigOrderBy(c))?.ToList() ?? new List<ViewConfigOrderBy>();
        }

        public XElement ToXElement()
        {
            var property = new XElement(ElementName);
            property.SetAttributeValue(nameof(DefaultName), DefaultName);
            property.SetAttributeValue(nameof(DefaultValue), DefaultValue.ToString());
            return property;
        }
    }


    public class ViewConfigOrderBy
    {
        public const string ElementName = "OrderBy";

        public string ComponentName { set; get; }
        public string Value { set; get; }

        public ViewConfigOrderBy(XElement element)
        {
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            Value = element.Attribute(nameof(Value))?.Value;
        }

        public XElement ToXElement()
        {
            var property = new XElement(ElementName);
            property.SetAttributeValue(nameof(ComponentName), ComponentName);
            property.SetAttributeValue(nameof(Value), Value);
            return property;
        }
    }
}
