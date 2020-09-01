using System.Xml.Linq;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Consolo_Core.Common.ExcelExportSolution
{
    public enum DataType
    {
        None = 0,
        Default = 1,
        String = 2,
        Int = 3,
        DateTime = 4,
    }

    public enum OperatorType
    {
        None = 0,
        eq,// =        
        neq,
        gt,// >
        lt,// <
    }

    public class ExportSourceTransformCase
    {
        public static string ElementName = "Case";

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
        public string When { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Then { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public OperatorType Operator { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Value { set; get; }

        public ExportSourceTransformCase(XElement element)
        {
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            DataType = element.Attribute(nameof(DataType))?.Value.ToEnum<DataType>() ?? DataType.None;
            When = element.Attribute(nameof(When))?.Value;
            Then = element.Attribute(nameof(Then))?.Value;
            Operator = element.Attribute(nameof(Operator))?.Value.ToEnum<OperatorType>() ?? OperatorType.None;
            Value = element.Attribute(nameof(Value))?.Value;
        }
    }
}
