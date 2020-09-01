using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Consolo_Core.Common.ExcelExportSolution
{
    public class ExportSourceMappingWhere
    {
        public static string ElementName = "MappingWhere";

        /// <summary>
        /// 来源列名
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 来源列名
        /// </summary>
        public DataType DataType { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public OperatorType Operator { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Value { set; get; }

        public ExportSourceMappingWhere(XElement element)
        {
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            DataType = element.Attribute(nameof(DataType))?.Value.ToEnum<DataType>() ?? DataType.None;
            Operator = element.Attribute(nameof(Operator))?.Value.ToEnum<OperatorType>() ?? OperatorType.None;
            Value = element.Attribute(nameof(Value))?.Value;
        }
    }
}
