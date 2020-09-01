using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Consolo_Core.Common.ExcelExportSolution
{
    public enum MappingFunctionType
    {
        None = 0,
        First,
        SumInt,
    }
    public class ExportSourceMapping
    {
        public static string ElementName = "Mapping";

        /// <summary>
        /// 来源列名
        /// </summary>
        public string SourceName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string FieldName { set; get; }
        /// <summary>
        /// 目标列名
        /// </summary>
        public string TargetColumnName { set; get; }
        /// <summary>
        /// 转换类型
        /// </summary>
        public MappingFunctionType FunctionType { set; get; }
        /// <summary>
        /// 关联项目
        /// </summary>
        public ExportSourceMappingRelatedBy RelatedBy { set; get; }
        /// <summary>
        /// 条件转换
        /// </summary>
        public List<ExportSourceMappingWhere> Wheres { set; get; }
        /// <summary>
        /// 排序项目
        /// </summary>
        public ExportSourceMappingOrderBy OrderBy { set; get; }

        public ExportSourceMapping(XElement element)
        {
            SourceName = element.Attribute(nameof(SourceName))?.Value;
            FieldName = element.Attribute(nameof(FieldName))?.Value;
            TargetColumnName = element.Attribute(nameof(TargetColumnName))?.Value;
            FunctionType = element.Attribute(nameof(FunctionType))?.Value.ToEnum<MappingFunctionType>() ?? MappingFunctionType.None;
            RelatedBy = element.Descendants(ExportSourceMappingRelatedBy.ElementName).Select(c => new ExportSourceMappingRelatedBy(c)).FirstOrDefault();
            Wheres = element.Descendants(ExportSourceMappingWhere.ElementName).Select(c => new ExportSourceMappingWhere(c)).ToList();
            OrderBy = element.Descendants(ExportSourceMappingOrderBy.ElementName).Select(c => new ExportSourceMappingOrderBy(c)).FirstOrDefault();
        }
    }
}
