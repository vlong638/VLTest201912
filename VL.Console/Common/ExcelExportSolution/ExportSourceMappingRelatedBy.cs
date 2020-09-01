using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Consolo_Core.Common.ExcelExportSolution
{
    public class ExportSourceMappingRelatedBy
    {
        public static string ElementName = "MappingRelatedBy";

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 列名
        /// </summary>
        public string RelatedColumnName { set; get; }
        /// <summary>
        /// 来源列名
        /// </summary>
        public DataType DataType { set; get; }

        public ExportSourceMappingRelatedBy(XElement element)
        {
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            RelatedColumnName = element.Attribute(nameof(RelatedColumnName))?.Value;
            DataType = element.Attribute(nameof(DataType))?.Value.ToEnum<DataType>() ?? DataType.None;
        }
    }
}
