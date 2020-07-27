using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    /// <summary>
    /// 死亡诊断
    /// </summary>
    public partial class VLConstraints
    {
        public readonly static Dictionary<string, string> Diagnosis_FM = new Dictionary<string, string>()
        {
            {"O31.200","一个或多个胎儿宫内死亡后的继续妊娠"},
            {"O31.201","双胎妊娠一胎宫内死亡"},
            {"O36.400","为胎儿宫内死亡给予的孕产妇医疗"},
            {"O95.x00","产科死亡"},
            {"O96.x00","任何产科原因的死亡, 发生于分娩后42天以上至一年以内"},
            {"O97.x00","直接产科原因后遗症的死亡"},
            {"P01.600","胎儿和新生儿受母体死亡的影响"},
            {"P95.x00","胎儿死亡"},
            {"P96.803","新生儿死亡"},
            {"R96.000","瞬间死亡"},
            {"R96.100","死亡发生于症状起始后24小时以内，另无解释"},
            {"R98.x00","无人在场的死亡"},
            {"R99.x00","原因不明确，其他的死亡"},
            {"R99.x01","死亡"},
            {"T86.104","移植肾死亡"},
            {"Z35.209","具有新生儿死亡史妊娠监督"},
            {"Z63.400","与家庭成员失踪和死亡有关，具有潜在健康问题"},
        };

        public static bool HasDeadDiagnosis(IEnumerable<Diagnosis> diagnosis)
        {
            return diagnosis.FirstOrDefault(c => Diagnosis_FM.ContainsKey(c.diag_code)) != null;
        }
    }
}
