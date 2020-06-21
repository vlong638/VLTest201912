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
        public static bool ToBool(this string str)
        {
            return bool.Parse(str);
        } 
        #endregion

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

        public static string GetSubStringOrEmpty(this string str, int start, int length = 0)
        {
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
    }
}
