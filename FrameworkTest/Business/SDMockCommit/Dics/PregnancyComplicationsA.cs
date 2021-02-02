using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameworkTest.Business.SDMockCommit
{
    //(\w+)\s+([\w\(\\、)]+)
    //{"$1" ,"$2"},
    //<Option value = "(\w+)" text="([\w\(\\、]+)"/>
    //存在中英文标点符号问题,作替换
    //存在名称不对称问题 后续处理需做应对 忽略无法匹配的
    public partial class VLConstraints
    {
        //运行时常量 readonly static (引用型),编译时常量 const (值类型)
        public readonly static Dictionary<string, string> PregnancyComplicationsA_FS = new Dictionary<string, string>()
        {
            {"1" ,"无"},
            {"2" ,"妊娠合并慢性高血压"},
            {"3" ,"妊娠合并重症肝炎"},
            {"4" ,"妊娠合并慢性肾炎"},
            {"5" ,"妊娠合并其他内科疾病"},
            {"6" ,"妊娠合并心脏病"},
            {"7" ,"甲状腺功能亢进"},
            {"8" ,"甲状腺功能减退"},
            {"9" ,"癫痫"},
            {"10" ,"自身免疫性疾病"},
            {"11" ,"艾滋病"},
            {"12" ,"梅毒"},
            {"13" ,"乙型肝炎（活动期）"},
            {"14" ,"乙肝病毒携带者"},
        };

        public readonly static Dictionary<string, string> PregnancyComplicationsA_SD = new Dictionary<string, string>()
        {
            {"O10.000" ,"2"},
            {"O10.001" ,"2"},
            {"O10.100" ,"2"},
            {"O10.101" ,"2"},
            {"O10.200" ,"2"},
            {"O10.201" ,"2"},
            {"O10.300" ,"2"},
            {"O10.301" ,"2"},
            {"O10.400" ,"2"},
            {"O10.401" ,"2"},
            {"O10.900" ,"2"},
            {"O11.x00" ,"2"},
            {"O11.x01" ,"2"},
            {"O11.x02" ,"2"},
            {"O13.x00" ,"2"},
            {"O13.x01" ,"2"},
            {"O16.x00" ,"2"},
            {"O98.406" ,"3"},
            {"O26.801" ,"4"},
            {"O26.802" ,"4"},
            {"O26.803" ,"4"},
            {"O26.804" ,"4"},
            {"O99.222" ,"4"},
            {"O99.223" ,"4"},
            {"O99.426" ,"4"},
            {"O99.810" ,"4"},
            {"O99.413" ,"6"},
            {"O99.414" ,"6"},
            {"O99.415" ,"6"},
            {"O99.416" ,"6"},
            {"O99.418" ,"6"},
            {"O99.419" ,"6"},
            {"O99.420" ,"6"},
            //{"O99.421" ,"6"},
            {"O99.216" ,"7"},
            {"O99.215" ,"8"},
            //{"O99.421" ,"9"},
            {"O26.608" ,"10"},
            {"B24.x01" ,"11"},
            {"O98.100" ,"12"},
            {"O98.101" ,"12"},
            {"O98.402" ,"13"},
            {"Z22.502" ,"14"},
        };

        public static string GetPregnancyComplicationsA(IEnumerable<Diagnosis> diagnosises)
        {
            HashSet<string> pregnancyComplications = new HashSet<string>();
            foreach (var diagnosis in diagnosises)
            {
                if (!PregnancyComplicationsA_SD.ContainsKey(diagnosis.diag_code))
                    continue;
                var fsCode = PregnancyComplicationsA_SD[diagnosis.diag_code];
                if (pregnancyComplications.Contains(fsCode))
                    continue;
                pregnancyComplications.Add(fsCode);
            }
            if (diagnosises.FirstOrDefault(c => c.diag_code == "O99.421") != null)
            {
                pregnancyComplications.Add("6");
                pregnancyComplications.Add("9");
            }
            if (pregnancyComplications.Count == 0)
            {
                pregnancyComplications.Add("1");
            }
            return string.Join(",", pregnancyComplications);
        }
    }
}
