using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FrameworkTest.ConfigurableEntity
{
    class EntityAppConfigTable
    {
        public static string ElementName = "TableName";

        public string TableName { set; get; }
        public List<EntityAppConfigProperty> Properties { set; get; }

        public EntityAppConfigTable()
        { 

        }
        public EntityAppConfigTable(XElement element)
        {
            TableName = element.Attribute(ElementName).Value;
            Properties = element.Descendants(EntityAppConfigProperty.ElementName).Select(c => new EntityAppConfigProperty(c)).ToList();
        }

        public XElement ToXElement()
        {
            var table = new XElement("Table");
            table.SetAttributeValue(nameof(TableName), TableName);
            var properties = Properties.Select(p => p.ToXElement());
            table.Add(properties);
            return table;
        }
    }
    class EntityAppConfigProperty
    {
        public static string ElementName = "Property";

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 列名(显示)
        /// </summary>
        public string DisplayName { set; get; }
        /// <summary>
        /// 说明(显示,编辑时易理解)
        /// </summary>
        public string Description { set; get; }

        #region 页面控制开关
        /// <summary>
        /// 是否在列表页显示
        /// </summary>
        public bool IsDisplayOnList { set; get; }
        /// <summary>
        /// 是否在详情页显示
        /// </summary>
        public bool IsDisplayOnDetail { set; get; }
        #endregion

        #region 数据格式及校验
        /// <summary>
        /// 数据类型(显示,校验)
        /// </summary>
        public string DataType { set; get; }
        ///// <summary>
        ///// 最大值(校验)
        ///// </summary>
        //public long? MaxValue { set; get; }
        ///// <summary>
        ///// 最小值(校验)
        ///// </summary>
        //public long? MinValue { set; get; }
        ///// <summary>
        ///// 是否允许空(校验)
        ///// </summary>
        //public bool IsRequired { set; get; } 
        #endregion

        public EntityAppConfigProperty(EntityDBConfig dbConfig)
        {
            ColumnName = dbConfig.ColumnName;
            DisplayName = "";
            DataType = dbConfig.DataType;
            IsDisplayOnList = false;
            IsDisplayOnDetail = false;
            Description = dbConfig.Description;
        }
        public EntityAppConfigProperty(XElement element)
        {
            ColumnName= element.Attribute(nameof(ColumnName)).Value;
            DisplayName = element.Attribute(nameof(DisplayName)).Value;
            DataType = element.Attribute(nameof(DataType)).Value;
            IsDisplayOnList = element.Attribute(nameof(IsDisplayOnList)).Value.ToBool();
            IsDisplayOnDetail = element.Attribute(nameof(IsDisplayOnDetail)).Value.ToBool();
            Description = element.Attribute(nameof(Description)).Value;
        }

        public XElement ToXElement()
        {
            var property = new XElement(ElementName);
            property.SetAttributeValue(nameof(ColumnName), ColumnName);
            property.SetAttributeValue(nameof(DisplayName), DisplayName);
            property.SetAttributeValue(nameof(IsDisplayOnList), IsDisplayOnList);
            property.SetAttributeValue(nameof(IsDisplayOnDetail), IsDisplayOnDetail);
            property.SetAttributeValue(nameof(DataType), DataType);
            property.SetAttributeValue(nameof(Description), Description);
            return property;
        }
    }
}
