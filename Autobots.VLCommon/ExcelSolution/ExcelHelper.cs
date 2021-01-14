using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Autobots.Infrastracture.Common.ExcelSolution
{
    public static class ExcelHelper
    {
        #region 值处理
        /// <summary>
        /// 设置Cell的数据
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNum"></param>
        /// <param name="columnNum"></param>
        /// <returns></returns>
        public static void VLSetCellValue(this ISheet sheet, int rowNum, int columnNum, string value)
        {
            ICell cell = GetCell(sheet, rowNum, columnNum);
            cell.SetCellValue(value);
        }

        private static ICell GetCell(ISheet sheet, int rowNum, int columnNum)
        {
            var row = sheet.GetRow(rowNum);
            if (row == null)
                row = sheet.CreateRow(rowNum);
            var cell = row.GetCell(columnNum);
            if (cell == null)
                cell = row.CreateCell(columnNum);
            return cell;
        }

        /// <summary>
        /// 设置Cell的数据
        /// </summary>
        public static void VLSetCellValue(this ISheet sheet, int rowNum, int columnNum, DateTime value)
        {
            ICell cell = GetCell(sheet, rowNum, columnNum);
            cell.SetCellValue(value);
        }
        /// <summary>
        /// 设置Cell的数据
        /// </summary>
        public static void VLSetCellValue(this ISheet sheet, int rowNum, int columnNum, bool value)
        {
            ICell cell = GetCell(sheet, rowNum, columnNum);
            cell.SetCellValue(value);
        }
        /// <summary>
        /// 设置Cell的数据
        /// </summary>
        public static void VLSetCellValue(this ISheet sheet, int rowNum, int columnNum, double value)
        {
            ICell cell = GetCell(sheet, rowNum, columnNum);
            cell.SetCellValue(value);
        }

        /// <summary>
        /// 获取Cell的数据
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNum"></param>
        /// <param name="columnNum"></param>
        /// <returns></returns>
        public static string VLGetCellValueAsString(this ISheet sheet, int rowNum, int columnNum)
        {
            ICell cell = GetCell(sheet, rowNum, columnNum);
            if (cell.CellType == CellType.Numeric)
            {
                if (DateUtil.IsCellDateFormatted(cell))
                {
                    return Convert.ToDateTime(cell.DateCellValue).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    return cell.NumericCellValue.ToString();
                }
            }
            else if (cell.CellType == CellType.Formula)
            {
                switch (cell.CachedFormulaResultType)
                {
                    case CellType.Numeric:
                        return cell.NumericCellValue.ToString();
                    case CellType.String:
                        return cell.StringCellValue.ToString();
                    case CellType.Formula:
                    case CellType.Error:
                        return "";
                    case CellType.Unknown:
                    case CellType.Blank:
                    case CellType.Boolean:
                    default:
                        throw new NotImplementedException("未支持该类型的公式取值");
                }
            }
            else if (cell.CellType == CellType.Blank)
                return cell.StringCellValue;
            else if (cell.CellType != CellType.String)
                throw new NotImplementedException(GetLocation(rowNum, columnNum) + CellTypeInvalid);
            return cell.StringCellValue;
        }

        static string CellTypeInvalid = "单元格格式未符合预期";
        static List<string> Alphabet = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        static string GetLocation(int row, int column)
        {
            row += 1;
            if (column >= 256)
                return row + "," + column;
            var upperStr = column > 16 ? Alphabet[column / 16] : "";
            var lowerStr = Alphabet[column % 16];
            return row + "," + upperStr + lowerStr;
        }
        #endregion

        #region Excel
        public static void ExportDataTableToExcel(this DataTable dt, string path)
        {
            var workbook = new XSSFWorkbook();
            string sheetName = "Sheet1";
            ISheet sheet = workbook.CreateSheet(sheetName);
            IRow dataRow = sheet.CreateRow(0);
            foreach (DataColumn column in dt.Columns)
            {
                dataRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }
            //填充内容  
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataRow = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    dataRow.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                }
            }
            workbook.Write(new FileStream(path, FileMode.Create, FileAccess.Write));
            workbook.Close();
        }
        #endregion
    }
}
