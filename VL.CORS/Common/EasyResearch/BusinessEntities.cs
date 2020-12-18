using Autobots.Infrastracture.Common.ValuesSolution;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ResearchAPI.Common
{
    public class BusinessEntities : List<BusinessEntity>
    {

        /// <summary>
        /// 
        /// </summary>
        public static string ElementName = "BusinessEntities";

        /// <summary>
        /// 
        /// </summary>
        public BusinessEntities()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public BusinessEntities(XElement element)
        {
            Id = element.Attribute(nameof(Id)).Value.ToLong().Value;
            BusinessType = element.Attribute(nameof(BusinessType))?.Value;
            var businessEntities = element.Elements(BusinessEntity.ElementName);
            this.AddRange(businessEntities.Select(c => new BusinessEntity(c)));
        }

        public long Id { set; get; }
        public string BusinessType { set; get; }

        public BusinessEntity GetByName(string name)
        {
            return this.FirstOrDefault(c => c.DisplayName == name);
        }
    }
}
