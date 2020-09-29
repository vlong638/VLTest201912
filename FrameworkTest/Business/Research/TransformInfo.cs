using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Research
{
    public class TransformInfo
    {
        public string TableName { set; get; }
        public List<string> KeyFields { set; get; }
        public List<TransfromBase> Transforms { set; get; }
        public string SourceSQL { get; set; }

        public string GetSourceSQL()
        {
            if (!SourceSQL.IsNullOrEmpty())
                return SourceSQL;

            var allFields = new List<string>();
            allFields.AddRange(KeyFields);
            foreach (var Transform in Transforms)
                allFields.AddRange(Transform.GetRequiredFields());
            return $"select {allFields.Distinct().Join(",")} from {TableName}";
        }

        public string GetSQLToUpdate(DataRow row)
        {
            return $"update {TableName} set {GetSets(row)} where {GetWheres(row)};";
        }

        private string GetWheres(DataRow row)
        {
            return KeyFields.Select(c => c + "=" + row.GetRowValue(c).ToMSSQLValue()).Join(" and ");
        }

        private string GetSets(DataRow row)
        {
            return Transforms.Select(c => c.GetMSSQLUpdateSQL(row)).Join(",");
        }

        internal bool IsParamsValid(DataRow row)
        {
            foreach (var item in Transforms)
            {
                if (!item.IsParamsValid(row))
                    return false;
            }
            return true;
        }

        //public Dictionary<string, object> GetParams(DataRow row)
        //{
        //    Dictionary<string, object> args = new Dictionary<string, object>();
        //    foreach (var key in KeyFields)
        //    {
        //        args.Add(key, row[key].ToString());
        //    }
        //    foreach (var FromTo in FromTos)//IssueTime IssueTime
        //    {
        //        var fromValue = row[FromTo.From].ToString();
        //        switch (FromTo.Type)
        //        {
        //            case SimpleTransformType.Date:
        //                args.Add(FromTo.To, fromValue.ToDate());
        //                break;
        //            case SimpleTransformType.DateTime:
        //                args.Add(FromTo.To, fromValue.ToDateTime());
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    return args;
        //}


        //internal bool IsColumnsValid(DataTable data)
        //{
        //    foreach (var key in KeyFields)
        //    {
        //        if (!data.Columns.Contains(key))
        //        {
        //            return false;
        //        }
        //    }
        //    foreach (var FromTo in FromTos)//IssueTime IssueTime
        //    {
        //        if (!data.Columns.Contains(FromTo.From))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
    }
}
