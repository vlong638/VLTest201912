//using System.Collections.Generic;

//namespace Research.Common
//{
//    /// <summary>
//    /// 关联连接集合
//    /// </summary>
//    public class Routers : List<Router>
//    {

//    }
//    /// <summary>
//    /// 关联连接
//    /// </summary>
//    public class Router
//    {
//        public Router(string from, string to, RouteType routeType, List<RouterOn> ons)
//        {
//            From = from;
//            To = to;
//            RouteType = routeType;
//            Ons = ons;
//        }

//        public string From { set; get; }
//        public string To { set; get; }
//        public RouteType RouteType { set; get; }
//        public List<RouterOn> Ons { set; get; }
//    }
//    /// <summary>
//    /// 连接内容
//    /// </summary>
//    public class RouterOn
//    {
//        public RouterOn(string from, string fromField, string to, string toField)
//        {
//            From = from;
//            FromField = fromField;
//            To = to;
//            ToField = toField;
//        }

//        public string From { set; get; }
//        public string FromField { set; get; }
//        public string To { set; get; }
//        public string ToField { set; get; }
//    }
//    /// <summary>
//    /// 连接类型
//    /// </summary>
//    public enum RouteType
//    {
//        None = 0,
//        LeftJoin = 1,
//        RightJoin = 2,
//    }
//}
