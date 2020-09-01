using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Consolo_Core.Common.ExcelExportSolution
{
    public class ExportSourceMappingOrderBy
    {
        public static string ElementName = "MappingOrderBy";

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 是否正序
        /// </summary>
        public bool IsAsc { set; get; }

        public ExportSourceMappingOrderBy(XElement element)
        {
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            IsAsc = element.Attribute(nameof(IsAsc))?.Value.ToBool() ?? true;
        }
    }
}
