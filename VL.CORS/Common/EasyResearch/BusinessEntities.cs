using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ResearchAPI.Common
{
    public class BusinessEntities : List<BusinessEntity>
    {
        #region 预设配置

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
            var businessEntities = element.Elements(BusinessEntity.ElementName);
            this.AddRange(businessEntities.Select(c => new BusinessEntity(c)));
        }

        #endregion

        public BusinessEntity GetByName(string name)
        {
            return this.FirstOrDefault(c => c.DisplayName == name);
        }
    }
}
