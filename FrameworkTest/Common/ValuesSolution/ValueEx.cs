﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Common.ValuesSolution
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

        #region string

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
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

        public static string GetSubStringOrEmpty(this string str, int start, int length = 0)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            if (start > str.Length)
                return "";
            if (length == 0)
            {
                return str.Substring(start);
            }
            else
            {
                length = (start + length) > str.Length ? str.Length - start : length;
                return str.Substring(start, length);
            }
        }


        #endregion

        #region int
        public static int? ToInt(this object item)
        {
            if (item == null)
                return null;
            if (item.GetType().Name == "Decimal")
            {
                return (int)(decimal)item;
            }
            int i;
            if (int.TryParse(item.ToString(), out i))
                return i;
            return null;
        }
        #endregion

        #region long
        public static long? ToLong(this object item)
        {
            if (item == null)
                return null;
            long l;
            if (long.TryParse(item.ToString(), out l))
                return l;
            return null;
        }
        #endregion

        #region decimal
        public static decimal? ToDecimal(this object item)
        {
            if (item == null)
                return null;
            decimal d;
            if (decimal.TryParse(item.ToString(), out d))
                return d;
            return null;
        }
        #endregion

        #region hex

        public static string ToHex(this byte[] byteDatas)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < byteDatas.Length; i++)
            {
                builder.Append(string.Format("{0:X2} ", byteDatas[i]));
            }
            return builder.ToString().Trim();
        }

        public static byte[] ToHexBytes(this string hex)
        {
            if (string.IsNullOrEmpty(hex) || hex.Length % 2 != 0) throw new ArgumentException("not a hexidecimal string");
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes.Add(Convert.ToByte(hex.Substring(i, 2), 16));
            }
            return bytes.ToArray();
        }

        #endregion

        #region DateTime

        public static DateTime? ToDateTime(this object item)
        {
            if (item == null)
                return null;
            DateTime dt;
            if (DateTime.TryParse(item.ToString(), out dt))
                return dt;
            return null;
        }

        public static bool IsValidDateTime(this DateTime? time)
        {
            if (time == null
                || time == DateTime.MinValue)
                return false;
            return true;
        }

        #endregion
    }
}
