using Autobots.Infrastracture.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ResearchAPI.CORS.Common
{
    public class COStatisticsEntities : List<COStatisticsEntity>
    {

        /// <summary>
        /// 
        /// </summary>
        public static string ElementName = "StatisticsEntities";

        /// <summary>
        /// 
        /// </summary>
        public COStatisticsEntities()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public COStatisticsEntities(XElement element)
        {
            //Id = element.Attribute(nameof(Id)).Value.ToLong().Value;
            var businessEntities = element.Elements(COStatisticsEntity.ElementName);
            this.AddRange(businessEntities.Select(c => new COStatisticsEntity(c)));
        }

        //public long Id { set; get; }
    }

    public class COStatisticsEntity
    {
        public const string ElementName = "StatisticsEntity";

        public COStatisticsEntity(XElement element)
        {
            Id = element.Attribute(nameof(Id)).Value.ToLong().Value;
            Name = element.Attribute(nameof(Name)).Value;
            Parameters = element.Attribute(nameof(Parameters))?.Value;
            var sql = element.Descendants("SQL")?.FirstOrDefault().ToString().TrimStart("<SQL>").TrimEnd("</SQL>");
            RawSQL = sql;
            SQLEntity = new RootSQL(RawSQL);
            ParentFormatter = element.Attribute(nameof(ParentFormatter))?.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        public long Id { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 默认参数
        /// </summary>
        public string Parameters { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string RawSQL { set; get; }
        /// <summary>
        /// 预设SQL
        /// </summary>
        public RootSQL SQLEntity { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string ParentFormatter { set; get; }
    }
}
