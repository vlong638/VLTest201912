using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Research
{
    public interface TransfromBase {
        List<string> GetRequiredFields();
        string GetMSSQLUpdateSQL(DataRow row);
        bool IsParamsValid(DataRow row);
    }

    /// <summary>
    /// 简单转换
    /// </summary>
    public class SimpleTransfrom :TransfromBase
    {
        public string From { set; get; }
        public string To { set; get; }
        ///
        public SimpleTransformType Type { set; get; }

        public string GetToValue(DataRow row)
        {
            var fromValue = row.GetRowValue(From);
            switch (Type)
            {
                case SimpleTransformType.Date:
                    var date = fromValue.ToDate();
                    return date.HasValue ? date.Value.ToString(@"yyyy-MM-dd") : null;
                case SimpleTransformType.DateTime:
                    var datetime = fromValue.ToDateTime();
                    return datetime.HasValue ? datetime.Value.ToString(@"yyyy-MM-dd HH:mm:ss") : null;
                case SimpleTransformType.Int:
                    var i = fromValue.ToInt();
                    return i.HasValue ? i.Value.ToString() : null;
                default:
                    return "";
            }
        }

        public string GetMSSQLUpdateSQL(DataRow row)
        {
            return To + "=" + GetToValue(row).ToMSSQLValue();
        }

        public bool IsParamsValid(DataRow row)
        {
            var fromValue = row[From].ToString();
            switch (Type)
            {
                case SimpleTransformType.Date:
                    if (fromValue.ToDate() == null)
                    {
                        return false;
                    }
                    break;
                case SimpleTransformType.DateTime:
                    if (fromValue.ToDateTime() == null)
                    {
                        return false;
                    }
                    break;
                case SimpleTransformType.Int:
                    if (fromValue.ToInt() == null)
                    {
                        return false;
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }

        public List<string> GetRequiredFields()
        {
            return new List<string>() { From };
        }
    }
    public enum PregnantTransformType
    {
        GestationalWeeksAndDay = 1,
    }
    /// <summary>
    /// 产科业务转换
    /// </summary>
    public class PregnantTransfrom : TransfromBase
    {
        public string LastMenstrualPeriod { set; get; }
        public string DateToCheck { set; get; }
        public string GestationalWeeks { set; get; }
        public string GestationalDays { set; get; }
        public PregnantTransformType Type { set; get; }

        public string GetMSSQLUpdateSQL(DataRow row)
        {
            switch (Type)
            {
                case PregnantTransformType.GestationalWeeksAndDay:
                    var lastMenstrualPeriod = row[LastMenstrualPeriod].ToDateTime();
                    var dateToCheck = row[DateToCheck].ToDateTime() ?? DateTime.Now;
                    PregnantCalculator.GetGestationalWeeksByLastMenstrualPeriodDate(lastMenstrualPeriod, dateToCheck, out int weeks, out int days);
                    return GestationalWeeks + "=" + weeks.ToString().ToMSSQLValue()+","+ GestationalDays + "=" + days.ToString().ToMSSQLValue();
                default:
                    throw new NotImplementedException();
            }
        }

        public bool IsParamsValid(DataRow row)
        {
            switch (Type)
            {
                case PregnantTransformType.GestationalWeeksAndDay:
                    var lastMenstrualPeriod = row[LastMenstrualPeriod].ToDateTime();
                    if (!lastMenstrualPeriod.HasValue)
                        return false;
                    break;
                default:
                    break;
            }
            return true;
        }

        public List<string> GetRequiredFields()
        {
            var result = new List<string>() { LastMenstrualPeriod };
            if (!DateToCheck.IsNullOrEmpty())
                result.Add(DateToCheck);
            return result;
        }
    }
}
