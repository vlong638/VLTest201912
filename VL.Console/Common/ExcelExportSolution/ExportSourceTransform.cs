using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Consolo_Core.Common.ExcelExportSolution
{
    public enum SourceType
    {
        None = 0,
        Field,
        JsonList,
    }
    public enum TransfromFunctionType
    {
        None = 0,
        Case,
        SumInt,
        SumCase,
    }

    public class ExportSourceTransform
    {
        public static string ElementName = "Transform";

        /// <summary>
        /// 来源列名
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string SubFieldName { set; get; }
        /// <summary>
        /// 来源类型
        /// </summary>
        public SourceType SourceType { set; get; }
        /// <summary>
        /// 转换类型
        /// </summary>
        public TransfromFunctionType FunctionType { set; get; }
        /// <summary>
        /// 目标列名
        /// </summary>
        public string TargetColumnName { set; get; }
        public List<ExportSourceTransformCase> Cases { set; get; }

        public ExportSourceTransform(XElement element)
        {
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            SubFieldName = element.Attribute(nameof(SubFieldName))?.Value;
            SourceType = element.Attribute(nameof(SourceType))?.Value.ToEnum<SourceType>() ?? SourceType.None;
            FunctionType = element.Attribute(nameof(FunctionType))?.Value.ToEnum<TransfromFunctionType>() ?? TransfromFunctionType.None;
            TargetColumnName = element.Attribute(nameof(TargetColumnName))?.Value;
            Cases = element.Descendants(ExportSourceTransformCase.ElementName).Select(c => new ExportSourceTransformCase(c)).ToList();
        }
    }
}
