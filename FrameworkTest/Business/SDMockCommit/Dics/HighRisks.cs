﻿using System.Collections.Generic;
using System.Linq;

namespace FrameworkTest.Business.SDMockCommit
{
    public class HighRisksMapper
    {
        public string HLId { set; get; }
        public string FSId { set; get; }
    }

    public partial class VLConstraints
    {

        public readonly static Dictionary<string, string> HighRisks = new Dictionary<string, string>()
        {
            {"1",@"年龄：≤18岁"},
            {"2",@"年龄：&gt;35岁"},
            {"3",@"年龄≥40岁"},
            {"4",@"1.2 BMI&lt;18.5"},
            {"5",@"1.2 BMI&gt;25"},
            {"6",@"1.2 BMI≥28"},
            {"7",@"1.3生殖道畸形"},
            {"8",@"1.4骨盆狭小"},
            {"9",@"各类流产≥3次"},
            {"10",@"早产"},
            {"11",@"围产儿死亡"},
            {"12",@"出生缺陷"},
            {"13",@"异位妊娠"},
            {"14",@"滋养细胞疾病等"},
            {"15",@"子官肌瘤：≥5cm"},
            {"16",@"卵巢囊肿：≥5cm"},
            {"17",@"盆腔手术史"},
            {"18",@"辅助生殖妊娠"},
            {"19",@"3.1.1双绒双羊，无其他并发症"},
            {"20",@"3.6.2双绒双羊伴有并发症"},
            {"21",@"3.1.2单绒双羊，无其他并发症"},
            {"22",@"3.6.3单绒双羊伴有并发症"},
            {"23",@"3.6.4单羊单绒"},
            {"24",@"3.6.1双胎、羊水过多伴发心肺功能减退"},
            {"25",@"3.1Ⅲ胎及以上妊娠"},
            {"26",@"2.1Ⅲ胎及以上妊娠伴发心肺功能減退"},
            {"27",@"3.2.1先兆早产（孕周≥34周）"},
            {"28",@"3.2.2先兆早产（孕周＜34周）"},
            {"29",@"3.3.1FGR，胎儿未合并其他异常"},
            {"30",@"3.3.2FGR，胎儿合并其他异常"},
            {"31",@"巨大儿"},
            {"32",@"3.5妊娠期高血压疾病(除外红、橙色);"},
            {"33",@"3.7.1妊娠≥34周的轻度子痫前期；"},
            {"34",@"3.7.2妊娠＜34周的妊娠期高血压疾病；"},
            {"35",@"3.7.3重度子痫前期，无心脑肝等脏器严重损害，无HELLP综合症"},
            {"36",@"3.7.4HELLP综合症；"},
            {"37",@"1.1.8妊娠期高血压性心脏病"},
            {"38",@"妊娠期肝内胆计淤积症"},
            {"39",@"3.7.1≥34周的胎膜早破；"},
            {"40",@"3.7.2＜34周的胎膜早破"},
            {"41",@"3.7.3孕足月胎膜早破且破膜时间超过24小时"},
            {"42",@"羊水过少"},
            {"43",@"羊水过多"},
            {"44",@"≥36周胎位不正"},
            {"45",@"低置胎盘"},
            {"46",@"妊娠剧吐"},
            {"47",@"Rh血型不合"},
            {"48",@"1.6瘢痕子官"},
            {"49",@"3.3 疤痕子官(距末次子官手术间隔&lt;18月)"},
            {"50",@"3.4 疤痕子官伴中央性前置胎盘或伴有可疑胎盘植入"},
            {"51",@"3.5各类子官手术史(如剖官产、宫角妊娠、子官肌瘤挖除术等)≥2次"},
            {"52",@"凶险性前置胎盘"},
            {"53",@"胎盘早剥"},
            {"54",@"胎盘植入"},
            {"55",@"原因不明的发熱"},
            {"56",@"产后抑郁症"},
            {"57",@"产褥期中暑"},
            {"58",@"产褥感染等"},
            {"59",@"红色预警范畴疾病产后尚未稳定"},
            {"60",@"不伴有肺动脉高压的房缺、室缺、动脉导管未闭"},
            {"61",@"法乐氏四联症修补术后无残余心脏结构异常等"},
            {"62",@"2.1.2心肌炎后遗症"},
            {"63",@"2.1.3心律失常"},
            {"64",@"2.1.4无合并症的轻度的肺动脉狭窄和二尖瓣脱垂"},
            {"65",@"2.1.6各种原因的轻度肺动脉高压(＜50mmHg)"},
            {"66",@"2.1.5经治疗后稳定的心肌病"},
            {"67",@"2.1.2需药物治疗的心肌炎后遺症、心律失常等"},
            {"68",@"2.1.1心功能 II级, 轻度左心功能障碍或者EF40%~50%"},
            {"69",@"2.1.3瓣膜性心脏病(轻度二尖瓣狭窄瓣口＞1.5 cm2, 主动脉瓣狭窄跨瓣压差&lt;50mmHg,无合并症的轻度肺动脉狭窄,二尖瓣脱垂，二叶式主动脉瓣疾病, Marfan综合征无主动脉扩张)"},
            {"70",@"2.1.4主动脉疾病(主动脉直径&lt;45mm),主动脉缩窄娇治术后"},
            {"71",@"2.1.7其他较严重心血管疾病"},
            {"72",@"1.1.1各种原因引起的肺动脉高压(≥50mmHg),如房缺、室缺、动脉导管未闭等"},
            {"73",@"1.1.4各类心肌病"},
            {"74",@"1.1.6急性心肌炎"},
            {"75",@"1.1.2复杂先心(法洛氏四联症、艾森曼格综合征等)和未手术的紫绀型心脏病(SpO2&lt;90%)；Fontan循环术后"},
            {"76",@"1.1.3心脏瓣膜病:瓣膜置换术后,中重度二尖瓣狭窄(瓣口&lt;1.5cm2),主动脉瓣狭窄(跨瓣压差≥50mmHg)、 马凡氏综合征等"},
            {"77",@"1.1.5感染性心内膜炎"},
            {"78",@"1.1.7风心病风湿活动期"},
            {"79",@"1.1.9其他严重心血管疾病"},
            {"80",@"2.2呼吸系统疾病:经呼吸内科诊治无需药物治疗、肺功能正常"},
            {"81",@"2.2.1哮喘"},
            {"82",@"2.2.1.1哮喘反复发作不伴肺功能不全"},
            {"83",@"2.2.1.2哮喘反复发作伴肺功能不全"},
            {"84",@"1.2呼吸系统疾病:哮喘反复发作、肺纤维化、胸廓或脊柱严重畸形等影响肺功能者"},
            {"85",@"2.2.2脊柱側弯"},
            {"86",@"2.2.3胸廓畸形等伴轻度肺功能不全"},
            {"87",@"2.3消化系统疾病:肝炎病毒携带(表面抗原阳性、肝功能正常)"},
            {"88",@"2.3.1原因不明的肝功能异常."},
            {"89",@"2.3.2仅需要药物治疗的肝硬化、肠梗阻、消化道出血等"},
            {"90",@"1.3消化系统疾病:重型肝炎、肝硬化失代偿、严重消化道出血、急性胰腺炎、肠梗阻等影响孕产妇生命的疾病"},
            {"91",@"肾脏疾病(目前病情稳定肾功能正常)"},
            {"92",@"慢性肾脏疾病伴肾功能不全代偿期(肌酐超过正常值上限)"},
            {"93",@"急、慢性肾脏疾病伴高血压、肾功能不全(肌酐超过正常值上限的1.5倍)"},
            {"94",@"2.5无需药物治疗的糖尿病、甲状腺疾病、垂体泌乳素瘤等"},
            {"95",@"2.5.1需药物治疗的糖尿病、甲状腺疾病、垂体泌乳素瘤"},
            {"96",@"1.5.1糖尿病并发肾病V级、严重心血管病、增生性视网膜病变或玻璃体出血、周围神经病变等"},
            {"97",@"1.5.2甲状腺功能亢进并发心脏病、感染、肝功能异常、精神异常等疾病"},
            {"98",@"1.5.3甲状腺功能减退引起相应系统功能障碍,基础代谢率小于-50%"},
            {"99",@"1.5.4垂体泌乳素瘤出现视力减退、视野缺损、偏盲等压迫症状"},
            {"100",@"2.5.2肾性尿崩症(尿量超过4000m1/日)等"},
            {"101",@"1.5.5尿崩症:中枢性尿崩症伴有明显的多饮、烦渴、多尿症状,或合并有其他垂体功能异常"},
            {"102",@"1.5 6嗜铬细胞瘤等"},
            {"103",@"2.6.1妊娠合并血小板减少(PLT 50-100×109/L)但无出血倾向"},
            {"104",@"2.6.1血小板减少(PLT30-50×109/L)"},
            {"105",@"1.6.2血小板减少(＜30×109/L)或进行性下降或伴有出血倾向"},
            {"106",@"2.6.2妊娠合并贫血(Hb 60-110g/L)"},
            {"107",@"2.6.2重度贫血(Hb40-60g/L)"},
            {"108",@"1.6.3重度贫血(Hb≤40g/L)"},
            {"109",@"2.6.3凝血功能障碍无出血倾向"},
            {"110",@"1.6.5凝血功能障碍伴有出血倾向(如先天性凝血因子缺乏、低纤维蛋白原血症等)"},
            {"111",@"1.6.1再生障碍性贫血"},
            {"112",@"1.6.4白血病"},
            {"113",@"2.6.4易栓症(如抗疑血酶缺陷症、蛋白C缺陷症、蛋白s缺陷症、抗磷脂综合征、肾病综合征等)"},
            {"114",@"1.6.6血栓栓塞性疾病(如下肢深静脉血检、颅内静脉窦血栓等)"},
            {"115",@"2.8免疫系统疾病:无需药物治疗(如系统性红斑狼疮、IgA肾病、类风湿性关节炎、干燥综合征、未分化结缔组织病等)"},
            {"116",@"2.7免疫系统疾病:应用小剂量激素(如强的松5-10mg/天) 6月以上,无临床活动表现(如系统性红班狼疮、重症 IgA肾病、类风湿性关节炎、干燥综合征、未分化结缔组织病等)"},
            {"117",@"1.7免疫系统疾病活动期,如系统性红班狼疮(SLE)、重症 IgA肾病、类风湿性关节炎、干燥综合征、未分化结缔组织病等"},
            {"118",@"2.8恶性肿瘤治疗后无转移无复发"},
            {"119",@"1.9.1妊娠期间发现的恶性肿瘤"},
            {"120",@"1.9.2治疗后复发或发生远处转移"},
            {"121",@"2.9智力障碍"},
            {"122",@"2.10精神病缓解期"},
            {"123",@"1.8精神病急性期"},
            {"124",@"2.7.1癫痫(单纯部分性发作和复杂部分性发作)"},
            {"125",@"2.11.1癫痫(失神发作)"},
            {"126",@"1.10.2癫痫全身发作"},
            {"127",@"2.7.2重症肌无力(眼肌型)等"},
            {"128",@"2.11.2重症肌无力(病变波及四肢骨骼肌和延脑部肌肉)等"},
            {"129",@"1.10.3重症肌无力(病变发展至延脑肌、肢带肌、躯干肌和呼吸肌)"},
            {"130",@"1.10.1脑血管畸形及手术史"},
            {"131",@"2.9尖锐湿疣、淋病等性传播疾病"},
            {"132",@"3.1病毒性肝炎"},
            {"133",@"3.2梅毒"},
            {"134",@"3.3结核病"},
            {"135",@"3.4HIV感染及艾滋病"},
            {"136",@"3.5重症感染性肺炎"},
            {"137",@"3.6特珠病毒感染(H1N7、寨卡等）"},
            {"138",@"3.7其他妊娠合并传染性疾病"},
            {"139",@"2.10吸毒史"},
            {"140",@"1.11吸毒"},
            {"141",@"2.11其他内外、科疾病等"},
            {"142",@"2.12其他较严重内、外科疾病等"},
            {"143",@"1.12其他严重内、外科疾病等"},
            {"521",@"边缘性前置胎盘"},
            {"522",@"中央性前置胎盘"},
        };

