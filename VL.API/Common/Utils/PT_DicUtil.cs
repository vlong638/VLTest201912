using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VL.API.Common.Utils
{
    public class DicUtil
    {
        public static bool HasItem(Dictionary<string, object> items)
        {
            return items != null && items.Count > 0;
        }
        public static string GetKey(string key, Dictionary<string, object> dataitems, bool ignorecase = true)
        {
            if (ignorecase)
            {
                string existkey = dataitems.Keys.FirstOrDefault(x => (x + "").Trim().ToLower() == (key + "").Trim().ToLower());
                if (string.IsNullOrWhiteSpace(existkey))
                {
                    return key;
                }
                key = existkey;
            }
            return key;
        }
        public static bool ContainsKey(Dictionary<string, object> dataitems, string key)
        {
            if (dataitems == null || dataitems.Count == 0 || string.IsNullOrWhiteSpace(key))
                return false;
            //string existkey = dataitems.Keys.FirstOrDefault(x => (x + "").Trim().ToLower() == (key + "").Trim().ToLower());
            //if (string.IsNullOrWhiteSpace(existkey))
            //{
            //    return false;
            //}
            //return true;
            key = GetKey(key, dataitems, ignorecase: true);
            return dataitems.ContainsKey(key);
        }
        public static string ContainsKey(ref Dictionary<string, object> dataitems, string key, object value, bool ignorecase = true)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;
            if (dataitems == null)
                dataitems = new Dictionary<string, object>();
            key = GetKey(key, dataitems, ignorecase: ignorecase);
            if (dataitems.ContainsKey(key))
                dataitems[key] = value;
            else
                dataitems.Add(key, value);
            return null;
        }

        public static string RemoveKey(ref Dictionary<string, object> dataitems, string key, bool ignorecase = true)
        {
            if (dataitems == null || dataitems.Count == 0 || string.IsNullOrWhiteSpace(key))
                return null;
            key = GetKey(key, dataitems, ignorecase: ignorecase);
            if (dataitems.ContainsKey(key))
                dataitems.Remove(key);
            return null;
        }
        public static T GetDicValue<T>(string key, Dictionary<string, object> dataitems, bool ignorecase = true)
        {
            if (dataitems == null || dataitems.Count == 0 || string.IsNullOrWhiteSpace(key))
                return default(T);

            key = GetKey(key, dataitems, ignorecase: ignorecase);

            if (!dataitems.ContainsKey(key) || dataitems[key] == null || dataitems[key] == DBNull.Value)
                return default(T);
            try
            {
                T value = default(T);

                if (typeof(T) == typeof(string))
                {
                    value = (T)Convert.ChangeType((dataitems[key] + "").Trim(), typeof(T));
                }
                else
                {
                    value = (T)Convert.ChangeType(dataitems[key], typeof(T));
                }

                return value;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items1">主表</param>
        /// <param name="items2">附表</param>
        /// <param name="update">当主表和附表含有相同key，且主表为空，附表不为空时，是否覆盖</param>
        /// <param name="updatenum">针对数值当主表和附表含有相同key，且主表为0，附表不为0时，是否覆盖</param>
        /// <param name="ischange">主表内容是否被修改</param>
        /// <returns></returns>
        public static Dictionary<string, object> Union(Dictionary<string, object> items1, Dictionary<string, object> items2, ref bool ischange, bool update = false, bool updatenum = false)
        {
            if (!HasItem(items1))
            {
                ischange = true;
                return items2;
            }
            if (!HasItem(items2))
            {
                ischange = true;
                return items1;
            }
            Dictionary<string, object> newitems = new Dictionary<string, object>(items1);

            foreach (KeyValuePair<string, object> kv in items2)
            {
                object old_val = GetDicValue<object>(kv.Key, newitems);
                object new_val = GetDicValue<object>(kv.Key, items2);
                if (!ContainsKey(newitems, kv.Key))
                {
                    ContainsKey(ref newitems, kv.Key, kv.Value);
                    ischange = true;
                }
                else if (update)
                {
                    if (string.IsNullOrWhiteSpace(old_val + "") && !string.IsNullOrWhiteSpace(new_val + ""))
                    {
                        ContainsKey(ref newitems, kv.Key, kv.Value);
                        ischange = true;
                    }
                    if (updatenum)
                    {
                        if (ContentUtil.IsDigital(old_val) && ContentUtil.IsDigital(new_val))
                        {
                            decimal d_old = GetDicValue<decimal>(kv.Key, newitems);
                            decimal d_new = GetDicValue<decimal>(kv.Key, items2);

                            if (d_old <= 0 && d_new > 0)
                            {
                                ContainsKey(ref newitems, kv.Key, kv.Value);
                                ischange = true;
                            }
                        }
                    }
                }

            }
            return newitems;
        }
        /// <param name="items1">主表</param>
        /// <param name="items2">附表</param>
        /// <param name="update">当主表和附表含有相同key，且主表为空，附表不为空时，是否覆盖</param>
        /// <param name="updatenum">针对数值当主表和附表含有相同key，且主表为0，附表不为0时，是否覆盖</param>
        /// <param name="ischange">主表内容是否被修改</param>
        /// <returns></returns>
        public static Dictionary<string, object> Union(Dictionary<string, object> items1, Dictionary<string, object> items2, bool update = false, bool updatenum = false)
        {
            bool ischange = false;
            return Union(items1, items2, ref ischange, update: update, updatenum: updatenum);
        }
        public static void CopyItem(string newkey, string oldkey, ref Dictionary<string, object> items)
        {
            if (!HasItem(items))
                return;
            DicUtil.ContainsKey(ref items, newkey, DicUtil.GetDicValue<string>(oldkey, items));
        }
        public static bool IsEquals(Dictionary<string, object> items1, Dictionary<string, object> items2)
        {
            if (!HasItem(items1) && !HasItem(items2)) return true;
            if (!HasItem(items1) && HasItem(items2)) return false;
            if (!HasItem(items2) && HasItem(items1)) return false;
            if (items1.Count != items2.Count) return false;

            bool b = items1.ToList().Exists(x => { bool _b = StringUtil.Trim(DicUtil.GetDicValue<string>(x.Key, items1)) != StringUtil.Trim(DicUtil.GetDicValue<string>(x.Key, items2)); return _b; }) ||
                items2.ToList().Exists(x => { bool _b = StringUtil.Trim(DicUtil.GetDicValue<string>(x.Key, items1)) != StringUtil.Trim(DicUtil.GetDicValue<string>(x.Key, items2)); return _b; });
            return !b;
        }
    }

    /// <summary>
    ///ContentUtil 的摘要说明
    /// </summary>
    public class ContentUtil
    {
        public ContentUtil()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        public static string getContent(object con, string empty)
        {
            if (con == null || con == DBNull.Value)
            {
                return empty;
            }
            else
            {
                return con.ToString().Trim();
            }
        }
        public static string padContent(int len, string con, char c)
        {
            if (string.IsNullOrWhiteSpace(con))
                return string.Empty;
            if (StringUtil.getStringLength(con.Trim()) <= len)
                return con.PadLeft(len, c);
            else
                return getContent(len, con);
        }
        public static string padContentRight(int len, string con, char c)
        {
            if (string.IsNullOrWhiteSpace(con))
                return string.Empty;
            if (StringUtil.getStringLength(con.Trim()) <= len)
                return con.PadRight(len, c);
            else
                return getContent(len, con);
        }
        /// <summary>
        /// 认为中文占两个字节
        /// 英文占一个字节
        /// </summary>
        /// <param name="len"></param>
        /// <param name="con"></param>
        /// <returns></returns>
        public static string getContent(int len, string con)
        {
            if (string.IsNullOrWhiteSpace(con))
                return string.Empty;
            if (StringUtil.getStringLength(con.Trim()) < len)
                return con;
            else
                //return con.Substring(0, len);
                return StringUtil.GetFirstString(con, len);

        }

        public static string getContent(int len, string con, string torlent)
        {
            if (len <= 0)
                return con;
            if (string.IsNullOrWhiteSpace(con))
                return string.Empty;
            if (StringUtil.getStringLength(con.Trim()) < len)
                return con;
            else
                //return con.Trim().Substring(0, len - torlent.Length) + torlent;
                return StringUtil.GetFirstString(con, len - torlent.Length) + torlent;
        }
        public static string getContent(int len, string con, string torlent, string tag, int rowlen)
        {
            string ret = string.Empty;
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(con))
                return ret;

            int index = tag.IndexOf(">");
            string start = tag.Substring(0, index + 1);
            string end = tag.Substring(index + 1, tag.Length - index - 1);

            ret = getContent(len, con, torlent);

            for (int i = 0; i < Math.Ceiling((double)ret.Length / (double)rowlen); i++)
            {
                sb.Append(start);
                if (Math.Ceiling((double)ret.Length / (double)rowlen) - i <= 1)
                {
                    sb.Append(ret.Substring(i * rowlen));
                }
                else
                {
                    sb.Append(ret.Substring(i * rowlen, rowlen));
                }
                sb.Append(end);
            }

            return sb.ToString();
        }
        public static string NullTextContent(string con)
        {
            return NullTextContent(con, null);
        }
        public static string NullTextContent(string con, string nulltext)
        {
            if (string.IsNullOrWhiteSpace(con))
            {
                //if (string.IsNullOrWhiteSpace(nulltext))
                //    return "暂无";
                //else
                return nulltext;
            }
            else
                return con;
        }
        public static string NullTextContent(string con, string nulltext, bool ignorezero, string unit)
        {
            if (string.IsNullOrWhiteSpace(con) || (ignorezero && (con == "-1" || con == int.MinValue + "" || con == "0" || con == "0.0" || con == "0.00" || con == "0.000")))
            {
                //if (string.IsNullOrWhiteSpace(nulltext))
                //    return "暂无";
                //else
                return nulltext;
            }
            else
                return con + unit;
        }
        public static string IgnoreMinDate(string con)
        {
            return IgnoreDefaultContent(con, new string[] { DateTime.MinValue.ToString(), DateTime.MinValue.ToShortDateString(), TimeUtil.GetDate(DateTime.MinValue), TimeUtil.GetDate(DateTime.MinValue, "yyyy-MM-dd HH:mm:ss"), TimeUtil.GetDate(DateTime.MinValue, "yyyy/MM/dd HH:mm:ss"), TimeUtil.GetDate(DateTime.MinValue, "yyyy/M/d HH:mm:ss") });
        }
        public static string IgnoreZero(string con, string emptystr = "")
        {
            return IgnoreDefaultContent(con, new string[] { int.MinValue + "", "-1", "0", "0.0", "0.00", "0.000" });
        }
        public static string IgnoreMinValue(string con, string emptystr = "")
        {
            return IgnoreDefaultContent(con, new string[] { int.MinValue + "" });
        }
        public static string IgnoreDefaultContent(string con, string[] df, string emptystr = "")
        {
            if (df == null || df.Length == 0)
                return emptystr;
            for (int i = 0; i < df.Length; i++)
            {
                if (con == df[i])
                    return emptystr;
            }
            return con;
        }
        public static string BindMultiContent(string con, string bingcon, bool ispre)
        {
            if (string.IsNullOrWhiteSpace(con))
                return null;
            if (ispre)
                return bingcon + con;
            else
                return con + bingcon;
        }
        public static string getHtmlContent(int len, string con, string torlent)
        {
            return StringUtil.HTMLSubstring(con, len, torlent);
        }
        public static int GetCeilInt(int i)
        {
            return i < 0 ? 0 : i;
        }

        public static T GetCeil<T>(object i)
        {

            try
            {
                T value = default(T);

                if (typeof(T) == typeof(string))
                {
                    value = (T)Convert.ChangeType((i + "").Trim(), typeof(T));
                }
                else
                {
                    value = (T)Convert.ChangeType(i, typeof(T));
                }
                decimal d = 0m;
                decimal.TryParse(i + "", out d);

                return d <= 0 ? default(T) : value;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        #region 数字转化成中文
        #region 转化函数
        /// <summary>
        /// 转化函数
        /// </summary>
        /// <param name="IntStr">需要转化的字符串</param>
        /// <returns>转化完后的字符串</returns>
        public static string IntToCHN(string IntStr)
        {
            StringBuilder retval = new StringBuilder();//用来存放中文形式的数字
            string[] SubNumStr = SubSectionNumStr(IntStr);//用来存放以亿为单位的字符串数组
            for (int i = 0; i < SubNumStr.Length; i++)
            {
                string OneStr = ManageStrZero(SubNumStr[i].ToString());//需要判断有多个“0”的情况
                char[] chars = OneStr.ToCharArray(); //存储字符
                int tempcount = chars.Length;//用来记录位数
                foreach (char c in chars)
                {
                    if (i != SubNumStr.Length - 1 && SubNumStr.Length > 1)
                        retval.Append(NumParseToText(c, tempcount - 1, true));//当整体数值大于亿时需要进位
                    else
                        retval.Append(NumParseToText(c, tempcount - 1, false));
                    if (tempcount > 0)
                        tempcount--;//向后移位
                }
            }
            return NumStrNodeManage(retval.ToString());
        }
        #endregion

        #region 将字符串以亿为单位拆分为字符数组
        /// <summary>
        /// 将字符串以亿为单位拆分为字符数组
        /// </summary>
        /// <param name="NumStr">字符串</param>
        /// <returns>得到用“|”号隔开的字符串数组</returns>
        private static string[] SubSectionNumStr(string NumStr)
        {
            StringBuilder temp = new StringBuilder();
            int Howm = NumStr.Length / 9;
            if (NumStr.Length > 9)
            {
                for (int i = 0; i < Howm; i++)
                {
                    temp.Insert(0, "|" + NumStr.Substring(NumStr.Length - 9, 9));
                    NumStr = NumStr.Remove(NumStr.Length - 9, 9);
                }
                if (NumStr != "")
                    temp.Insert(0, NumStr);
                return temp.ToString().Split('|');
            }
            else
                return NumStr.Split();
        }
        #endregion

        #region 判断有多个“0”的情况
        /// <summary>
        /// 判断有多个“0”的情况
        /// </summary>
        /// <param name="ZeroStr">处理字符</param>
        /// <returns>多个零处理成“*”号</returns>
        private static string ManageStrZero(string ZeroStr)
        {
            return ZeroStr.Replace("00000000", "*******0").Replace("0000000", "******0").Replace("000000", "*****0").Replace("00000", "****0").Replace("0000", "***0").Replace("000", "**0").Replace("00", "*0");
        }
        #endregion

        #region 将字符转化为大写的中文形式
        /// <summary>
        /// 将数字字符转化为大写的中文形式
        /// </summary>
        /// <param name="str">需要转化的字符</param>
        /// <param name="count">位数</param>
        /// <param name="moreBill">是否超过9位</param>
        /// <returns>返回中文形式的数字字符</returns>
        private static string NumParseToText(char str, int count, bool moreBill)
        {
            string retval = "";
            switch (str)
            {
                case '0': retval = "零"; break;
                case '1': retval = "一"; break;
                case '2': retval = "二"; break;
                case '3': retval = "三"; break;
                case '4': retval = "四"; break;
                case '5': retval = "五"; break;
                case '6': retval = "六"; break;
                case '7': retval = "七"; break;
                case '8': retval = "八"; break;
                case '9': retval = "九"; break;
                default: break;
            }
            if (moreBill)
            {
                count++;//当整体数值大于亿时需要进位
            }
            if ((str != '0' && str != '*') || count == 4 || count == 8)//当字符为0时不需要加位数但如果过万仍需要：十万 百万 千万 和过亿时需要：十亿 百亿 千亿
            {
                switch (count.ToString())
                {
                    case "1":
                    case "5":
                    case "9":
                        retval += "十"; break;
                    case "2":
                    case "6":
                    case "10":
                        retval += "百"; break;
                    case "3":
                    case "7":
                    case "11":
                        retval += "千"; break;
                    case "4": retval += "万"; break;
                    case "8": retval += "亿"; break;
                    default: break;
                }
            }
            return retval;
        }
        #endregion

        #region 尾数、亿和万节点处理
        private static string NumStrNodeManage(string Numstr)
        {
            string TempNumStr = Numstr;
            if (Numstr.EndsWith("零"))//如果以零结尾则需要去除
                TempNumStr = Numstr.Remove(Numstr.Length - 1);
            TempNumStr = TempNumStr.Replace("零零", "亿零").Replace("零万", "万零");//数字过亿时会出现亿为转折点因为将多个0处理成一个0时是以亿为一组
            return TempNumStr;
        }
        #endregion

        #region 判断是否是数字
        /// <summary>
        /// 判断是否是数字
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNumber(String strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumber) &&
            !objTwoDotPattern.IsMatch(strNumber) &&
            !objTwoMinusPattern.IsMatch(strNumber) &&
            objNumberPattern.IsMatch(strNumber);
        }
        #endregion
        #endregion

        /// <summary>
        /// 正则表达式取值
        /// </summary>
        /// <param name="HtmlCode">源码</param>
        /// <param name="RegexString">正则表达式</param>
        /// <param name="GroupKey">正则表达式分组关键字</param>
        /// <param name="RightToLeft">是否从右到左</param>
        /// <returns></returns>
        public static string[] GetRegValue(string HtmlCode, string RegexString, string GroupKey, bool RightToLeft, bool replaceenter = true)
        {
            if (string.IsNullOrWhiteSpace(HtmlCode))
                return null;
            MatchCollection m;
            Regex r;
            if (RightToLeft == true)
            {
                r = new Regex(RegexString, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.RightToLeft);
            }
            else
            {
                r = new Regex(RegexString, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }
            if (replaceenter)
            {
                HtmlCode = Regex.Replace(HtmlCode, @"((\r\n)+|\s+)", "");
                HtmlCode = Regex.Replace(HtmlCode, @"(\r+|\n+)", "");
            }
            m = r.Matches(HtmlCode);
            string[] MatchValue = new string[m.Count];
            for (int i = 0; i < m.Count; i++)
            {
                MatchValue[i] = m[i].Groups[GroupKey].Value;
            }
            return MatchValue;
        }

        public static string GetRegValue(string HtmlCode, string RegexString, bool RightToLeft, bool replaceenter = true)
        {
            if (string.IsNullOrWhiteSpace(HtmlCode)) return HtmlCode;
            Match m;
            Regex r;
            if (RightToLeft == true)
            {
                r = new Regex(RegexString, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.RightToLeft);
            }
            else
            {
                r = new Regex(RegexString, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }
            if (replaceenter)
            {
                HtmlCode = Regex.Replace(HtmlCode, @"((\r\n)+|\s+)", "");
                HtmlCode = Regex.Replace(HtmlCode, @"(\r+|\n+)", "");
            }
            m = r.Match(HtmlCode);
            return m.Value;
        }
        public static string RemoveRN(string HtmlCode, string replaceStr = "")
        {
            HtmlCode = Regex.Replace(HtmlCode, @"((\r\n)+|\s+)", replaceStr);
            HtmlCode = Regex.Replace(HtmlCode, @"(\r+|\n+)", replaceStr);

            return HtmlCode;
        }
        public static string RemoveRN_1(string HtmlCode, string replaceStr = "")
        {
            HtmlCode = Regex.Replace(HtmlCode, @"(\r+|\n+)", replaceStr);

            return HtmlCode;
        }
        //public static string GetMiddleValue(string text,string startstr,string endstr)
        //{
        //    string _value = ContentUtil.GetRegValue(text, string.Format(@"{0}(?<Middle>.*?){1}",startstr,endstr), false, replaceenter: false);
        //    string _bultrason = string.Empty;
        //    if (!string.IsNullOrWhiteSpace(_value))
        //    {
        //        _bultrason = (text + "").Replace(_value, "");
        //    }
        //    else
        //    {
        //        _bultrason = (text + "");
        //    }
        //    return ContentUtil.RemoveRN(_bultrason);
        //}


        public static string GetMiddleValue(string OrgStr, string Start, string End)
        {
            return GetRegValue(OrgStr, string.Format("(?<={0})([.\\S\\s]*)(?={1})", Start, End), false, replaceenter: false);
        }

        private static string[] cstr = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };   //定义数组
        private static string[] wstr = { "", "", "十" };
        public static string ConvertChinese(string str)
        {
            int len = str.Length;    //获取输入文本的长度值
            string tmpstr = "";      //定义字符串
            string rstr = "";
            for (int i = 1; i <= len; i++)
            {
                tmpstr = str.Substring(len - i, 1);    //截取输入文本 再将值赋给新的字符串
                rstr = string.Concat(cstr[Int32.Parse(tmpstr)] + wstr[i], rstr);  //将两个数组拼接在一起形成新的字符串
            }
            rstr = rstr.Replace("十零", "十");      //将新的字符串替换
            rstr = rstr.Replace("一十", "十");
            return rstr;                            //返回新的字符串
        }

        public static void AddUnit(string key, ref Dictionary<string, object> items, string unit)
        {
            if (string.IsNullOrWhiteSpace(key) || items == null || items.Count == 0)
                return;
            if (string.IsNullOrWhiteSpace(unit))
                return;
            decimal val = DicUtil.GetDicValue<decimal>(key, items);
            if (val > 0m)
                DicUtil.ContainsKey(ref items, key, val + unit);
        }
        public static string AddUnit(string key, Dictionary<string, object> items, string unit)
        {
            if (string.IsNullOrWhiteSpace(key) || items == null || items.Count == 0)
                return null;

            decimal val = DicUtil.GetDicValue<decimal>(key, items);
            if (string.IsNullOrWhiteSpace(unit))
                return val.ToString();
            if (val > 0m)
                return val + unit;
            return val.ToString();
        }
        public static string AddTitle(string title, string text, string cut = "")
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(title))
                sb.Append(title + "：");
            sb.Append(text);
            if (!string.IsNullOrWhiteSpace(cut))
                sb.Append(cut);
            return sb.ToString();
        }
        public static string AddTitle(string title, string key, Dictionary<string, object> items, string cut)
        {
            string text = DicUtil.GetDicValue<string>(key, items);
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(key))
                return string.Empty;
            return AddTitle(title, text, cut);
        }
        public static string AddTitleUnit(string title, string val, string unit, string cut)
        {
            decimal d = 0m;
            decimal.TryParse(val, out d);
            if (d <= 0m)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(title))
                sb.Append(title + "：");
            sb.Append(d);
            if (!string.IsNullOrWhiteSpace(unit))
                sb.Append(unit);
            if (!string.IsNullOrWhiteSpace(cut))
                sb.Append(cut);
            return sb.ToString();
        }
        public static string AddTitleUnit(string title, string key, Dictionary<string, object> items, string unit, string cut)
        {
            decimal val = DicUtil.GetDicValue<decimal>(key, items);
            if (val <= 0m || string.IsNullOrWhiteSpace(key))
                return string.Empty;
            return AddTitleUnit(title, val + "", unit, cut);
        }
        public static bool IsDigital(object obj)
        {
            if (obj == null) return false;
            Type t = obj.GetType();
            return (t == typeof(int) || t == typeof(decimal) || t == typeof(float) || t == typeof(long) || t == typeof(double));
        }
        public static string GetConn(string basec, Dictionary<string, object> param)
        {
            if (!DicUtil.HasItem(param)) return basec;

            string sql = GetSql(basec, param);

            string dickey = string.Empty;
            string key = string.Empty;
            string midval = string.Empty;
            bool b = false;
            Dictionary<string, object> new_param = new Dictionary<string, object>();
            List<string> ls = HtmlUtil.GetColumnNames(basec);
            if (ls != null && ls.Count > 0)
            {
                ls.ForEach(x =>
                {
                    if (!DicUtil.ContainsKey(param, x))
                        DicUtil.ContainsKey(ref param, x, "");
                });
            }
            param.ToList().ForEach(x =>
            {
                key = "datablock." + x.Key;
                if ((sql + "").Contains(key))
                {
                    midval = GetFilterCondition(string.IsNullOrWhiteSpace(DicUtil.GetDicValue<string>(x.Key, param)), key, sql, ref dickey);
                    DicUtil.ContainsKey(ref new_param, dickey, midval);
                    b = true;
                }
            });
            if (b)
            {
                sql = GetSql(sql, new_param, addpre: false);
            }
            return sql;
        }
        public static string GetSql(string basec, Dictionary<string, object> param, bool addpre = true)
        {
            if (!DicUtil.HasItem(param)) return basec;
            param.ToList().ForEach(x =>
            {
                basec = (basec + "").Replace((addpre ? "#dataitems." : "") + x.Key + (addpre ? "#" : ""), x.Value + "");
            });
            return basec;
        }
        public static string GetFilterCondition(bool b_select, string key, string basestr, ref string dickey)
        {
            if (string.IsNullOrWhiteSpace(basestr) || string.IsNullOrWhiteSpace(key)) return string.Empty;

            string regstr = "#" + key + "=>{(?<key>.*?)}#";
            dickey = ContentUtil.GetRegValue(basestr, regstr, false, false);

            string[] values = ContentUtil.GetRegValue(basestr, regstr, "key", false, replaceenter: false);
            if (values == null || values.Length == 0) return string.Empty;

            string midval = values[0];
            if (!b_select)
            {
                return midval;
            }
            else
            {
                return string.Empty;
            }
        }
        public static bool SpecalExeclData(object text)
        {
            string s = text + "";
            return (s + "").Length > 0
                && new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" }.Contains((s + "").Substring(0, 1))
                && (((s + "").Length == 18 || (s + "").Length == 15) && IdentityCard.IsIdCardValid(s));
        }
    }
    public class StringUtil
    {
        public static List<T> CastToList<T>(ArrayList a)
        {
            if (a == null || a.Count == 0) return null;
            return a.Cast<T>().ToList();
        }
        public static string GenerateString<T>(IEnumerable<T> arr)
        {
            if (arr == null || !arr.Any())
                return null;
            return GenerateString<T>(arr.ToArray());
        }
        public static string GenerateString<T>(T[] arr)
        {
            string rtn = string.Empty;
            if (arr == null || arr.Length == 0)
                return rtn;
            bool isfirst = true;
            List<T> ls = new List<T>();
            foreach (T tmp in arr)
            {
                if (string.IsNullOrWhiteSpace(tmp + ""))
                    continue;
                if (ls.Contains(tmp))
                    continue;
                if (!isfirst)
                    rtn += ",";
                if (isfirst)
                {
                    isfirst = false;
                }
                if (tmp is string)
                    rtn += "'" + tmp + "'";
                else
                    rtn += tmp;
                ls.Add(tmp);
            }
            return rtn;
        }
        public static string SpliceString<T>(T[] arr, string cut = ",", bool judgecontain = true, bool judgeisnull = true)
        {
            string rtn = string.Empty;
            if (arr == null || arr.Length == 0)
                return rtn;
            bool isfirst = true;
            List<T> ls = new List<T>();
            foreach (T tmp in arr)
            {
                if (string.IsNullOrWhiteSpace(tmp + "") && judgeisnull)
                    continue;
                if (ls.Contains(tmp) && judgecontain)
                    continue;
                if (!isfirst)
                    rtn += cut;
                if (isfirst)
                {
                    isfirst = false;
                }
                if (tmp is string)
                    rtn += "" + tmp + "";
                else
                    rtn += tmp;
                ls.Add(tmp);
            }
            return rtn;
        }
        public static string SpliceString<T>(IEnumerable<T> arr)
        {
            if (arr == null || !arr.Any())
                return null;
            return SpliceString<T>(arr.ToArray());
        }
        public static string SpliceString<T>(IEnumerable<T> arr, string cut, bool judgecontain = true)
        {
            if (arr == null || !arr.Any())
                return null;
            return SpliceString<T>(arr.ToArray(), cut, judgecontain);
        }
        public static string Trim(object o)
        {
            return (o + "").Trim();
        }
        public static string TrimNumber(object o)
        {
            return (o + "").Trim(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
        }

        /// <summary>
        /// 获取中英文混排字符串的实际长度(字节数)
        /// </summary>
        /// <param name="str">要获取长度的字符串</param>
        /// <returns>字符串的实际长度值（字节数）</returns>
        public static int getStringLength(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return 0;
            int strlen = 0;
            ASCIIEncoding strData = new ASCIIEncoding();
            //将字符串转换为ASCII编码的字节数字
            byte[] strBytes = strData.GetBytes(str);
            for (int i = 0; i <= strBytes.Length - 1; i++)
            {
                if (strBytes[i] == 63)  //中文都将编码为ASCII编码63,即"?"号
                    strlen++;
                strlen++;
            }
            return strlen;

        }
        public static string SubStr(int a_StartIndex, int a_Cnt, string con)
        {
            byte[] l_byte = System.Text.Encoding.Default.GetBytes(con);
            return System.Text.Encoding.Default.GetString(l_byte, a_StartIndex, a_Cnt).Trim();
        }
        public static string GetFirstString(string stringToSub, int length)
        {
            //System.Text.Encoding en = System.Text.Encoding.GetEncoding("GB2312");

            //byte[] l_byte = en.GetBytes(stringToSub);
            //return en.GetString(l_byte, 0, length).Trim();

            Encoding encode = Encoding.GetEncoding("GB2312");
            byte[] byteArr = encode.GetBytes(stringToSub.Trim());
            if (byteArr.Length <= length) return stringToSub;

            int m = 0, n = 0;
            foreach (byte b in byteArr)
            {
                if (n >= length) break;
                if (b > 127) m++; //重要一步：对前p个字节中的值大于127的字符进行统计  
                n++;
            }
            if (m % 2 != 0) n = length - 1; //如果非偶：则说明末尾为双字节字符，截取位数加1 

            return encode.GetString(byteArr, 0, n).Trim();
            //Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);
            //char[] stringChar = stringToSub.ToCharArray();
            //StringBuilder sb = new StringBuilder();
            //int nLength = 0;
            //for (int i = 0; i < stringChar.Length; i++)
            //{
            //    if (regex.IsMatch((stringChar[i]).ToString()))
            //    {
            //        nLength += 2;
            //    }
            //    else
            //    {
            //        nLength = nLength + 1;
            //    }

            //    if (nLength >= length)
            //    {
            //        break;
            //    } 
            //    sb.Append(stringChar[i]);
            //} 
            //return sb.ToString();
        }
        /// <summary>  
        /// 按文本内容长度截取HTML字符串(支持截取带HTML代码样式的字符串)  
        /// </summary>  
        /// <param name="html">将要截取的字符串参数</param>  
        /// <param name="len">截取的字节长度</param>  
        /// <param name="endString">字符串末尾补上的字符串</param>  
        /// <returns>返回截取后的字符串</returns>  
        public static string HTMLSubstring(string html, int len, string endString)
        {
            if (string.IsNullOrEmpty(html) || html.Length <= len) return html;
            MatchCollection mcentiry, mchtmlTag;
            ArrayList inputHTMLTag = new ArrayList();
            string r = "", tmpValue;
            int rWordCount = 0, wordNum = 0, i = 0;
            Regex rxSingle = new Regex("^<(br|hr|img|input|param|meta|link)", RegexOptions.Compiled | RegexOptions.IgnoreCase)//是否单标签正则  
                , rxEndTag = new Regex("</[^>]+>", RegexOptions.Compiled)//是否结束标签正则  
                , rxTagName = new Regex("<([a-z]+)[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase)//获取标签名正则  
                , rxHtmlTag = new Regex("<[^>]+>", RegexOptions.Compiled)//html标签正则  
                , rxEntity = new Regex("&[a-z]{1,9};", RegexOptions.Compiled | RegexOptions.IgnoreCase)//实体正则  
                , rxEntityReverse = new Regex("§", RegexOptions.Compiled)//反向替换实体正则  
                ;
            html = html.Replace("§", "§");//替换字符§为他的实体“§”，以便进行下一步替换  
            mcentiry = rxEntity.Matches(html);//收集实体对象到匹配数组中  
            html = rxEntity.Replace(html, "§");//替换实体为特殊字符§，这样好控制一个实体占用一个字符  
            mchtmlTag = rxHtmlTag.Matches(html);//收集html标签到匹配数组中  
            html = rxHtmlTag.Replace(html, "__HTMLTag__");//替换为特殊标签  
            string[] arrWord = html.Split(new string[] { "__HTMLTag__" }, StringSplitOptions.None);//通过特殊标签进行拆分  
            wordNum = arrWord.Length;
            //获取指定内容长度及HTML标签  
            for (; i < wordNum; i++)
            {
                if (rWordCount + arrWord[i].Length >= len) r += arrWord[i].Substring(0, len - rWordCount) + endString;
                else r += arrWord[i];
                rWordCount += arrWord[i].Length;//计算已经获取到的字符长度  
                if (rWordCount >= len) break;
                //搜集已经添加的非单标签，以便封闭HTML标签对  
                if (i < wordNum - 1)
                {
                    tmpValue = mchtmlTag[i].Value;
                    if (!rxSingle.IsMatch(tmpValue))
                    { //不是单标签  
                        if (rxEndTag.IsMatch(tmpValue) && inputHTMLTag.Count > 0) inputHTMLTag.RemoveAt(inputHTMLTag.Count - 1);
                        else inputHTMLTag.Add(tmpValue);
                    }
                    r += tmpValue;
                }

            }
            //替换回实体  
            for (i = 0; i < mcentiry.Count; i++) r = rxEntityReverse.Replace(r, mcentiry[i].Value, 1);
            //封闭标签  
            for (i = inputHTMLTag.Count - 1; i >= 0; i--) r += "</" + rxTagName.Match(inputHTMLTag[i].ToString()).Groups[1].Value + ">";
            return r;
        }
        public static string GetNoNullOrWhiteSpaceString(string[] arr)
        {
            string rtn = string.Empty;
            if (arr == null || arr.Length == 0)
                return rtn;

            return arr.FirstOrDefault(x => { return !string.IsNullOrWhiteSpace(x + ""); });
        }
        public static bool Contains<T>(List<T> arr, T val)
        {
            return arr != null && arr.Contains(val);
        }
        public static bool Contains<T>(T[] arr, T val)
        {
            return arr != null && !string.IsNullOrWhiteSpace(val + "") && arr.Contains(val);
        }
        public static T ConvertTo<T>(object o)
        {
            if (o == null) return default(T);
            if (string.IsNullOrWhiteSpace(o + "")) return default(T);
            try
            {
                return (T)Convert.ChangeType(o, typeof(T));
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
    /// <summary>
    ///HtmlUtil 的摘要说明
    /// </summary>
    public class HtmlUtil
    {
        public static List<string> GetColumnNames(string conn)
        {
            var ss = ContentUtil.GetRegValue(conn, "#dataitems.(?<key>.*?)#", "key", false, false);
            if (ss != null && ss.Length > 0)
            {
                return ss.ToList();
            }
            return null;
        }
        public static string ReplaceHtmlTag_1(string html)
        {

            return (html + "").Replace("<", "＜").Replace(">", "＞");
        }
        public static string ReplaceHtmlTag(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
        public static string TextToHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;
            return html.Replace("\r\n", "<br />").Replace("\r", "<br />").Replace("\n", "<br />");
        }
        public static string TextOneLine(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            return text.Replace("\r", "\\r").Replace("\n", "\\n");
        }
        public static string UnTextOneLine(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            return text.Replace("\\r", "\r").Replace("\\n", "\n");
        }
        /// <summary>
        /// 字符串转Unicode
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Unicode编码后的字符串</returns>
        public static string String2Unicode(string source)
        {
            var bytes = Encoding.Unicode.GetBytes(source);
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0:x2}{1:x2}", bytes[i + 1], bytes[i]);
            }
            return stringBuilder.ToString();
        }
        /// <summary>  
        /// 字符串转为UniCode码字符串  
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        public static string StringToUnicode(string s)
        {
            char[] charbuffers = s.ToCharArray();
            byte[] buffer;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < charbuffers.Length; i++)
            {
                buffer = System.Text.Encoding.Unicode.GetBytes(charbuffers[i].ToString());
                sb.Append(String.Format("\\u{0:X2}{1:X2}", buffer[1], buffer[0]));
            }
            return sb.ToString();
        }
        /// <summary>  
        /// Unicode字符串转为正常字符串  
        /// </summary>  
        /// <param name="srcText"></param>  
        /// <returns></returns>  
        public static string UnicodeToString(string srcText)
        {
            string dst = "";
            string src = srcText;
            int len = srcText.Length / 6;
            for (int i = 0; i <= len - 1; i++)
            {
                string str = "";
                str = src.Substring(0, 6).Substring(2);
                src = src.Substring(6);
                byte[] bytes = new byte[2];
                bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                bytes[0] = byte.Parse(int.Parse(str.Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                dst += Encoding.Unicode.GetString(bytes);
            }
            return dst;
        }
    }
    /// <summary>
    ///TimeUtil 的摘要说明
    /// </summary>
    public class TimeUtil
    {
        public TimeUtil()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        public static List<DateTime> SpecificDate(string mode)
        {
            DateTime dp_timeS = DateTime.MinValue;
            DateTime dp_timeE = DateTime.MinValue;
            switch (mode)
            {
                case "today":
                    dp_timeS = DateTime.Today;
                    dp_timeE = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
                    break;
                case "yestday":
                    dp_timeS = DateTime.Today.AddDays(-1);
                    dp_timeE = DateTime.Today.AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                    break;
                case "week":
                    dp_timeS = DateTime.Today.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek))));
                    dp_timeE = DateTime.Today.AddDays(Convert.ToDouble((6 - Convert.ToInt16(DateTime.Now.DayOfWeek)))).AddHours(23).AddMinutes(59).AddSeconds(59);
                    break;
                case "lastweek":
                    dp_timeS = DateTime.Today.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek))) - 7);
                    dp_timeE = DateTime.Today.AddDays(Convert.ToDouble((6 - Convert.ToInt16(DateTime.Now.DayOfWeek))) - 7).AddHours(23).AddMinutes(59).AddSeconds(59);
                    break;
                case "month":
                    dp_timeS = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01"));
                    dp_timeE = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01")).AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                    break;
                case "lastmonth":
                    dp_timeS = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01")).AddMonths(-1);
                    dp_timeE = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01")).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                    break;
                case "year":
                    dp_timeS = DateTime.Parse(DateTime.Today.ToString("yyyy-01-01"));
                    dp_timeE = DateTime.Parse(DateTime.Today.ToString("yyyy-01-01")).AddYears(1).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                    break;
                case "lastyear":
                    dp_timeS = DateTime.Parse(DateTime.Today.ToString("yyyy-01-01")).AddYears(-1);
                    dp_timeE = DateTime.Parse(DateTime.Today.ToString("yyyy-01-01")).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                    break;
                case "onemonth":
                    dp_timeS = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01")).AddMonths(1);
                    dp_timeE = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01")).AddMonths(2).AddSeconds(-1);
                    break;
                case "threemonth":
                    dp_timeS = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01")).AddMonths(1);
                    dp_timeE = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01")).AddMonths(4).AddSeconds(-1);
                    break;
                case "sixmonth":
                    dp_timeS = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01")).AddMonths(1);
                    dp_timeE = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01")).AddMonths(7).AddSeconds(-1);
                    break;
                default:
                    break;
            }
            List<DateTime> ld = new List<DateTime>();
            ld.Add(dp_timeS);
            ld.Add(dp_timeE);
            return ld;
        }
        public static bool CheckDate(object dt, bool tranall = false)
        {
            return GetDateTimeShare(dt, tranall: tranall) != DateTime.MinValue;
        }
        //使用范围广，不可改动
        public static string GetDate(object dt, bool tranall = false)
        {
            return GetDate(dt, "yyyy-MM-dd", tranall: tranall);
        }
        public static string GetDate(object dt, string format, bool tranall = false)
        {
            if (dt == DBNull.Value)
                return string.Empty;
            if (string.IsNullOrWhiteSpace(dt + ""))
                return string.Empty;
            DateTime dtResult = dt.GetType() == typeof(DateTime) ? ((DateTime)dt) : GetDateTimeShare(dt, tranall: tranall);
            if (dtResult == DateTime.MinValue)
                return dt + "";

            return dtResult.ToString(format);
        }
        public static string GetEmptyDate(object dt, bool tranall = false)
        {
            if (GetDate(dt, tranall: tranall) == DateTime.MinValue.ToString("yyyy-MM-dd"))
            {
                return "-";
            }
            return GetDate(dt);
        }
        public static string GetEmptyDate(object dt, string _nonetext, bool tranall = false)
        {
            if (GetDate(dt, tranall: tranall) == DateTime.MinValue.ToString("yyyy-MM-dd"))
            {
                return _nonetext;
            }
            return GetDate(dt);
        }
        public static string GetEmptyDate(object dt, string _nonetext, string formate, bool tranall = false)
        {
            if (GetDate(dt, tranall: tranall) == DateTime.MinValue.ToString("yyyy-MM-dd"))
            {
                return _nonetext;
            }
            return GetDate(dt, formate, tranall: tranall);
        }
        public static DateTime GetDateTime(object dt, bool tranall = false)
        {
            return GetDateTimeShare(dt, tranall: tranall).Date;
        }
        public static DateTime GetDateTimeTotal(string dt, bool tranall = false)
        {
            return GetDateTimeShare(dt, tranall: tranall);
        }
        public static DateTime GetDateTimeShare(object dt, bool tranall = false)
        {
            if ((dt + "").Length < 6)
                return DateTime.MinValue;
            DateTime dtResult = new DateTime();
            if (dt == DBNull.Value)
                return DateTime.MinValue;
            if (!DateTime.TryParse(dt + "", out dtResult))
            {
                //类似9.1会被直接转掉
                string[] formats = { "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "yyyy-MM-ddTHH:mm:ss", "yyyy/MM/dd HH:mm:ss", "yyyy/M/d HH:mm:ss" };
                if (tranall)
                {
                    List<string> b = formats.ToList();
                    b.Add("yyyyMMddHHmmss");
                    b.Add("yyyyMMdd");
                    formats = b.ToArray();
                }
                if (!DateTime.TryParseExact(dt + "", formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out dtResult))
                {
                    return DateTime.MinValue;
                }
            }
            return dtResult;
        }
        public static int GetAge(object dt)
        {
            DateTime dttime = new DateTime();
            if (!DateTime.TryParse(dt + "", out dttime))
            {
                return 0;
            }
            return (int)(Math.Ceiling((GetDateTime(DateTime.Now + "") - GetDateTime(dttime + "")).TotalDays) / 365);
        }
        public static int GetAge(object dt, object now)
        {
            DateTime dttime = new DateTime();
            DateTime dtnow = new DateTime();
            if (!DateTime.TryParse(dt + "", out dttime) || !DateTime.TryParse(now + "", out dtnow))
            {
                return 0;
            }
            //return (dtnow.Year - dttime.Year) + 1;
            return (int)(Math.Abs(Math.Ceiling((GetDateTime(dtnow + "") - GetDateTime(dttime + "")).TotalDays) / 365));
        }
        public static int GetWeek(int days)
        {
            return (int)(days / 7);
        }
        public static int GetWeek(object strdt1, object strdt2)
        {
            DateTime dt1;
            DateTime dt2;
            if (!string.IsNullOrWhiteSpace(strdt1 + "") &&
                DateTime.TryParse(strdt1 + "", out dt1) &&
                !string.IsNullOrWhiteSpace(strdt2 + "") &&
                DateTime.TryParse(strdt2 + "", out dt2))
            {
                return (int)(Math.Ceiling((GetDateTime(dt2.Date + "") - GetDateTime(dt1.Date + "")).TotalDays) / 7);
            }
            return -1;
        }
        public static int GetWeekOfDay(int days)
        {
            return (int)(days % 7);
        }
        public static int GetWeekOfDay(object strdt1, object strdt2)
        {
            DateTime dt1;
            DateTime dt2;
            if (!string.IsNullOrWhiteSpace(strdt1 + "") &&
                DateTime.TryParse(strdt1 + "", out dt1) &&
                !string.IsNullOrWhiteSpace(strdt2 + "") &&
                DateTime.TryParse(strdt2 + "", out dt2))
            {
                return (int)(Math.Ceiling((GetDateTime(dt2.Date + "") - GetDateTime(dt1.Date + "")).TotalDays) % 7);
            }
            return -1;
        }
        public static int GetDay(object strdt1, object strdt2)
        {
            DateTime dt1;
            DateTime dt2;
            if (!string.IsNullOrWhiteSpace(strdt1 + "") &&
                DateTime.TryParse(strdt1 + "", out dt1) &&
                !string.IsNullOrWhiteSpace(strdt2 + "") &&
                DateTime.TryParse(strdt2 + "", out dt2))
            {
                return (int)(Math.Ceiling((GetDateTime(dt2.Date + "") - GetDateTime(dt1.Date + "")).TotalDays));
            }
            return -1;
        }
        public static DateTime GetDate(DateTime dt, int week)
        {
            if (dt == null)
                return new DateTime();
            return dt.AddDays(week * 7);
        }
        public static DateTime GetDate(string strDate, int week)
        {
            DateTime dt = Convert.ToDateTime(strDate);
            return GetDate(dt, week);
        }

        public static string GetDatePart(object dt, string partname, int maxlen = 4)
        {
            DateTime d = GetDateTimeTotal(dt + "");
            if (d == DateTime.MinValue)
                return string.Empty;
            if (partname.ToLower() == "y")
            {
                if (maxlen == 4)
                    return (d.Year + "");
                else
                    return (d.Year + "").Substring(maxlen);
            }
            else if (partname.ToLower() == "m")
            {
                return (d.Month + "").PadLeft(maxlen, '0');
            }
            else if (partname.ToLower() == "d")
            {
                return (d.Day + "").PadLeft(maxlen, '0');
            }
            else if (partname.ToLower() == "h")
            {
                return (d.Hour + "").PadLeft(maxlen, '0');
            }
            else if (partname.ToLower() == "f")
            {
                return (d.Minute + "").PadLeft(maxlen, '0');
            }
            else if (partname.ToLower() == "s")
            {
                return (d.Second + "").PadLeft(maxlen, '0');
            }
            return string.Empty;
        }
        public static int GetAge_Month(object dt)
        {
            return GetAge_Month(dt, DateTime.Today);
        }
        public static int GetAge_Month(object dtS, object dtE)
        {
            DateTime dttimeS = GetDateTime(dtS);
            DateTime dttimeE = GetDateTime(dtE);
            if (dttimeS == DateTime.MinValue || dttimeE == DateTime.MinValue)
                return 0;
            return (int)(Math.Ceiling((dttimeE - dttimeS).TotalDays) / 30);
        }
    }
    /// <summary>
    ///IdentityCard 的摘要说明
    /// </summary>
    public class IdentityCard
    {
        public enum Gender
        {
            female = 2,
            male = 1,
            none = 9
        }
        private string _id15;
        private string _id18;
        private bool _valid;
        private string _local;

        public string CardNo;
        public bool IsValid
        {
            get
            {
                return _valid;
            }
            set
            {
                _valid = value;
            }
        }
        public string BirthDate
        {
            get
            {
                if (_valid)
                    return BirthYear + '-' + BirthMonth + '-' + BirthDay;
                return string.Empty;
            }
            set { }
        }
        // 返回生日中的年，格式如下，1981
        public string BirthYear
        {
            get
            {
                var year = "";
                if (_valid)
                    year = ID18.Substring(6, 4);
                return year;
            }
            set { }
        }
        // 返回生日中的月，格式如下，10
        public string BirthMonth
        {
            get
            {
                var month = "";
                if (_valid)
                    month = ID18.Substring(10, 2);
                if (month.Substring(0, 1) == "0")
                    month = month.Substring(1, 1);
                return month;
            }
            set { }
        }
        // 返回生日中的日，格式如下，10
        public string BirthDay
        {
            get
            {
                var day = "";
                if (_valid)
                    day = ID18.Substring(12, 2);
                return day;
            }
            set { }
        }
        // 返回性别，1：男，0：女
        public int Sex
        {
            get
            {
                int sex = -1;
                if (_valid)
                    sex = Convert.ToInt32(ID18.Substring(16, 1)) % 2;
                return sex;
            }
            set { }
        }
        public string ID15
        {
            get
            {
                var id15 = "";
                if (_valid)
                    id15 = this._id15;
                return id15;
            }
            set
            {
                _id15 = value;
            }
        }
        public string ID18
        {
            get
            {
                var id18 = "";
                if (_valid)
                    id18 = this._id18;
                return id18;
            }
            set
            {
                _id18 = value;
            }
        }
        // 返回所在省，例如：上海市、浙江省
        public string Local
        {
            get
            {
                var local = "";
                if (_valid) local = this._local;
                return local;
            }
            set
            {
                _local = value;
            }
        }
        // 返回年龄
        public int Age
        {
            get
            {
                int age = 0;
                if (_valid)
                    age = TimeUtil.GetAge(Convert.ToDateTime(BirthDate));
                return age;
            }
            set { }
        }
        public IdentityCard()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        public IdentityCard(string CardNo)
        {
            this._id15 = "";
            this._id18 = "";
            this._valid = false;
            this._local = "";
            if (!string.IsNullOrWhiteSpace(CardNo))
            {
                this.SetCardNo(CardNo);
            }
        }
        public void SetCardNo(string CardNo)
        {
            this._id15 = "";
            this._id18 = "";
            this._local = "";
            this.CardNo = CardNo.Replace(" ", "");

            string strCardNo;
            if (CardNo.Length == 18)
            {
                if (!Regex.IsMatch(CardNo, @"^\d{17}(\d|x|X)$")) return;
                strCardNo = CardNo.ToUpper();
            }
            else
            {
                if (!Regex.IsMatch(CardNo, @"^\d{15}$")) return;
                strCardNo = CardNo.Substring(0, 6) + "19" + CardNo.Substring(6, 9);
                strCardNo += GetVCode(strCardNo);
            }
            IsValid = CheckValid(strCardNo);
        }
        public string GetVCode(string CardNo17)
        {
            var Wi = new object[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
            var Ai = new object[] { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };
            var cardNoSum = 0;
            for (var i = 0; i < CardNo17.Length; i++) cardNoSum += Convert.ToInt32(CardNo17.Substring(i, 1)) * Convert.ToInt32(Wi[i]);
            int seq = cardNoSum % 11;
            return Ai[seq] + "";
        }
        public bool CheckValid(string CardNo18)
        {
            if (string.IsNullOrWhiteSpace(CardNo18) || CardNo18.Length != 18)
                return false;
            if (this.GetVCode(CardNo18.Substring(0, 17)) != CardNo18.Substring(17, 1))
                return false;
            if (!this.IsDate(CardNo18.Substring(6, 8)))
                return false;
            Dictionary<int, string> aCity = new Dictionary<int, string>();
            aCity.Add(11, "北京");
            aCity.Add(12, "天津");
            aCity.Add(13, "河北");
            aCity.Add(14, "山西");
            aCity.Add(15, "内蒙古");
            aCity.Add(21, "辽宁");
            aCity.Add(22, "吉林");
            aCity.Add(23, "黑龙江");
            aCity.Add(31, "上海");
            aCity.Add(32, "江苏");
            aCity.Add(33, "浙江");
            aCity.Add(34, "安徽");
            aCity.Add(35, "福建");
            aCity.Add(36, "江西");
            aCity.Add(37, "山东");
            aCity.Add(41, "河南");
            aCity.Add(42, "湖北");
            aCity.Add(43, "湖南");
            aCity.Add(44, "广东");
            aCity.Add(45, "广西");
            aCity.Add(46, "海南");
            aCity.Add(50, "重庆");
            aCity.Add(51, "四川");
            aCity.Add(52, "贵州");
            aCity.Add(53, "云南");
            aCity.Add(54, "西藏");
            aCity.Add(61, "陕西");
            aCity.Add(62, "甘肃");
            aCity.Add(63, "青海");
            aCity.Add(64, "宁夏");
            aCity.Add(65, "新疆");
            aCity.Add(71, "台湾");
            aCity.Add(81, "香港");
            aCity.Add(82, "澳门");
            aCity.Add(91, "国外");
            int i = 0;
            int.TryParse(CardNo18.Substring(0, 2), out i);
            if (!aCity.ContainsKey(i) || aCity[i] == null) return false;
            this.ID18 = CardNo18;
            this.ID15 = CardNo18.Substring(0, 6) + CardNo18.Substring(8, 9);
            this.Local = aCity[i];
            return true;
        }
        public bool IsDate(string strDate)
        {
            if (!Regex.IsMatch(strDate, @"^(\d{1,4})(\d{1,2})(\d{1,2})$"))
                return false;
            try
            {
                int year = Convert.ToInt32(strDate.Substring(0, 4));
                int month = Convert.ToInt32(strDate.Substring(4, 2));
                int day = Convert.ToInt32(strDate.Substring(6, 2));
                DateTime dt = new DateTime(year, month, day);

                return dt.Year == year && dt.Month == month && dt.Day == day;
            }
            catch
            {
                return false;
            }
        }
        public static int GetAge(string CardNo)
        {
            if (string.IsNullOrWhiteSpace(CardNo))
                return -1;
            IdentityCard ic = new IdentityCard(CardNo.Trim());
            if (ic.IsValid)
            {
                int age = TimeUtil.GetAge(ic.BirthDate, DateTime.Today);
                return age;
            }
            return -1;
        }
        public static string GetBirthDay(string CardNo)
        {
            if (string.IsNullOrWhiteSpace(CardNo))
                return null;
            IdentityCard ic = new IdentityCard(CardNo.Trim());
            if (ic.IsValid)
            {
                return ic.BirthDate;
            }
            return null;
        }
        public static int GetSexCode(string CardNo)
        {
            if (string.IsNullOrWhiteSpace(CardNo))
                return 9;
            IdentityCard ic = new IdentityCard(CardNo.Trim());
            if (ic.IsValid)
            {
                int sex = -1;
                if (ic.Sex == 1)
                    sex = (int)Gender.male;
                else if (ic.Sex == 0)
                    sex = (int)Gender.female;
                else if (ic.Sex == -1)
                    sex = (int)Gender.none;
                return sex;
            }
            return (int)Gender.none;
        }
        ///// <summary>
        ///// 转换15位身份证号码为18位
        ///// </summary>
        ///// <param name="oldIDCard">15位的身份证</param>
        ///// <returns>返回18位的身份证</returns>
        //public static string IDCard15To18(string oldIDCard)
        //{
        //    IdentityCard ic = new IdentityCard(oldIDCard);
        //    if (!ic.IsValid)
        //        return null;

        //    int iS = 0;

        //    //加权因子常数
        //    int[] iW = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
        //    //校验码常数
        //    string LastCode = "10X98765432";
        //    //新身份证号
        //    string newIDCard;

        //    newIDCard = oldIDCard.Substring(0, 6);
        //    //填在第6位及第7位上填上‘1’，‘9’两个数字
        //    newIDCard += "19";

        //    newIDCard += oldIDCard.Substring(6, 9);

        //    //进行加权求和
        //    for (int i = 0; i < 17; i++)
        //    {
        //        iS += int.Parse(newIDCard.Substring(i, 1)) * iW[i];
        //    }

        //    //取模运算，得到模值
        //    int iY = iS % 11;
        //    //从LastCode中取得以模为索引号的值，加到身份证的最后一位，即为新身份证号。
        //    newIDCard += LastCode.Substring(iY, 1);
        //    return newIDCard;
        //}
        public static string IDCard15to18(string idcard15)
        {
            if (!IdentityCard.IsIdCardValid(idcard15))
                return idcard15;
            string s;
            int sum, n, p;
            if (string.IsNullOrWhiteSpace(idcard15))
            {
                return idcard15;
            }
            idcard15 = idcard15.Trim();
            if (idcard15.Length != 15)
            {
                return idcard15;
            }
            s = idcard15.Substring(0, 6) + "19" + idcard15.Substring(6);
            sum = 0;
            p = 1;
            for (int i = 17; i >= 1; i--)
            {
                p *= 2;
                p %= 11;
                if (!int.TryParse(s[i - 1].ToString(), out n))
                {
                    return idcard15;
                }
                sum += n * p;
            }
            sum %= 11;
            sum = (12 - sum) % 11;
            if (sum == 10)
            {
                return s + "X";
            }
            else
            {
                return s + sum.ToString();
            }
        }
        public static bool IsIdCardValid(string idcard)
        {
            if (string.IsNullOrWhiteSpace(idcard))
                return false;
            IdentityCard ic = new IdentityCard((idcard + "").Trim());
            if (ic != null && ic.IsValid)
            {
                return true;
            }
            return false;
        }
        public static string[] GetAllIdcard(string idcard)
        {
            if (!IdentityCard.IsIdCardValid(idcard))
                return new string[] { idcard };
            List<string> ls = new List<string>();
            IdentityCard ic = new IdentityCard(idcard);
            if (ic == null)
                return new string[] { idcard };
            ls.Add(ic.ID15);
            ls.Add(ic.ID18);

            return ls.ToArray();
        }
    }
}