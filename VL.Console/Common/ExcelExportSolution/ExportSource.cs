using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Consolo_Core.Common.ExcelExportSolution
{
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
        /// 
        /// </summary>
        public List<ExportSourceTransform> Transforms { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public List<ExportSourceMapping> Mappings { set; get; }
        /// <summary>
        /// 页面条件项
        /// </summary>
        public List<ExportSourceWhere> Wheres { set; get; }
        /// <summary>
        /// 页面排序项
        /// </summary>
        public List<ExportSourceOrderBy> OrderBys { set; get; }
        /// <summary>
        /// 
        /// </summary>
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
            Mappings = element.Descendants(ExportSourceMapping.ElementName).Select(c => new ExportSourceMapping(c)).ToList();
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
                            case TransfromFunctionType.Case:
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
                            case TransfromFunctionType.None:
                                break;
                            case TransfromFunctionType.Case:
                                break;
                            case TransfromFunctionType.SumInt:
                                #region SumInt
                                datatable.Columns.Add(new DataColumn(transform.TargetColumnName));
                                for (int i = 0; i < datatable.Rows.Count; i++)
                                {
                                    var jsonTable = datatable.Rows[i][transform.ColumnName].ToString()?.FromJson<DataTable>();
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
                            case TransfromFunctionType.SumCase:
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

        public void DoMappings(ref DataTable datatable, Dictionary<string, DataTable> dataSources)
        {
            foreach (var mapping in Mappings)
            {
                datatable.Columns.Add(new DataColumn(mapping.TargetColumnName));
                var sourceData = dataSources[mapping.SourceName];
                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    var row = datatable.Rows[i];
                    EnumerableRowCollection<DataRow> temp = null;
                    //关联
                    switch (mapping.RelatedBy.DataType)
                    {
                        case DataType.String:
                            var relatedString = row[mapping.RelatedBy.RelatedColumnName].ToString();
                            temp= dataSources[mapping.SourceName].AsEnumerable().Where(c => c.Field<string>(mapping.RelatedBy.ColumnName) == relatedString);
                            break;
                        case DataType.Int:
                            var relatedInt = row[mapping.RelatedBy.RelatedColumnName].ToInt();
                            temp = dataSources[mapping.SourceName].AsEnumerable().Where(c => c.Field<int>(mapping.RelatedBy.ColumnName) == relatedInt);
                            break;
                        case DataType.DateTime:
                            var relatedDateTime = row[mapping.RelatedBy.RelatedColumnName].ToDateTime();
                            temp = dataSources[mapping.SourceName].AsEnumerable().Where(c => c.Field<DateTime>(mapping.RelatedBy.ColumnName) == relatedDateTime);
                            break;
                        default:
                            break;
                    }
                    //筛选
                    foreach (var where in mapping.Wheres)
                    {
                        switch (where.DataType)
                        {
                            case DataType.String:
                                var compareString = where.Value;
                                switch (where.Operator)
                                {
                                    case OperatorType.eq:
                                        temp = temp.Where(c => c.Field<string>(mapping.FieldName) == compareString);
                                        break;
                                    case OperatorType.neq:
                                        temp = temp.Where(c => c.Field<string>(mapping.FieldName) != compareString);
                                        break;
                                    default:
                                        throw new NotImplementedException("未支持该`OperatorType` 201");
                                }
                                break;
                            case DataType.Int:
                                var compareInt = where.Value.ToInt();
                                switch (where.Operator)
                                {
                                    case OperatorType.eq:
                                        temp = temp.Where(c => c.Field<int>(mapping.FieldName) == compareInt);
                                        break;
                                    case OperatorType.neq:
                                        temp = temp.Where(c => c.Field<int>(mapping.FieldName) != compareInt);
                                        break;
                                    case OperatorType.gt:
                                        temp = temp.Where(c => c.Field<int>(mapping.FieldName) > compareInt);
                                        break;
                                    case OperatorType.lt:
                                        temp = temp.Where(c => c.Field<int>(mapping.FieldName) < compareInt);
                                        break;
                                    default:
                                        throw new NotImplementedException("未支持该`OperatorType` 220");
                                }
                                break;
                            case DataType.DateTime:
                                var compareDateTime = where.Value.ToDateTime();
                                switch (where.Operator)
                                {
                                    case OperatorType.eq:
                                        temp = temp.Where(c => c.Field<DateTime>(mapping.FieldName) == compareDateTime);
                                        break;
                                    case OperatorType.neq:
                                        temp = temp.Where(c => c.Field<DateTime>(mapping.FieldName) != compareDateTime);
                                        break;
                                    case OperatorType.gt:
                                        temp = temp.Where(c => c.Field<DateTime>(mapping.FieldName) > compareDateTime);
                                        break;
                                    case OperatorType.lt:
                                        temp = temp.Where(c => c.Field<DateTime>(mapping.FieldName) < compareDateTime);
                                        break;
                                    default:
                                        throw new NotImplementedException("未支持该`OperatorType` 241");
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    //排序
                    if (mapping.OrderBy.IsAsc)
                    {
                        temp = temp.OrderBy(c => c[mapping.OrderBy.ColumnName]);
                    }
                    else
                    {
                        temp = temp.OrderByDescending(c => c[mapping.OrderBy.ColumnName]);
                    }
                    //求值
                    switch (mapping.FunctionType)
                    {
                        case MappingFunctionType.None:
                            break;
                        case MappingFunctionType.First:
                            row[mapping.TargetColumnName] = temp.First().Field<string>(mapping.FieldName);
                            break;
                        case MappingFunctionType.SumInt:
                            row[mapping.TargetColumnName] = temp.Sum(c=>c.Field<int>(mapping.FieldName));
                            break;
                        default:
                            throw new NotImplementedException("未支持该`FunctionType` 269");
                    }
                }
            }
        }

        private string GetValue(DataRow row, List<ExportSourceTransformCase> cases)
        {
            foreach (var @case in cases)
            {
                string text = "";
                if (@case.DataType != DataType.Default)
                    text = row[@case.ColumnName].ToString();
                switch (@case.DataType)
                {
                    case DataType.Default:
                        return @case.Then;
                    case DataType.String:
                        if (text == @case.Value)
                            return @case.Then;
                        break;
                    case DataType.Int:
                        var sourceValueInt = text.ToInt();
                        if (!sourceValueInt.HasValue)
                            return "";
                        var compareValueInt = @case.Value.ToInt();
                        if (!compareValueInt.HasValue)
                            return "";
                        switch (@case.Operator)
                        {
                            case OperatorType.eq:
                                if (sourceValueInt == compareValueInt)
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
                    case DataType.DateTime:
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
}
