using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ValuesSolution;

namespace Research.Common
{
    public class Routers : List<Router>
    {
        #region 预设配置

        /// <summary>
        /// 
        /// </summary>
        public static string ElementName = "Routers";

        /// <summary>
        /// 
        /// </summary>
        public Routers()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public Routers(XElement element)
        {
            var routers = element.Elements(Router.ElementName);
            this.AddRange(routers.Select(c => new Router(c)));
        }

        #endregion
    }

    public class Router
    {
        public const string ElementName = "Router";

        public Router()
        {
        }
        public Router(XElement element)
        {
            From = element.Attribute(nameof(From))?.Value;
            FromAlias = element.Attribute(nameof(FromAlias))?.Value;
            To = element.Attribute(nameof(To))?.Value;
            ToAlias = element.Attribute(nameof(ToAlias))?.Value;
            RouteType = element.Attribute(nameof(RouteType))?.Value.ToEnum<RouteType>() ?? RouteType.None;
            Ons = element.Descendants(RouterOn.ElementName).Select(c => new RouterOn(c)).ToList();
        }

        public string From { set; get; }
        public string FromAlias { set; get; }
        public string To { set; get; }
        public string ToAlias { set; get; }
        public RouteType RouteType { set; get; }
        public List<RouterOn> Ons { set; get; } = new List<RouterOn>();
        public long CustomBusinessEntityId { get; set; }

        internal Router Clone()
        {
            return new Router()
            {
                From = this.From,
                FromAlias = this.FromAlias,
                To = this.To,
                ToAlias = this.ToAlias,
                RouteType = this.RouteType,
                Ons = this.Ons,
                CustomBusinessEntityId = this.CustomBusinessEntityId,
            };
        }
    }

    public class RouterOn
    {
        public const string ElementName = "On";

        public RouterOn()
        {
        }
        public RouterOn(XElement element)
        {
            FromField = element.Attribute(nameof(FromField))?.Value;
            ToField = element.Attribute(nameof(ToField))?.Value;
        }

        public RouterOn(string fromField, string toField)
        {
            FromField = fromField;
            ToField = toField;
        }

        public string FromField { set; get; }
        public string ToField { set; get; }

        internal string ToSQL(Router c)
        {
            return $@"{c.FromAlias}.{FromField} = [{c.ToAlias}].{ToField}";
        }
    }
}
