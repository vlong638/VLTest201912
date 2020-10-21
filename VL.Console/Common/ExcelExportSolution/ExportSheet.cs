using NPOI.HSSF.UserModel;
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
        public List<SQLConfigSource> Sources { set; get; }

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
            Sources = element.Descendants(SQLConfigSource.ElementName).Select(c => new SQLConfigSource(c)).ToList();
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
                        if (holder.LoopAuto)
                        {
                            if (j == 0)
                            {
                                MyInsertRow(sheet, i + 1, talbe.Rows.Count - 1, row);
                                rowsCount += talbe.Rows.Count - 1;
                            }
                            for (int n = 0; n < talbe.Rows.Count; n++)
                            {
                                var value = talbe.Rows[n][holder.Field]?.ToString();
                                sheet.VLSetCellValue(i + n, j, value);
                            }
                        }
                        else if(holder.Loop > 1)
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
        private void MyInsertRow(ISheet sheet, int startAt, int addCount, IRow sourceRow)
        {
            #region 批量移动行
            if (addCount>0)
            {
                sheet.ShiftRows(
                startAt,
                sheet.LastRowNum,                            //--结束行
                addCount,                             //--移动大小(行数)--往下移动
                true,                                   //是否复制行高
                false                                  //是否重置行高
                );
            }
            #endregion

            #region 对批量移动后空出的空行插，创建相应的行，并以插入行的上一行为格式源(即：插入行-1的那一行)
            for (int i = startAt; i < startAt + addCount - 1; i++)
            {
                IRow targetRow = null;
                ICell sourceCell = null;
                ICell targetCell = null;

                targetRow = sheet.CreateRow(i + 1);

                for (int m = sourceRow.FirstCellNum; m < sourceRow.LastCellNum; m++)
                {
                    sourceCell = sourceRow.GetCell(m);
                    if (sourceCell == null)
                        continue;
                    targetCell = targetRow.CreateCell(m);
                    targetCell.CellStyle = sourceCell.CellStyle;
                    targetCell.SetCellType(sourceCell.CellType);
                }
            }

            IRow firstTargetRow = sheet.CreateRow(startAt);
            ICell firstSourceCell = null;
            ICell firstTargetCell = null;

            for (int m = sourceRow.FirstCellNum; m < sourceRow.LastCellNum; m++)
            {
                firstSourceCell = sourceRow.GetCell(m);
                if (firstSourceCell == null)
                    continue;
                firstTargetCell = firstTargetRow.CreateCell(m);
                firstTargetCell.CellStyle = firstSourceCell.CellStyle;
                firstTargetCell.SetCellType(firstSourceCell.CellType);
            }
            #endregion
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
