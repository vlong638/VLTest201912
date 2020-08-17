using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Common.ExcelSolution
{
    public static class ExcelHelper
    {
        /// <summary>
        /// 设置Cell的数据
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static void VLSetCellValue(this ISheet sheet, int row, int column, string value)
        {
            var cell = sheet.GetRow(row).GetCell(column);
            if (cell == null)
                cell = sheet.GetRow(row).CreateCell(column);
            cell.SetCellValue(value);
        }
        /// <summary>
        /// 设置Cell的数据
        /// </summary>
        public static void VLSetCellValue(this ISheet sheet, int row, int column, DateTime value)
        {
            var cell = sheet.GetRow(row).GetCell(column);
            if (cell == null || cell.CellType == CellType.Blank)
                return;
            cell.SetCellValue(value);
        }
        /// <summary>
        /// 设置Cell的数据
        /// </summary>
        public static void VLSetCellValue(this ISheet sheet, int row, int column, bool value)
        {
            var cell = sheet.GetRow(row).GetCell(column);
            if (cell == null || cell.CellType == CellType.Blank)
                return;
            cell.SetCellValue(value);
        }
        /// <summary>
        /// 设置Cell的数据
        /// </summary>
        public static void VLSetCellValue(this ISheet sheet, int row, int column, double value)
        {
            var cell = sheet.GetRow(row).GetCell(column);
            if (cell == null || cell.CellType == CellType.Blank)
                return;
            cell.SetCellValue(value);
        }

        /// <summary>
        /// 获取Cell的数据
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static string VLGetCellValueAsString(this ISheet sheet, int row, int column)
        {
            var cell = sheet.GetRow(row).GetCell(column);
            if (cell == null || cell.CellType == CellType.Blank)
                return null;
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
                    case CellType.Unknown:
                    case CellType.Blank:
                    case CellType.Boolean:
                    case CellType.Error:
                    default:
                        throw new NotImplementedException("未支持该类型的公式取值");
                }
            }
            else if (cell.CellType != CellType.String)
                throw new NotImplementedException(GetLocation(row, column) + CellTypeInvalid);
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
    }
}
