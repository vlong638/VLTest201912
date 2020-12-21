using Autobots.Infrastracture.Common.ValuesSolution;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ResearchAPI.Common
{
    public class COBusinessEntities : List<COBusinessEntity>
    {

        /// <summary>
        /// 
        /// </summary>
        public static string ElementName = "BusinessEntities";

        /// <summary>
        /// 
        /// </summary>
        public COBusinessEntities()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public COBusinessEntities(XElement element)
        {
            Id = element.Attribute(nameof(Id)).Value.ToLong().Value;
            BusinessType = element.Attribute(nameof(BusinessType))?.Value;
            var businessEntities = element.Elements(COBusinessEntity.ElementName);
            this.AddRange(businessEntities.Select(c => new COBusinessEntity(c)));
        }

        public long Id { set; get; }
        public string BusinessType { set; get; }

        public COBusinessEntity GetByName(string name)
        {
            return this.FirstOrDefault(c => c.DisplayName == name);
        }
    }
}
