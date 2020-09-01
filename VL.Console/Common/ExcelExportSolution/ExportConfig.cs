using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.ExcelSolution;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Consolo_Core.Common.ExcelExportSolution
{
    /// <summary>
    /// 查询sql配置
    /// </summary>
    public class ExportConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public static string NodeElementName = "Export";

        /// <summary>
        /// 导出标识名
        /// </summary>
        public string ExportName { set; get; }
        /// <summary>
        /// 输出文件名
        /// </summary>
        public string FileName { set; get; }
        /// <summary>
        /// 输出数据源
        /// </summary>
        public List<ExportSheet> Sheets { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public ExportConfig()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ExportConfig(XElement element)
        {
            ExportName = element.Attribute(nameof(ExportName)).Value;
            FileName = element.Attribute(nameof(FileName)).Value;
            Sheets = element.Descendants(ExportSheet.NodeElementName).Select(c => new ExportSheet(c)).ToList();
        }
    }
}
