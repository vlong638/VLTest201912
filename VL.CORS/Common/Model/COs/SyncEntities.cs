using Autobots.Infrastracture.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ResearchAPI.CORS.Common
{
    public class COSyncEntities : List<COSyncEntity>
    {

        /// <summary>
        /// 
        /// </summary>
        public static string ElementName = "SyncEntities";

        /// <summary>
        /// 
        /// </summary>
        public COSyncEntities()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public COSyncEntities(XElement element)
        {
            Id = element.Attribute(nameof(Id)).Value.ToLong().Value;
            BusinessType = element.Attribute(nameof(BusinessType))?.Value;
            var businessEntities = element.Elements(COSyncEntity.ElementName);
            this.AddRange(businessEntities.Select(c => new COSyncEntity(c)));
        }

        public long Id { set; get; }
        public string BusinessType { set; get; }

        public COSyncEntity GetByName(string name)
        {
            return this.FirstOrDefault(c => c.DisplayName == name);
        }
    }

    public class COSyncEntity
    {
        public const string ElementName = "SyncEntity";

        public COSyncEntity(XElement element)
        {
            Id = element.Attribute(nameof(Id)).Value.ToLong().Value;
            DisplayName = element.Attribute(nameof(DisplayName))?.Value;
            SourceName = element.Attribute(nameof(SourceName))?.Value;
            TargetName = element.Attribute(nameof(TargetName))?.Value;
            Properties.AddRange(element.Descendants(COSyncEntityProperty.ElementName).Select(c => new COSyncEntityProperty(this, c)));
        }

        public long Id { set; get; }
        public string DisplayName { set; get; }
        public string SourceName { set; get; }
        public string TargetName { set; get; }
        public List<COSyncEntityProperty> Properties { set; get; } = new List<COSyncEntityProperty>();
    }


    public class COSyncEntityProperty
    {
        public const string ElementName = "Property";

        public COSyncEntityProperty()
        {
        }

        public COSyncEntityProperty(COSyncEntityProperty c)
        {
            Id = c.Id;
            DisplayName = c.DisplayName;
            SourceName = c.SourceName;
            ColumnType = c.ColumnType;
            MaxLength = c.MaxLength;
            Precision = c.Precision;
            Scale = c.Scale;
            Enum = c.Enum;
            IsEnumText = c.IsEnumText;
            ControlType = c.ControlType;
        }

        public COSyncEntityProperty(COSyncEntity COSyncEntity, XElement element)
        {
            Id = element.Attribute(nameof(Id)).Value.ToLong().Value;
            DisplayName = element.Attribute(nameof(DisplayName))?.Value;
            SourceName = element.Attribute(nameof(SourceName))?.Value;
            ColumnType = element.Attribute(nameof(ColumnType))?.Value;
            MaxLength = element.Attribute(nameof(MaxLength))?.Value.ToInt() ?? 0;
            Precision = element.Attribute(nameof(Precision))?.Value;
            Scale = element.Attribute(nameof(Scale))?.Value;
            Enum = element.Attribute(nameof(Enum))?.Value;
            IsEnumText = element.Attribute(nameof(IsEnumText))?.Value == "是";
            ControlType = element.Attribute(nameof(ControlType))?.Value;
        }

        public string GetTargetColumnDefinition()
        {
            //单选框
            //多选框
            if (ColumnType=="多选框")
            {
                return "nvarchar(255)";
            }
            //否
            //是
            if (IsEnumText)
            {
                return "nvarchar(255)";
            }
            if (!Enum.IsNullOrEmpty())
            {
                MaxLength = MaxLength > 20 ? MaxLength : 20;
            }
            switch (ColumnType)
            {
                case "datetime2":
                case "datetime":
                case "date":
                    return "datetime2(0)";//N=0到7，表示精确到秒钟后的几位数。DateTime(0)表示精确到秒；DateTime2(3)相当于原始的DateTime类型，但是能精确到1毫秒，占用7字节；DateTime2(7)则能达到最高的精度，100纳秒。
                case "decimal":
                    return $"decimal({Precision},{Scale})";
                case "int":
                    return "int";
                case "bigint":
                    return "bigint";
                case "bit":
                case "char":
                case "nchar":
                    return "nvarchar(20)";
                case "varchar":
                case "nvarchar":
                    var maxLength = MaxLength.ToInt().Value;
                    return $"nvarchar({(maxLength < 0 ? "max" : (maxLength > 4000 ? 4000 : maxLength).ToString())})";
                default:
                    throw new NotImplementedException(ColumnType + ",类型处理未实现");
            }
        }

        public long Id { set; get; }
        public string DisplayName { set; get; }
        public string SourceName { set; get; }
        public string ColumnType { set; get; }
        public int MaxLength { set; get; }
        public string Precision { set; get; }
        public string Scale { set; get; }
        public string Enum { set; get; }
        public bool IsEnumText { set; get; }
        public string ControlType { set; get; }
    }
}
