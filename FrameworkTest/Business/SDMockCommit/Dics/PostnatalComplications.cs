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
        public readonly static Dictionary<string, string> PostnatalComplications_FS = new Dictionary<string, string>()
        {
            {"1","无"},
            {"2","产褥中暑"},
            {"3","产褥感染"},
            {"4","急性乳房炎"},
            {"5","晚期产后出血"},
            {"6","产褥期精神障碍"},
            {"7","子宫复旧不全"},
            {"8","产后出血"},
        };

        public readonly static Dictionary<string, string> PostnatalComplications_SD = new Dictionary<string, string>()
        {
            //产褥中暑	
            {"T67.901","2"},
            //产褥感染	
            {"O86.800","3"},
            //急性乳房炎	
            {"O91.000","4"},
            {"O91.001","4"},
            {"O91.100","4"},
            {"O91.101","4"},
            {"O91.102","4"},
            {"O91.200","4"},
            {"O91.201","4"},
            {"O91.202","4"},
            //晚期产后出血	
            {"O72.200","5"},
            {"O72.202","5"},
            //产褥期精神障碍	
            {"F53.900","6"},
            //子宫复旧不全	
            {"O90.801","7"},
            //产后出血	
            {"O72.100","8"},
        };

        public static string GetPostnatalComplications(IEnumerable<Diagnosis> diagnosises)
        {
            HashSet<string> postnatalComplications = new HashSet<string>();
            foreach (var diagnosis in diagnosises)
            {
                if (!PostnatalComplications_SD.ContainsKey(diagnosis.diag_code))
                    continue;
                var fsCode = PostnatalComplications_SD[diagnosis.diag_code];
                if (postnatalComplications.Contains(fsCode))
                    continue;
                postnatalComplications.Add(fsCode);
            }
            if (postnatalComplications.Count == 0)
            {
                postnatalComplications.Add("1");
            }
            return string.Join(",", postnatalComplications);
        }
    }
}
