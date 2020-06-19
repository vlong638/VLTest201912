using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.GJPredeliveryAsync
{
    public static  class PreDeliveryParser
    {
        public static List<string> GetMatches(this string str, string start, string end)
        {
            List<string> result = new List<string>();
            var temp = "";//临时字符串,用以判断起止
            var cStart = start.ToCharArray();
            var cEnd = end.ToCharArray();
            var isStart = false;
            var current = "";//当前内容项
            foreach (var c in str)
            {
                if (!isStart && cStart.Contains(c))
                {
                    if (start[temp.Length] != c)
                    {
                        temp = start[0] == c ? c.ToString() : "";

                    }
                    else
                    {
                        temp += c;
                        if (temp == start)
                        {
                            isStart = true;
                            temp = "";
                            current = start;
                        }
                    }
                    continue;
                }
                current += c;
                if (isStart && cEnd.Contains(c))
                {
                    if (end[temp.Length] != c)
                    {
                        temp = end[0] == c ? c.ToString() : "";

                    }
                    else
                    {
                        temp += c;
                        if (temp == end)
                        {
                            isStart = false;
                            temp = "";
                            result.Add(current);
                            current = "";
                        }
                    }
                }
            }
            return result;
        }

        public static List<PreDelivery> GetPreDeliveries(string text, ref StringBuilder log)
        {
            List<PreDelivery> ps = new List<PreDelivery>();
            var trs = text.GetMatches("<tr>", "</tr>");
            foreach (var tr in trs)
            {
                #region 0309 增加对特殊字符的支持 &;
                //<tr><td style="border-bottom:1px solid black; border-left:1px solid black; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:4%"><p style="text-align:justify">3.8</p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:4%px"><p style="text-align:justify">4：10</p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:4%px"><p style="text-align:justify"><br></p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:4%px"><p style="text-align:justify"><br></p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:4%px"><p style="text-align:justify"><br></p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:4%px"><p style="text-align:justify">LOA&nbsp;</p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:4%px"><p style="text-align:justify"><br></p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:4%px"><p style="text-align:justify">140</p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:4%px"><p style="text-align:justify">30</p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:4%px"><p style="text-align:justify">5-6</p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:4%px"><p style="text-align:justify">中</p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:4%px"><p style="text-align:justify">头</p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:4%px"><p style="text-align:justify">S=-3</p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:4%px"><p style="text-align:justify">100%</p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:4%px"><p style="text-align:justify">2</p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:4%px"><p style="text-align:justify">完整</p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:5%px"><p style="text-align:justify"><br></p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:14%px"><p style="text-align:justify"><br></p></td><td style="border-bottom:1px solid black; border-left:none; border-right:1px solid black; border-top:none; height:20px; vertical-align:top; width:7%px"><p style="text-align:justify">唐唐鹤</p></td></tr> 
                #endregion

                PreDelivery entity = new PreDelivery();
                var tds = tr.GetMatches(@""">", "</");
                var contents = tds.Select(c => c.TrimStart(@"""><p style=""text-align:justify"">").TrimEnd("</")).ToList();
                if (contents.Count() > 0)
                {
                    log.AppendLine($"匹配到数据:count:{contents.Count()},data1:{contents[0]},data2:{contents[1]}");
                }
                //Regex regex = new Regex(@""">(<p style=""text-align:justify"">)?([\w+\.<&;>\s-""：:，,=%/]?)(</)?");
                //var matches = regex.Matches(tr);
                //var matchList = new List<Match>();
                //foreach (Match match in matches)
                //{
                //    matchList.Add(match);
                //}
                //var contents = matchList.Select(c => c.Groups[2].Value.TrimEnd(@"</p>").TrimEnd(@"<br>")).ToList();
                if (contents.Count() == 17)
                {
                    int index = 0;
                    var 时间 = GetHtmlValue(contents, index);
                    var 日期 = GetHtmlValue(contents, ++index);
                    var 体温 = GetHtmlValue(contents, ++index);
                    var 血压 = GetHtmlValue(contents, ++index);
                    var 脉搏 = GetHtmlValue(contents, ++index);
                    var 胎方位 = GetHtmlValue(contents, ++index);

                    var 胎心_位置 = GetHtmlValue(contents, ++index);
                    var 胎心_速度 = GetHtmlValue(contents, ++index);
                    var 宫缩_All = GetHtmlValue(contents, ++index);
                    var 先露 = GetHtmlValue(contents, ++index);
                    var 高位 = GetHtmlValue(contents, ++index);

                    var 宫颈_容受 = GetHtmlValue(contents, ++index);
                    var 宫颈_扩张 = GetHtmlValue(contents, ++index);
                    var 胎膜 = GetHtmlValue(contents, ++index);
                    var 出入量 = GetHtmlValue(contents, ++index);
                    var 特殊记录 = GetHtmlValue(contents, ++index);
                    var 检查者 = GetHtmlValue(contents, ++index);

                    if (string.IsNullOrEmpty(时间)
                    && string.IsNullOrEmpty(日期)
                    && string.IsNullOrEmpty(体温)
                    && string.IsNullOrEmpty(血压)
                    && string.IsNullOrEmpty(脉搏)
                    && string.IsNullOrEmpty(胎方位)
                    && string.IsNullOrEmpty(胎心_位置)
                    && string.IsNullOrEmpty(胎心_速度)
                    && string.IsNullOrEmpty(先露)
                    && string.IsNullOrEmpty(高位)
                    && string.IsNullOrEmpty(宫颈_容受)
                    && string.IsNullOrEmpty(宫颈_扩张)
                    && string.IsNullOrEmpty(胎膜)
                    && string.IsNullOrEmpty(出入量)
                    && string.IsNullOrEmpty(特殊记录)
                    && string.IsNullOrEmpty(检查者))
                        break;

                    var date = 时间.Split('.', ',', '，', '-');
                    var time = 日期.Split(':', '：');
                    var dateTime = DateTime.MinValue;
                    int month, day, hour, minute;
                    if (date.Count() == 2 && time.Count() == 2
                        && int.TryParse(date[0], out month)
                        && int.TryParse(date[1], out day)
                        && int.TryParse(time[0], out hour)
                        && int.TryParse(time[1], out minute))
                    {
                        dateTime = new DateTime(DateTime.Now.Year, month, day, hour, minute, 0);
                    }
                    else
                    {
                        continue;
                    }
                    entity.daichan_date = dateTime;
                    entity.daichan_time = dateTime;
                    entity.tiwen = 体温;
                    var bloodPressure = 血压.Split('/');
                    if (bloodPressure.Count() == 2)
                    {
                        entity.xueya1 = bloodPressure[0];
                        entity.xueya2 = bloodPressure[1];
                    }
                    entity.maibo = 脉搏;
                    entity.tai_fangwei = 胎方位;
                    //胎心.位置 无对应项
                    entity.tai_xin = 胎心_速度;
                    FetalPresentationType fetalPresentation;
                    if (Enum.TryParse<FetalPresentationType>(先露, out fetalPresentation))
                        entity.FetalPresentation = fetalPresentation;
                    entity.xianlu = 高位;
                    entity.Capacity = 宫颈_容受;
                    entity.gongkou = 宫颈_扩张;
                    entity.tai_mo = 胎膜;
                    List<string> qita = new List<string>();
                    if (!string.IsNullOrEmpty(出入量)) qita.Add(出入量);
                    if (!string.IsNullOrEmpty(特殊记录)) qita.Add(特殊记录);
                    entity.qita = string.Join(",", qita);
                    entity.jiancha_ren = 检查者;
                    ps.Add(entity);
                }
                else if (contents.Count() == 19)
                {
                    int index = 0;
                    var 时间 = GetHtmlValue(contents, index);
                    var 日期 = GetHtmlValue(contents, ++index);
                    var 体温 = GetHtmlValue(contents, ++index);
                    var 血压 = GetHtmlValue(contents, ++index);
                    var 脉搏 = GetHtmlValue(contents, ++index);
                    var 胎方位 = GetHtmlValue(contents, ++index);
                    var 胎心_位置 = GetHtmlValue(contents, ++index);
                    var 胎心_速度 = GetHtmlValue(contents, ++index);
                    var 宫缩_持续 = GetHtmlValue(contents, ++index);
                    var 宫缩_间隔 = GetHtmlValue(contents, ++index);
                    var 宫缩_性质 = GetHtmlValue(contents, ++index);
                    var 先露 = GetHtmlValue(contents, ++index);
                    var 高位 = GetHtmlValue(contents, ++index);
                    var 宫颈_容受 = GetHtmlValue(contents, ++index);
                    var 宫颈_扩张 = GetHtmlValue(contents, ++index);
                    var 胎膜 = GetHtmlValue(contents, ++index);
                    var 出入量 = GetHtmlValue(contents, ++index);
                    var 特殊记录 = GetHtmlValue(contents, ++index);
                    var 检查者 = GetHtmlValue(contents, ++index);

                    if (string.IsNullOrEmpty(时间)
                    && string.IsNullOrEmpty(日期)
                    && string.IsNullOrEmpty(体温)
                    && string.IsNullOrEmpty(血压)
                    && string.IsNullOrEmpty(脉搏)
                    && string.IsNullOrEmpty(胎方位)
                    && string.IsNullOrEmpty(胎心_位置)
                    && string.IsNullOrEmpty(胎心_速度)
                    && string.IsNullOrEmpty(宫缩_持续)
                    && string.IsNullOrEmpty(宫缩_间隔)
                    && string.IsNullOrEmpty(宫缩_性质)
                    && string.IsNullOrEmpty(先露)
                    && string.IsNullOrEmpty(高位)
                    && string.IsNullOrEmpty(宫颈_容受)
                    && string.IsNullOrEmpty(宫颈_扩张)
                    && string.IsNullOrEmpty(胎膜)
                    && string.IsNullOrEmpty(出入量)
                    && string.IsNullOrEmpty(特殊记录)
                    && string.IsNullOrEmpty(检查者))
                        break;

                    var date = 时间.Split('.', ',', '，', '-');
                    var time = 日期.Split(':', '：');
                    var dateTime = DateTime.MinValue;
                    int year = DateTime.Now.Year;
                    int month, day, hour, minute;
                    if (date.Count() == 2 && time.Count() == 2
                        && int.TryParse(date[0], out month)
                        && int.TryParse(date[1], out day)
                        && int.TryParse(time[0], out hour)
                        && int.TryParse(time[1], out minute))
                    {
                        dateTime = new DateTime(year, month, day, hour, minute, 0);
                        log.AppendLine($"识别数据成功:date:{date},time:{time}");
                    }
                    else if (date.Count() == 3 && time.Count() == 2
                        && int.TryParse(date[0], out year)
                        && int.TryParse(date[1], out month)
                        && int.TryParse(date[2], out day)
                        && int.TryParse(time[0], out hour)
                        && int.TryParse(time[1], out minute))
                    {
                        dateTime = new DateTime(year, month, day, hour, minute, 0);
                        log.AppendLine($"识别数据成功:date:{date},time:{time}");
                    }
                    else
                    {
                        log.AppendLine($"识别数据失败:date:{date},time:{time}");
                        continue;
                    }
                    entity.daichan_date = dateTime;
                    entity.daichan_time = dateTime;
                    entity.tiwen = 体温;
                    var bloodPressure = 血压.Split('/');
                    if (bloodPressure.Count() == 2)
                    {
                        entity.xueya1 = bloodPressure[0];
                        entity.xueya2 = bloodPressure[1];
                    }
                    entity.maibo = 脉搏;
                    entity.tai_fangwei = 胎方位;
                    //胎心.位置 无对应项
                    entity.tai_xin = 胎心_速度;
                    entity.chixu = 宫缩_持续;
                    entity.jiange = 宫缩_间隔;
                    entity.gongsuo_qd = 宫缩_性质;
                    FetalPresentationType fetalPresentation;
                    if (Enum.TryParse<FetalPresentationType>(先露, out fetalPresentation))
                        entity.FetalPresentation = fetalPresentation;
                    entity.xianlu = 高位;
                    entity.Capacity = 宫颈_容受;
                    entity.gongkou = 宫颈_扩张;
                    entity.tai_mo = 胎膜;
                    List<string> qita = new List<string>();
                    if (!string.IsNullOrEmpty(出入量)) qita.Add(出入量);
                    if (!string.IsNullOrEmpty(特殊记录)) qita.Add(特殊记录);
                    entity.qita = string.Join(",", qita);
                    entity.jiancha_ren = 检查者;
                    ps.Add(entity);
                }
                else
                {
                    continue;
                }
            }
            return ps;
        }

        public static string GetHtmlValue(List<string> htmls, int index)
        {
            return htmls[index].Trim("<br>");
        }
    }
}
