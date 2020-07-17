using FrameworkTest.Common.PagerSolution;
using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FrameworkTest.ConfigurableEntity
{
    public class EntityAppConfig
    {
        public static string RootElementName = "Views";
        public static string NodeElementName = "View";

        public string ViewName { set; get; }
        public List<EntityAppConfigProperty> Properties { set; get; } 
        public List<EntityAppConfigWhere> Wheres { set; get; }
        public EntityAppConfigOrderBy OrderBy { set; get; }

        public EntityAppConfig()
        {

        }
        public EntityAppConfig(XElement element)
        {
            ViewName = element.Attribute(nameof(ViewName)).Value;
            Properties = element.Descendants(EntityAppConfigProperty.ElementName).Select(c => new EntityAppConfigProperty(c)).ToList();
            Wheres = element.Descendants(EntityAppConfigWhere.ElementName).Select(c => new EntityAppConfigWhere(c)).ToList();
            OrderBy = element.Descendants(EntityAppConfigOrderBy.ElementName).Select(c => new EntityAppConfigOrderBy(c)).FirstOrDefault() ?? new EntityAppConfigOrderBy();
        }

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
                        System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("FrameworkTest");
                        var type = assembly.ExportedTypes.FirstOrDefault(c => c.Name == property.EnumType);
                        if (type!=null)
                        {
                            foreach (var item in list)
                            {
                                var value = item[property.ColumnName];
                                var description = value.ToEnumDescription(type);
                                item[property.ColumnName] = description;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public XElement ToXElement()
        {
            //view
            var node = new XElement(NodeElementName);
            node.SetAttributeValue(nameof(ViewName), ViewName);
            //properties
            if (Properties != null && Properties.Count > 0)
            {
                var properties = Properties.Select(p => p.ToXElement());
                node.Add(properties);
            }
            //wheres
            if (Wheres != null && Wheres.Count > 0)
            {
                var wheresRoot = new XElement(EntityAppConfigWhere.RootElementName);
                var wheres = Wheres.Select(p => p.ToXElement());
                wheresRoot.Add(wheres);
                node.Add(wheresRoot);
            }
            //orderby
            if (OrderBy != null)
            {
                node.Add(OrderBy.ToXElement());
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

    public class EntityAppConfigWhere
    {
        public const string RootElementName = "Wheres";
        public const string ElementName = "Where";

        public EntityAppConfigWhere()
        {
            ComponentName = "";
            Value = "";
        }
        public EntityAppConfigWhere(XElement element)
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
        }

        public string ComponentName { set; get; }
        public CompareOperator CompareOperator { set; get; }
        public string Value { set; get; }
        public LinkOperator LinkOperator { set; get; }

        public XElement ToXElement()
        {
            var property = new XElement(ElementName);
            property.SetAttributeValue(nameof(ComponentName), ComponentName);
            property.SetAttributeValue(nameof(CompareOperator), CompareOperator.ToString());
            property.SetAttributeValue(nameof(Value), Value);
            property.SetAttributeValue(nameof(LinkOperator), LinkOperator.ToString());
            return property;
        }
    }

    public class EntityAppConfigOrderBy
    {
        public const string ElementName = "OrderBy";

        public string ComponentName { set; get; }
        public bool IsAsc { set; get; }

        public EntityAppConfigOrderBy()
        {
            ComponentName = "";
        }
        public EntityAppConfigOrderBy(XElement element)
        {
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            IsAsc = element.Attribute(nameof(IsAsc))?.Value.ToBool() ?? false;
        }

        public XElement ToXElement()
        {
            var property = new XElement(ElementName);
            property.SetAttributeValue(nameof(ComponentName), ComponentName);
            property.SetAttributeValue(nameof(IsAsc), IsAsc.ToString());
            return property;
        }
    }
}