        public readonly static List<HighRisksMapper> HighRisksMapper = new List<HighRisksMapper>()
        {
            //Hele高危  FS高危
            new HighRisksMapper() { HLId ="a160101",FSId= "2"},
            new HighRisksMapper() { HLId ="a160102",FSId= "48"},
            new HighRisksMapper() { HLId ="a160104",FSId= "10"},
            new HighRisksMapper() { HLId ="a160104",FSId= "11"},
            new HighRisksMapper() { HLId ="a160104",FSId= "12"},
            new HighRisksMapper() { HLId ="a160104",FSId= "13"},
            new HighRisksMapper() { HLId ="a160104",FSId= "14"},
            new HighRisksMapper() { HLId ="a160104",FSId= "9"},
            new HighRisksMapper() { HLId ="a160105",FSId= "4"},
            new HighRisksMapper() { HLId ="a160105",FSId= "5"},
            new HighRisksMapper() { HLId ="a160106",FSId= "15"},
            new HighRisksMapper() { HLId ="a160106",FSId= "16"},
            new HighRisksMapper() { HLId ="a160107",FSId= "18"},
            new HighRisksMapper() { HLId ="a160108",FSId= "7"},
            new HighRisksMapper() { HLId ="a160109",FSId= "8"},
            new HighRisksMapper() { HLId ="a160110",FSId= "17"},
            new HighRisksMapper() { HLId ="a170102",FSId= "31"},
            new HighRisksMapper() { HLId ="a170201",FSId= "32"},
            new HighRisksMapper() { HLId ="a170301",FSId= "45"},
            new HighRisksMapper() { HLId ="a170501",FSId= "43"},
            new HighRisksMapper() { HLId ="a170502",FSId= "42"},
            new HighRisksMapper() { HLId ="a170702",FSId= "39"},
            new HighRisksMapper() { HLId ="a170703",FSId= "44"},
            new HighRisksMapper() { HLId ="a170704",FSId= "46"},
            new HighRisksMapper() { HLId ="a170705",FSId= "55"},
            new HighRisksMapper() { HLId ="a170801",FSId= "87"},
            new HighRisksMapper() { HLId ="a170802",FSId= "38"},
            new HighRisksMapper() { HLId ="a170901",FSId= "103"},
            new HighRisksMapper() { HLId ="a170902",FSId= "106"},
            new HighRisksMapper() { HLId ="a171001",FSId= "60"},
            new HighRisksMapper() { HLId ="a171001",FSId= "61"},
            new HighRisksMapper() { HLId ="a171002",FSId= "62"},
            new HighRisksMapper() { HLId ="a171003",FSId= "64"},
            new HighRisksMapper() { HLId ="a171004",FSId= "63"},
            new HighRisksMapper() { HLId ="a180101",FSId= "80"},
            new HighRisksMapper() { HLId ="a180201",FSId= "91"},
            new HighRisksMapper() { HLId ="a180301",FSId= "94"},
            new HighRisksMapper() { HLId ="a180401",FSId= "115"},
            new HighRisksMapper() { HLId ="a180501",FSId= "124"},
            new HighRisksMapper() { HLId ="a180801",FSId= "131"},
            new HighRisksMapper() { HLId ="a180802",FSId= "139"},
            new HighRisksMapper() { HLId ="b160101",FSId= "3"},
            new HighRisksMapper() { HLId ="b160104",FSId= "51"},
            new HighRisksMapper() { HLId ="b160105",FSId= "6"},
            new HighRisksMapper() { HLId ="b170106",FSId= "35"},
            new HighRisksMapper() { HLId ="b170107",FSId= "50"},
            new HighRisksMapper() { HLId ="b170601",FSId= "47"},
            new HighRisksMapper() { HLId ="b170702",FSId= "40"},
            new HighRisksMapper() { HLId ="b170705",FSId= "55"},
            new HighRisksMapper() { HLId ="b170706",FSId= "56"},
            new HighRisksMapper() { HLId ="b170706",FSId= "57"},
            new HighRisksMapper() { HLId ="b170706",FSId= "58"},
            new HighRisksMapper() { HLId ="b170801",FSId= "89"},
            new HighRisksMapper() { HLId ="b170802",FSId= "88"},
            new HighRisksMapper() { HLId ="b170901",FSId= "104"},
            new HighRisksMapper() { HLId ="b170902",FSId= "107"},
            new HighRisksMapper() { HLId ="b170903",FSId= "109"},
            new HighRisksMapper() { HLId ="b170904",FSId= "113"},
            new HighRisksMapper() { HLId ="b171001",FSId= "68"},
            new HighRisksMapper() { HLId ="b171002",FSId= "67"},
            new HighRisksMapper() { HLId ="b171003",FSId= "69"},
            new HighRisksMapper() { HLId ="b171004",FSId= "70"},
            new HighRisksMapper() { HLId ="b171005",FSId= "66"},
            new HighRisksMapper() { HLId ="b171006",FSId= "65"},
            new HighRisksMapper() { HLId ="b180101",FSId= "81"},
            new HighRisksMapper() { HLId ="b180102",FSId= "85"},
            new HighRisksMapper() { HLId ="b180103",FSId= "86"},
            new HighRisksMapper() { HLId ="b180201",FSId= "92"},
            new HighRisksMapper() { HLId ="b180301",FSId= "95"},
            new HighRisksMapper() { HLId ="b180302",FSId= "100"},
            new HighRisksMapper() { HLId ="b180401",FSId= "116"},
            new HighRisksMapper() { HLId ="b180501",FSId= "125"},
            new HighRisksMapper() { HLId ="b180701",FSId= "118"},
            new HighRisksMapper() { HLId ="b180801",FSId= "121"},
            new HighRisksMapper() { HLId ="b180802",FSId= "122"},
            new HighRisksMapper() { HLId ="c170101",FSId= "105"},
            new HighRisksMapper() { HLId ="c170101",FSId= "26"},
            new HighRisksMapper() { HLId ="c170101",FSId= "75"},
            new HighRisksMapper() { HLId ="c170101",FSId= "90"},
            new HighRisksMapper() { HLId ="c170102",FSId= "108"},
            new HighRisksMapper() { HLId ="c170102",FSId= "40"},
            new HighRisksMapper() { HLId ="c170102",FSId= "72"},
            new HighRisksMapper() { HLId ="c170103",FSId= "110"},
            new HighRisksMapper() { HLId ="c170103",FSId= "76"},
            new HighRisksMapper() { HLId ="c170104",FSId= "114"},
            new HighRisksMapper() { HLId ="c170104",FSId= "73"},
            new HighRisksMapper() { HLId ="c170105",FSId= "111"},
            new HighRisksMapper() { HLId ="c170105",FSId= "77"},
            new HighRisksMapper() { HLId ="c170106",FSId= "112"},
            new HighRisksMapper() { HLId ="c170106",FSId= "59"},
            new HighRisksMapper() { HLId ="c170106",FSId= "74"},
            new HighRisksMapper() { HLId ="c170107",FSId= "52"},
            new HighRisksMapper() { HLId ="c170107",FSId= "78"},
            new HighRisksMapper() { HLId ="c170108",FSId= "37"},
            new HighRisksMapper() { HLId ="c170108",FSId= "53"},
            new HighRisksMapper() { HLId ="c170705",FSId= "55"},
            new HighRisksMapper() { HLId ="c180101",FSId= "84"},
            new HighRisksMapper() { HLId ="c180201",FSId= "93"},
            new HighRisksMapper() { HLId ="c180301",FSId= "79"},
            new HighRisksMapper() { HLId ="c180301",FSId= "96"},
            new HighRisksMapper() { HLId ="c180302",FSId= "97"},
            new HighRisksMapper() { HLId ="c180303",FSId= "98"},
            new HighRisksMapper() { HLId ="c180304",FSId= "99"},
            new HighRisksMapper() { HLId ="c180305",FSId= "102"},
            new HighRisksMapper() { HLId ="c180306",FSId= "101"},
            new HighRisksMapper() { HLId ="c180401",FSId= "117"},
            new HighRisksMapper() { HLId ="c180501",FSId= "130"},
            new HighRisksMapper() { HLId ="c180502",FSId= "126"},
            new HighRisksMapper() { HLId ="c180503",FSId= "129"},
            new HighRisksMapper() { HLId ="c180701",FSId= "119"},
            new HighRisksMapper() { HLId ="c180702",FSId= "120"},
            new HighRisksMapper() { HLId ="c180801",FSId= "140"},
            new HighRisksMapper() { HLId ="c180802",FSId= "143"},
            new HighRisksMapper() { HLId ="c180803",FSId= "123"},
            new HighRisksMapper() { HLId ="z160101",FSId= "132"},
            new HighRisksMapper() { HLId ="z160102",FSId= "133"},
            new HighRisksMapper() { HLId ="z160103",FSId= "135"},
            new HighRisksMapper() { HLId ="z160104",FSId= "134"},
            new HighRisksMapper() { HLId ="z160105",FSId= "136"},
            new HighRisksMapper() { HLId ="z160106",FSId= "137"},
            new HighRisksMapper() { HLId ="a160101",FSId= "1"},

            //修正用匹配扩展
            //BMI
            new HighRisksMapper() { HLId ="4",FSId= "4"},//<18.5
            new HighRisksMapper() { HLId ="6",FSId= "6"},//>=28
            new HighRisksMapper() { HLId ="5",FSId= "5"},//>>25
            //age
            new HighRisksMapper() { HLId ="1",FSId= "1"},// <= 18岁
            new HighRisksMapper() { HLId ="2",FSId= "2"},// >  35岁
            new HighRisksMapper() { HLId ="3",FSId= "3"},// >= 40岁
        };
        
        //注意特殊值有
        //{"11" ,"博士"},
        //{"12" ,"硕士"},
        //注意特殊值有
        //{"91" ,"未分类"},
        /// <summary>
        /// return "" when not contains key
        /// return matchedKey when matched
        /// return "" when not matched
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> GetHighRisks_By_HighRisks_Hele(string key)
        {
            if (string.IsNullOrEmpty(key))
                return new List<string>();
            return HighRisksMapper.Where(c => c.HLId == key).Select(c => c.FSId).ToList();
        }
    }
}
