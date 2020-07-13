using FrameworkTest.Common.ValuesSolution;
using FrameworkTest.ConfigurableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    public class WMH_WCQBJ_GWYCF_SCORE_SAVERequest : List<WMH_WCQBJ_GWYCF_SCORE_SAVE>
    {
        internal void Update(List<WMH_GWYCF_GW_LIST1_Data> allHighRisks, List<HighRiskEntity> heleHighRisks, decimal? bmi,int? createage, ref StringBuilder logger)
        {
            var currentHighRisks = allHighRisks.Where(c => c.D8 == "1").ToList();
            logger.AppendLine($@"currentHighRisks;");
            logger.AppendLine(currentHighRisks.ToJson());

            #region BMI相关特殊处理

            //1	年龄：≤18岁	 a160101 	    年龄≥35岁或≤18岁 
            //2	年龄：&gt;35岁	 a160101 	    年龄≥35岁或≤18岁 
            //3	年龄≥40岁	 b160101 	    年龄≥40岁 

            var BMIs = new List<string>() { 
                "a160105", "b160105" //BMI
                ,"a160101","b160101" //年龄
            };
            for (int i = heleHighRisks.Count() - 1; i >= 0; i--)
            {
                var highRisk = heleHighRisks[i];
                if (BMIs.Contains(highRisk.R))
                {
                    heleHighRisks.Remove(highRisk);
                }
            }
            logger.AppendLine($">>>bmi:{bmi}");
            if (bmi.HasValue)
            {
                if (bmi >= 28)
                {
                    heleHighRisks.Add(new HighRiskEntity() { R = "6" });
                }
                else if (bmi > 25)
                {
                    heleHighRisks.Add(new HighRiskEntity() { R = "5" });
                }
                else if (bmi < 18.5M)
                {
                    heleHighRisks.Add(new HighRiskEntity() { R = "4" });
                }
            }
            logger.AppendLine($">>>createage:{createage}");
            if (createage.HasValue)
            { 
                if (createage <= 18)
                {
                    heleHighRisks.Add(new HighRiskEntity() { R = "1" });
                }
                else if (createage >= 40)
                {
                    heleHighRisks.Add(new HighRiskEntity() { R = "3" });
                }
                else if (createage > 35)
                {
                    heleHighRisks.Add(new HighRiskEntity() { R = "2" });
                }
            }
            #endregion

            //维护待提交数据
            var toAdds = heleHighRisks.Where(hele => currentHighRisks.FirstOrDefault(cur => VLConstraints.GetHighRisks_By_HighRisks_Hele(hele.R).Contains(cur.Id)) == null);
            var toDeletes = currentHighRisks.Where(cur => heleHighRisks.FirstOrDefault(hele => VLConstraints.GetHighRisks_By_HighRisks_Hele(hele.R).Contains(cur.Id)) == null);
            foreach (var toAdd in toAdds)
            {
                var toAddsFromAll = allHighRisks.Where(c => VLConstraints.GetHighRisks_By_HighRisks_Hele(toAdd.R).Contains(c.Id));
                foreach (var toAddFromAll in toAddsFromAll)
                {
                    if (this.FirstOrDefault(c => c.Id == toAddFromAll.Id) == null)
                    {
                        this.Add(new WMH_WCQBJ_GWYCF_SCORE_SAVE(toAddFromAll) { D8 = "1" });
                    }
                }
            }
            foreach (var toDelete in toDeletes)
            {
                if (this.FirstOrDefault(c => c.Id == toDelete.Id) == null)
                {
                    this.Add(new WMH_WCQBJ_GWYCF_SCORE_SAVE(toDelete) { D8 = "0" });
                }
            }

            //维护当前高危以更新专科检查
            AddCurrentHighRisks(allHighRisks.Where(c => this.FirstOrDefault(d => d.Id == c.Id && d.D8 == "1") != null));//新增
            AddCurrentHighRisks(allHighRisks.Where(c => c.D8 == "1" && this.FirstOrDefault(d => d.Id == c.Id && d.D8 == "0") == null));//未删除的
            CurrentHighRisks.OrderBy(c => c.Id);

            logger.AppendLine(">>>toAdds");
            logger.AppendLine(toAdds.ToJson());
            logger.AppendLine(">>>toDeletes");
            logger.AppendLine(toDeletes.ToJson());
            logger.AppendLine(">>>Current");
            logger.AppendLine(CurrentHighRisks.ToJson());

        }

        private void AddCurrentHighRisks(IEnumerable<WMH_GWYCF_GW_LIST1_Data> highRisks)
        {
            foreach (var highRisk in highRisks)
            {
                if (CurrentHighRisks.FirstOrDefault(c => c.Id == highRisk.Id) == null)
                {
                    CurrentHighRisks.Add(highRisk);
                }
            }
        }

        public List<WMH_GWYCF_GW_LIST1_Data> CurrentHighRisks = new List<WMH_GWYCF_GW_LIST1_Data>();
        public string GetCurrentHighRiskNames()
        {
            if (CurrentHighRisks.Count() == 0)
            {
                return "";
            }
            return string.Join(",", CurrentHighRisks.Select(c => c.D5));
        }
    }

    public class WMH_WCQBJ_GWYCF_SCORE_SAVE
    {
        public WMH_WCQBJ_GWYCF_SCORE_SAVE()
        {
        }
        public WMH_WCQBJ_GWYCF_SCORE_SAVE(string mainId, WMH_GWYCF_LIST_Data dataFromChange)
        {
            //WMH_GWYCF_LIST_Data
            //public string D1 { set; get; }//D1 :"2020-07-07",
            //public string D2 { set; get; }//D2 :"28+6",
            //public string D3 { set; get; }//D3 :"早产",
            //public string D4 { set; get; }//D4 :"",
            //public string D5 { set; get; }//D5 :"2020-07-07",
            //public string D6 { set; get; }//D6 :"28+6",
            //public string D7 { set; get; }//D7 :"",
            //public string D8 { set; get; }//D8 :"",
            //public string D9 { set; get; }//D9 :"",
            //public string D10 { set; get; }//D10 :"",
            //public string D11 { set; get; }//D11 :"2020-07-07",
            //public string D12 { set; get; }//D12 :"",
            //public string D13 { set; get; }//D13 :"",
            //public string D14 { set; get; }//D14 :"",
            //public string D15 { set; get; }//D15 :"A9D11542F2F395F7E05355FE8013B8D1",
            //public string D16 { set; get; }//D16 :"黄色(一般风险）",
            //public string D17 { set; get; }//D17 :"10"


            //public string D1 { set; get; }//D1 :"A9D6CFBEA85451B8E05355FE80130200"
            //public string D2 { set; get; }//D2 :"A8A7AEAD72C162A2E05355FE801348F3"
            //public string D3 { set; get; }//D3 :"基本情况"
            //public string D4 { set; get; }//D4 :"年龄"
            //public string D5 { set; get; }//D5 :"年龄≥40岁"
            //public string D6 { set; get; }//D6 :"橙色(较高风险)"
            //public string D7 { set; get; }//D7 :"转高危妊娠门诊，孕期合并症筛查，建议早期产前诊断，I级胎儿监护"
            //public string D8 { set; get; }//D8 :"0"
            //public string D9 { set; get; }//D9 :"3"
            //public string D10 { set; get; }//D10 :""
            //public string D11 { set; get; }//D11 :"取本次评估时年龄值"
            //public string D12 { set; get; }//D12 :"Ⅲ级"
            //public string D13 { set; get; }//D13 :"3"
            //public string D14 { set; get; }//D14 :""
            //public string D15 { set; get; }//D15 :""
            //public string _id { set; get; }//_id :13
            //public string _uid { set; get; }//_id :13

            this.D1 = dataFromChange.D15;
            this.D2 = mainId;
            this.D3 = "";
            this.D4 = "";
            this.D5 = dataFromChange.D3;
            this.D6 = dataFromChange.D16;
            this.D7 = dataFromChange.D10;
            this.D8 = "";//删除为0 新增为1
            this.D9 = "";
            this.D10 = "";
            this.D11 = "";
            this.D12 = "";
            this.D13 = "";
            this.D14 = "";
            this.D15 = "";
            this._id = "";
            this._uid = "";
        }

        public WMH_WCQBJ_GWYCF_SCORE_SAVE(WMH_GWYCF_GW_LIST1_Data dataFromAll)
        {
            //WMH_GWYCF_GW_LIST1_Data
            //public string D1 { set; get; }//:"A9C39D1285D0A96BE05355FE8013EFEA",
            //public string D2 { set; get; }//:"A8A7AEAD72C162A2E05355FE801348F3", //MainId 但有些有 有些没
            //public string D3 { set; get; }//:"基本情况",
            //public string D4 { set; get; }//:"年龄",
            //public string D5 { set; get; }//:"年龄：≤18岁",
            //public string D6 { set; get; }//:"黄色(一般风险）",
            //public string D7 { set; get; }//:"转高危妊娠门诊",
            //public string D8 { set; get; }//:"0",
            //public string D9 { set; get; }//:"1",
            //public string D10 { set; get; }//:"",
            //public string D11 { set; get; }//:"取本次评估时年龄值",
            //public string D12 { set; get; }//:"Ⅱ级",
            //public string D13 { set; get; }//:"1",
            //public string D14 { set; get; }//:"",
            //public string D15 { set; get; }//:""

            //public string D1 { set; get; }//D1 :"A9D6CFBEA85451B8E05355FE80130200"
            //public string D2 { set; get; }//D2 :"A8A7AEAD72C162A2E05355FE801348F3"
            //public string D3 { set; get; }//D3 :"基本情况"
            //public string D4 { set; get; }//D4 :"年龄"
            //public string D5 { set; get; }//D5 :"年龄≥40岁"
            //public string D6 { set; get; }//D6 :"橙色(较高风险)"
            //public string D7 { set; get; }//D7 :"转高危妊娠门诊，孕期合并症筛查，建议早期产前诊断，I级胎儿监护"
            //public string D8 { set; get; }//D8 :"0"
            //public string D9 { set; get; }//D9 :"3"
            //public string D10 { set; get; }//D10 :""
            //public string D11 { set; get; }//D11 :"取本次评估时年龄值"
            //public string D12 { set; get; }//D12 :"Ⅲ级"
            //public string D13 { set; get; }//D13 :"3"
            //public string D14 { set; get; }//D14 :""
            //public string D15 { set; get; }//D15 :""
            //public string _id { set; get; }//_id :13
            //public string _uid { set; get; }//_id :13

            //var dateStr = DateTime.Now.ToString("yyyy-MM-dd");
            //var week = VLConstraints.GetGestationalWeeksByPrenatalDate(dateofprenatal?.ToDateTime(), DateTime.Now);

            this.D1 = dataFromAll.D1;
            this.D2 = dataFromAll.D2;
            this.D3 = dataFromAll.D3;
            this.D4 = dataFromAll.D4;
            this.D5 = dataFromAll.D5;
            this.D6 = dataFromAll.D6;
            this.D7 = dataFromAll.D7;
            this.D8 = dataFromAll.D8;
            this.D9 = dataFromAll.D9;
            this.D10 = dataFromAll.D10;
            this.D11 = dataFromAll.D11;
            this.D12 = dataFromAll.D12;
            this.D13 = dataFromAll.D13;
            this.D14 = dataFromAll.D14;
            this.D15 = "";//D15 :"A9D11542F2F395F7E05355FE8013B8D1",
            this._id = "";
            this._uid = "";
        }

        public string D1 { set; get; }//D1 :"A9D6CFBEA85451B8E05355FE80130200"
        public string D2 { set; get; }//D2 :"A8A7AEAD72C162A2E05355FE801348F3"
        public string D3 { set; get; }//D3 :"基本情况"
        public string D4 { set; get; }//D4 :"年龄"
        public string D5 { set; get; }//D5 :"年龄≥40岁"
        public string D6 { set; get; }//D6 :"橙色(较高风险)"
        public string D7 { set; get; }//D7 :"转高危妊娠门诊，孕期合并症筛查，建议早期产前诊断，I级胎儿监护"
        public string D8 { set; get; }//D8 :"0"
        public string D9 { set; get; }//D9 :"3"
        public string D10 { set; get; }//D10 :""
        public string D11 { set; get; }//D11 :"取本次评估时年龄值"
        public string D12 { set; get; }//D12 :"Ⅲ级"
        public string D13 { set; get; }//D13 :"3"
        public string D14 { set; get; }//D14 :""
        public string D15 { set; get; }//D15 :""
        public string _id { set; get; }//_id :13
        public string _uid { set; get; }//_id :13

        internal string Id { get { return D9; } }
    }
}
