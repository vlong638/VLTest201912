using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace VLTest2015
{
    public static class ValueEx
    {
        #region bool
        public static bool? ToBool(this string str)
        {
            if (str == null)
                return null;

            bool result;
            bool.TryParse(str, out result);
            return result;
        }
        #endregion

        #region int
        public static int? ToInt(this object item)
        {
            if (item == null)
                return null;
            return int.Parse(item.ToString());
        }
        #endregion

        #region MyRegion
        public static long? ToLong(this object item)
        {
            if (item == null)
                return null;
            return long.Parse(item.ToString());
        } /**/
        #endregion

        public static DateTime? ToDateTime(this object item)
        {
            if (item == null)
                return null;
            return DateTime.Parse(item.ToString());
        }

        #region string
        public static string TrimStart(this string str, string strToTrim)
        {
            while (str.StartsWith(strToTrim))
            {
                str = str.Substring(strToTrim.Length);
            }
            return str;
        }
        public static string TrimEnd(this string str, string strToTrim)
        {
            while (str.EndsWith(strToTrim))
            {
                str = str.Substring(0, str.Length - strToTrim.Length);
            }
            return str;
        }
        public static string Trim(this string str, string strToTrim)
        {
            return str.TrimStart(strToTrim).TrimEnd(strToTrim);
        } 
        #endregion
    }
}
