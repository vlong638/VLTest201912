using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ExcelSolution;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Consolo_Core.Common.ExcelExportSolution
{
    /// <summary>
    /// 查询sql配置
    /// </summary>
    public class ExportSheet
    {
        /// <summary>
        /// 
        /// </summary>
        public static string NodeElementName = "Sheet";

        /// <summary>
        /// Sheet名称
        /// </summary>
        public string SheetName { set; get; }
        /// <summary>
        /// 输出数据源
        /// </summary>
        public List<ExportSource> Sources { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public ExportSheet()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ExportSheet(XElement element)
        {
            SheetName = element.Attribute(nameof(SheetName)).Value;
            Sources = element.Descendants(ExportSource.NodeElementName).Select(c => new ExportSource(c)).ToList();
        }

        public void Render(ISheet sheet)
        {
            List<string> contentsToDeal = new List<string>();
            var rowsCount = sheet.LastRowNum;
            for (int i = 0; i <= rowsCount; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null)
                    row = sheet.CreateRow(i);
                var columnsCount = row.LastCellNum;
                for (int j = 0; j < columnsCount; j++)
                {
                    var text = sheet.VLGetCellValueAsString(i, j) ?? "";
                    if (text.StartsWith("@"))
                    {
                        contentsToDeal.Add(text);
                        PlaceHolder holder = new PlaceHolder(text);
                        if (!DataSources.ContainsKey(holder.Source))
                            continue;
                        var talbe = (DataSources[holder.Source]);
                        if (talbe == null || !talbe.Columns.Contains(holder.Field))
                            continue;
                        if (holder.Loop > 1)
                        {
                            for (int n = 0; n < holder.Loop; n++)
                            {
                                if (n + 1 > talbe.Rows.Count)
                                    break;

                                var value = talbe.Rows[n][holder.Field]?.ToString();
                                sheet.VLSetCellValue(i + n, j, value);
                            }
                        }
                        else if (holder.Func.IsNullOrEmpty())
                        {
                            var value = talbe.Rows[0][holder.Field]?.ToString();
                            sheet.VLSetCellValue(i, j, value);
                        }
                        else
                        {
                            switch (holder.Func.ToLower())
                            {
                                case "@sumint":
                                    int sumValue = 0;
                                    for (int n = 0; n < talbe.Rows.Count; n++)
                                    {
                                        sumValue += talbe.Rows[n][holder.Field]?.ToString().ToInt() ?? 0;
                                    }
                                    sheet.VLSetCellValue(i, j, sumValue.ToString());
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public void UpdateWheres(List<VLKeyValue> wheres)
        {
            if (wheres == null)
                return;

            foreach (var sourceConfig in Sources)
            {
                foreach (var where in wheres)
                {
                    var whereConfig = sourceConfig.Wheres.FirstOrDefault(c => c.ComponentName.ToLower() == where.Key.ToLower());
                    if (whereConfig == null)
                        continue;
                    if (!where.Value.IsNullOrEmpty())
                    {
                        whereConfig.IsOn = true;
                        whereConfig.Value = where.Value;
                    }
                }
            }
        }

        public Dictionary<string, DataTable> DataSources { set; get; }
    }
}
