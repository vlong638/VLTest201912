using Autobots.Infrastracture.Common.ValuesSolution;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace ResearchAPI.CORS.Common
{
    public class IfCondition
    {
        public static string ElementName = "If";
        public string Operator { set; get; }
        public string ComponentName { set; get; }
        public string Value { set; get; }
        public string Text { set; get; }

        public IfCondition(XElement element)
        {
            Operator = element.Attribute(nameof(Operator)).Value;
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            Value = element.Attribute(nameof(Value))?.Value;
            Text = WebUtility.HtmlDecode(element.Value);
        }

        internal string GetSQL(List<SQLConfigV3Where> wheres)
        {
            switch (Operator)
            {
                case "NotEmpty":
                    var where = wheres.FirstOrDefault(c => c.ComponentName == ComponentName);
                    if (where != null && !where.Value.IsNullOrEmpty())
                    {
                        return Text;
                    }
                    break;
                case "eq":
                    where = wheres.FirstOrDefault(c => c.ComponentName == ComponentName);
                    if (where != null && where.Value == Value)
                    {
                        return Text;
                    }
                    break;
                default:
                    break;
            }
            return "";
        }
    }
}
