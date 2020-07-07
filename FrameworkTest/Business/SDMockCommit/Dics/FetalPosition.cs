using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    //(\w+)\s+([\w\(\)]+)
    //{"$1" ,"$2"},
    //<Option value = "(\w+)" text="([\w\(\\、\（\）]+)"/>
    //存在中英文标点符号问题,作替换
    //存在名称不对称问题 后续处理需做应对 忽略无法匹配的
    public partial class VLConstraints
    {
        public readonly static Dictionary<string, string> FetalPosition = new Dictionary<string, string>()
        {
            {"脐右下方" ,"脐右下方"},
            {"脐左下方" ,"脐左下方"},
            {"脐左上方" ,"脐左上方"},
            {"脐右上方" ,"脐右上方"},
            {"脐正中线上方" ,"脐正中线上方"},
            {"脐正中线下方" ,"脐正中线下方"},
        };

        public readonly static Dictionary<string, string> FetalPosition_HELE = new Dictionary<string, string>()
        {
            {"01" ,"左枕前（LOA）"},
            {"02" ,"右枕前（ROA）"},
            {"03" ,"左枕后（LOP）"},
            {"04" ,"右枕后（ROP）"},
            {"05" ,"左枕横（LOT）"},
            {"06" ,"右枕横（ROT）"},
            {"07" ,"左颏前（LMA）"},
            {"08" ,"右颏前（RMA）"},
            {"09" ,"左颏后（LMP）"},
            {"10" ,"右颏后（RMP）"},
            {"11" ,"左颏横（LMT）"},
            {"12" ,"右颏横（RMT）"},
            {"13" ,"左骶前（LSA）"},
            {"14" ,"右骶前（RSA）"},
            {"15" ,"左骶后（LSP）"},
            {"16" ,"右骶后（RSP）"},
            {"17" ,"左骶横（LST）"},
            {"18" ,"右骶横（RST）"},
            {"19" ,"左肩前（LScA）"},
            {"20" ,"右肩前（RscA）"},
            {"21" ,"左肩后（LScP）"},
            {"22" ,"右肩后（RScP）"},
            {"99" ,"不定"},
        };

        /// <summary>
        /// return "" when not contains key
        /// return matchedKey when matched
        /// return "" when not matched
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get_FetalPosition_By_FetalPosition_Hele(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (!EdemaStatus_Hele.ContainsKey(key))
                return "";
            switch (key)
            {
                case "02": return "脐右下方";
                case "01": return "脐左下方";
                case "13": return "脐左上方";
                case "14": return "脐右上方";
                case "20": return "脐正中线上方";
                case "19": return "脐正中线下方";
                default:
                    return "";
            }
        }
    }
}
   
