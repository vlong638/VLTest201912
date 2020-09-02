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
    public class ViewConfig
    {
        public static string RootElementName = "Views";
        public static string NodeElementName = "View";

        public string ViewName { set; get; }
        public List<ViewConfigProperty> Properties { set; get; } 
        public List<ViewConfigWhere> Wheres { set; get; }
        public ViewConfigOrderBy OrderBy { set; get; }

        public ViewConfig()
        {

        }
        public ViewConfig(XElement element)
        {
            ViewName = element.Attribute(nameof(ViewName)).Value;
            Properties = element.Descendants(ViewConfigProperty.ElementName).Select(c => new ViewConfigProperty(c)).ToList();
            Wheres = element.Descendants(ViewConfigWhere.ElementName).Select(c => new ViewConfigWhere(c)).ToList();
            OrderBy = element.Descendants(ViewConfigOrderBy.ElementName).Select(c => new ViewConfigOrderBy(c)).FirstOrDefault() ?? new ViewConfigOrderBy();
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
                var wheresRoot = new XElement(ViewConfigWhere.RootElementName);
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

    public class ViewConfigWhere
    {
        public const string RootElementName = "Wheres";
        public const string ElementName = "Where";

        public ViewConfigWhere()
        {
            ComponentName = "";
            Value = "";
        }
        public ViewConfigWhere(XElement element)
        {
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            Value = element.Attribute(nameof(Value))?.Value;
            #region 函数支持
            if (Value == "$GetDate()")
            {
                Value = DateTime.Now.ToString("yyyy-MM-dd");
            } 
            #endregion
        }

        public string ComponentName { set; get; }
        public string Value { set; get; }

        public XElement ToXElement()
        {
            var property = new XElement(ElementName);
            property.SetAttributeValue(nameof(ComponentName), ComponentName);
            property.SetAttributeValue(nameof(Value), Value);
            return property;
        }
    }

    public class ViewConfigOrderBy
    {
        public const string ElementName = "OrderBy";

        public string ComponentName { set; get; }
        public bool IsAsc { set; get; }

        public ViewConfigOrderBy()
        {
            ComponentName = "";
        }
        public ViewConfigOrderBy(XElement element)
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
