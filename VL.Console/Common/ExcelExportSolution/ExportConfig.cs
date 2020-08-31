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
        //internal void PrepareSourceData(DbContext dbContext)
        //{
        //    DataSources = new Dictionary<string, DataTable>();
        //    foreach (var sourceConfig in Sources)
        //    {
        //        var dbContext = new DbContext(DBHelper.GetDbConnection("Data Source=192.168.50.102;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=HZFYUSER;Password=HZFYPWD"));
        //        var sampleRespository = new SampleRespository(dbContext);
        //        var result = dbContext.DelegateTransaction((g) =>
        //        {
        //            return sampleRespository.GetCommonSelect(sourceConfig);
        //        });
        //        DataSources[sourceConfig.SourceName] = result.Data;
        //    }
        //}
    }

    public enum CaseType { 
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
        public string SourceColumnName { set; get; }
        /// <summary>
        /// 来源列名
        /// </summary>
        public CaseType Type { set; get; }
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
            SourceColumnName = element.Attribute(nameof(SourceColumnName))?.Value;
            Type = element.Attribute(nameof(Type))?.Value.ToEnum<CaseType>() ?? CaseType.None;
            When = element.Attribute(nameof(When))?.Value;
            Then = element.Attribute(nameof(Then))?.Value;
            Operator = element.Attribute(nameof(Operator))?.Value.ToEnum<OperatorType>() ?? OperatorType.None;
            Value = element.Attribute(nameof(Value))?.Value;
        }
    }
    public enum SourceType
    {
        None = 0,
        Field,
        JsonList,
    }
    public enum FunctionType
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
        public string SourceColumnName { set; get; }
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
        public FunctionType FunctionType { set; get; }
        /// <summary>
        /// 目标列名
        /// </summary>
        public string TargetColumnName { set; get; }
        public List<ExportSourceTransformCase>  Cases { set; get; }

        public ExportSourceTransform(XElement element)
        {
            SourceColumnName = element.Attribute(nameof(SourceColumnName))?.Value;
            SubFieldName = element.Attribute(nameof(SubFieldName))?.Value;
            SourceType = element.Attribute(nameof(SourceType))?.Value.ToEnum<SourceType>() ?? SourceType.None;
            FunctionType = element.Attribute(nameof(FunctionType))?.Value.ToEnum<FunctionType>() ?? FunctionType.None;
            TargetColumnName = element.Attribute(nameof(TargetColumnName))?.Value;
            Cases = element.Descendants(ExportSourceTransformCase.ElementName).Select(c => new ExportSourceTransformCase(c)).ToList();
        }
    }

    /// <summary>
    /// 数据源
    /// </summary>
    public class ExportSource
    {
        /// <summary>
        /// 
        /// </summary>
        public const string NodeElementName = "Source";

        /// <summary>
        /// 数据源名称
        /// </summary>
        public string SourceName { set; get; }
        /// <summary>
        /// 页面字段
        /// </summary>
        public List<ExportSourceProperty> Properties { set; get; }
        /// <summary>
        /// 页面字段
        /// </summary>
        public List<ExportSourceTransform> Transforms { set; get; }
        /// <summary>
        /// 页面条件项
        /// </summary>
        public List<ExportSourceWhere> Wheres { set; get; }
        /// <summary>
        /// 页面排序项
        /// </summary>
        public List<ExportSourceOrderBy> OrderBys { set; get; }
        public string SQL { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ExportSource(XElement element)
        {
            SourceName = element.Attribute(nameof(SourceName))?.Value;
            SQL = element.Descendants("SQL").First().Value;
            Properties = element.Descendants(ExportSourceProperty.NodeElementName).Select(c => new ExportSourceProperty(c)).ToList();
            Transforms = element.Descendants(ExportSourceTransform.ElementName).Select(c => new ExportSourceTransform(c)).ToList();
            Wheres = element.Descendants(ExportSourceWhere.NodeElementName).Select(c => new ExportSourceWhere(c)).ToList();
            OrderBys = element.Descendants(ExportSourceOrderBy.NodeElementName).Select(c => new ExportSourceOrderBy(c)).ToList();
        }

        public string GetListSQL()
        {
            var sql = SQL;
            var propertiesIsOn = Properties.Select(c => c.Alias);
            var fields = propertiesIsOn.Count() == 0 ? "*" : string.Join(",", propertiesIsOn);
            sql = sql.Replace("@Fields", fields);
            var wheresIsOn = Wheres.Where(c => c.IsOn).Select(c => c.SQL);
            var wheres = wheresIsOn.Count() == 0 ? "" : $"where {string.Join(" and ", wheresIsOn)}";
            sql = sql.Replace("@Wheres", wheres);
            var orderByIsOn = OrderBys.FirstOrDefault(c => c.IsOn) ?? OrderBys.First();
            var orderBy = orderByIsOn.Alias;
            var order = orderByIsOn.IsAsc ? "asc" : "desc";
            sql = sql.Replace("@OrderBy", $"order by {orderBy} {order}");
            return sql;
        }

        public Dictionary<string, object> GetParams()
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            foreach (var where in Wheres)
            {
                if (where.IsOn)
                {
                    args.Add(where.ComponentName, where.Formatter.IsNullOrEmpty() ? where.Value : where.Formatter.Replace("@" + where.ComponentName, where.Value.ToString()));
                }
            }
            return args;
        }

        public void DoTransforms(ref DataTable datatable)
        {
            foreach (var transform in Transforms)
            {
                switch (transform.SourceType)
                {
                    case SourceType.Field:
                        #region Field
                        switch (transform.FunctionType)
                        {
                            case FunctionType.Case:
                                #region Case
                                datatable.Columns.Add(new DataColumn(transform.TargetColumnName));
                                for (int i = 0; i < datatable.Rows.Count; i++)
                                {
                                    var value = GetValue(datatable.Rows[i], transform.Cases);
                                    datatable.Rows[i][transform.TargetColumnName] = value;
                                }
                                #endregion
                                break;
                            default:
                                throw new NotImplementedException("SourceType.Field未支持该`FunctionType` ");
                        } 
                        #endregion
                        break;
                    case SourceType.JsonList:
                        #region JsonList
                        switch (transform.FunctionType)
                        {
                            case FunctionType.None:
                                break;
                            case FunctionType.Case:
                                break;
                            case FunctionType.SumInt:
                                #region SumInt
                                datatable.Columns.Add(new DataColumn(transform.TargetColumnName));
                                for (int i = 0; i < datatable.Rows.Count; i++)
                                {
                                    var jsonTable = datatable.Rows[i][transform.SourceColumnName].ToString()?.FromJson<DataTable>();
                                    if (jsonTable == null)
                                        continue;
                                    var sumValue = jsonTable.AsEnumerable().Sum(c =>
                                    {
                                        var text = c.Field<string>(transform.SubFieldName);
                                        var value = text?.ToInt();
                                        return value.HasValue ? value.Value : 0;
                                    });
                                    datatable.Rows[i][transform.TargetColumnName] = sumValue;
                                }
                                #endregion
                                break;
                            case FunctionType.SumCase:
                                break;
                            default:
                                throw new NotImplementedException("未支持该`FunctionType` ");
                        } 
                        #endregion
                        break;
                    default:
                        throw new NotImplementedException("SumInt下未支持该`SourceType` ");
                }




            }
        }

        private string GetValue(DataRow row, List<ExportSourceTransformCase> cases)
        {
            foreach (var @case in cases)
            {
                string text = "";
                if (@case.Type != CaseType.Default)
                    text = row[@case.SourceColumnName].ToString();
                switch (@case.Type)
                {
                    case CaseType.Default:
                        return @case.Then;
                    case CaseType.String:
                        if (text == @case.Value)
                            return @case.Then;
                        break;
                    case CaseType.Int:
                        var sourceValueInt = text.ToInt();
                        if (!sourceValueInt.HasValue)
                            return "";
                        var compareValueInt = @case.Value.ToInt();
                        if (!compareValueInt.HasValue)
                            return "";
                        switch (@case.Operator)
                        {
                            case OperatorType.eq:
                                if (sourceValueInt== compareValueInt)
                                    return @case.Then;
                                break;
                            case OperatorType.neq:
                                if (sourceValueInt != compareValueInt)
                                    return @case.Then;
                                break;
                            case OperatorType.gt:
                                if (sourceValueInt > compareValueInt)
                                    return @case.Then;
                                break;
                            case OperatorType.lt:
                                if (sourceValueInt < compareValueInt)
                                    return @case.Then;
                                break;
                            default:
                                throw new NotImplementedException("未支持该`Operator` locator:471");
                        }
                        break;
                    case CaseType.DateTime:
                        var sourceValueTime = text.ToDateTime();
                        if (!sourceValueTime.HasValue)
                            return "";
                        var compareValueTime = @case.Value.ToDateTime();
                        if (!compareValueTime.HasValue)
                            return "";
                        switch (@case.Operator)
                        {
                            case OperatorType.eq:
                                if (sourceValueTime.Value == compareValueTime.Value)
                                    return @case.Then;
                                break;
                            case OperatorType.neq:
                                if (sourceValueTime.Value != compareValueTime.Value)
                                    return @case.Then;
                                break;
                            case OperatorType.gt:
                                if (sourceValueTime.Value > compareValueTime.Value)
                                    return @case.Then;
                                break;
                            case OperatorType.lt:
                                if (sourceValueTime.Value < compareValueTime.Value)
                                    return @case.Then;
                                break;
                            default:
                                throw new NotImplementedException("未支持该`Operator` locator:500");
                        }
                        break;
                    default:
                        throw new NotImplementedException("未支持该`CaseType` locator:466");
                }
            }
            return "";
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ExportSourceProperty
    {
        /// <summary>
        /// 
        /// </summary>
        public const string NodeElementName = "Property";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ExportSourceProperty(XElement element)
        {
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            Alias = element.Attribute(nameof(Alias))?.Value;
        }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 查询用名称
        /// </summary>
        public string Alias { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement ToXElement()
        {
            var property = new XElement(NodeElementName);
            property.SetAttributeValue(nameof(ColumnName), ColumnName);
            property.SetAttributeValue(nameof(Alias), Alias);
            return property;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ExportSourceWhere
    {
        /// <summary>
        /// 
        /// </summary>
        public const string NodeElementName = "Where";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ExportSourceWhere(XElement element)
        {
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            Formatter = element.Attribute(nameof(Formatter))?.Value;
            IsOn = element.Attribute(nameof(IsOn))?.Value.ToBool() ?? false;
            SQL = element.Value;
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ComponentName { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string SQL { set; get; }
        /// <summary>
        /// 格式化
        /// </summary>
        public string Formatter { get; set; }

        #region compute
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsOn { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public object Value { set; get; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement ToXElement()
        {
            var property = new XElement(NodeElementName);
            property.SetAttributeValue(nameof(ComponentName), ComponentName);
            property.SetAttributeValue(nameof(Formatter), Formatter);
            property.SetValue(SQL);
            return property;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ExportSourceOrderBy
    {
        /// <summary>
        /// 
        /// </summary>
        public const string NodeElementName = "OrderBy";

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsOn { set; get; }
        /// <summary>
        /// 是否正序
        /// </summary>
        public bool IsAsc { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string ComponentName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Alias { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ExportSourceOrderBy(XElement element)
        {
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            Alias = element.Attribute(nameof(Alias))?.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement ToXElement()
        {
            var property = new XElement(NodeElementName);
            property.SetAttributeValue(nameof(ComponentName), ComponentName);
            property.SetAttributeValue(nameof(Alias), Alias);
            return property;
        }
    }

}
