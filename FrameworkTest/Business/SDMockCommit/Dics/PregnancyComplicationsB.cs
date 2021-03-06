﻿using System.Collections.Generic;
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
        public readonly static Dictionary<string, string> PregnancyComplicationsB_FS = new Dictionary<string, string>()
        {
            {"1","无"},
            {"2","妊娠期糖尿病"},
            {"3","中重度贫血"},
            {"4","妊娠期高血压"},
            {"5","轻度子痫前期"},
            {"6","重度子痫前期"},
            {"7","慢性高血压合并子痫前期"},
            {"8","HELLP综合征"},
            {"9","胆汁淤积综合征"},
            {"10","前置胎盘"},
            {"11","胎盘早剥"},
            {"12","胎膜早破"},
            {"13","羊水过多"},
            {"14","羊水过少"},
            {"15","脐带脱垂"},
            {"16","滞产、软产道损伤"},
            {"17","产后出血"},
            {"18","感染"},
        };

        public readonly static Dictionary<string, string> PregnancyComplicationsB_SD = new Dictionary<string, string>()
        {
            //妊娠期糖尿病  
            {"O24.000","2"},
            {"O24.100","2"},
            {"O24.200","2"},
            {"O24.300","2"},
            {"O24.301","2"},
            {"O24.400","2"},
            {"O24.900","2"},
            //中重度贫血   
            {"O99.006","3"},
            {"O99.007","3"},
            //妊娠期高血压 
            {"O10.000","4"},
            {"O10.001","4"},
            {"O10.100","4"},
            {"O10.101","4"},
            {"O10.200","4"},
            {"O10.201","4"},
            {"O10.300","4"},
            {"O10.301","4"},
            {"O10.400","4"},
            {"O10.401","4"},
            {"O10.900","4"},
            {"O11.x00","4"},
            {"O11.x01","4"},
            //{"O11.x02","4"},
            {"O13.x00","4"},
            {"O13.x01","4"},
            {"O16.x00","4"},
            //轻度子痫前期  
            //{"O14.901","5"},
            //重度子痫前期  
            {"O14.100","6"},
            {"O14.000","6"},
            {"O14.900","6"},
            //{"O14.901","6"},
            {"O15.000","6"},
            {"O15.001","6"},
            {"O15.100","6"},
            {"O15.101","6"},
            {"O15.200","6"},
            {"O15.201","6"},
            {"O15.900","6"},
            //慢性高血压合并子痫前期 
            //{"O11.x02","7"},
            //HELLP综合征    
            {"O14.101","8"},
            //胆汁淤积综合征 
            {"O26.606","9"},
            //前置胎盘    
            {"O44.000","10"},
            {"O44.001","10"},
            {"O44.002","10"},
            {"O44.003","10"},
            {"O44.100","10"},
            {"O44.101","10"},
            {"O44.102","10"},
            {"O44.103","10"},
            //胎盘早剥    
            {"O45.000","11"},
            {"O45.001","11"},
            {"O45.800","11"},
            {"O45.900","11"},
            //胎膜早破    
            {"O42.000","12"},
            {"O42.100","12"},
            {"O42.200","12"},
            {"O42.900","12"},
            //羊水过多    
            {"O40.x00","13"},
            //羊水过少    
            {"O41.000","14"},
            //脐带脱垂    
            {"O69.000","15"},
            {"O69.001","15"},
            //滞产、软产道损伤	
            {"O63.900","16"},
            {"O71.301","16"},
            {"O71.402","16"},
            {"O71.701","16"},
            {"O71.703","16"},
            {"O71.704","16"},
            {"O71.705","16"},
            //产后出血   
            {"O72.100","17"},
            {"O72.200","17"},
            {"O72.202","17"},
            //感染	
            {"O03.000","18"},
            {"O03.001","18"},
            {"O03.002","18"},
            {"O03.500","18"},
            {"O03.501","18"},
            {"O03.502","18"},
            {"O03.503","18"},
            {"O03.504","18"},
            {"O04.000","18"},
            {"O04.001","18"},
            {"O04.500","18"},
            {"O04.502","18"},
            {"O04.503","18"},
            {"O05.000","18"},
            {"O05.500","18"},
            {"O06.000","18"},
            {"O06.500","18"},
            {"O07.000","18"},
            {"O07.500","18"},
            {"O08.000","18"},
            {"O08.003","18"},
            {"O08.004","18"},
            {"O08.006","18"},
            {"O23.000","18"},
            {"O23.100","18"},
            {"O23.200","18"},
            {"O23.300","18"},
            {"O23.400","18"},
            {"O23.500","18"},
            {"O23.900","18"},
            {"O23.901","18"},
            {"O41.100","18"},
            {"O75.300","18"},
            {"O86.000","18"},
            {"O86.001","18"},
            {"O86.002","18"},
            {"O86.100","18"},
            {"O86.200","18"},
            {"O86.201","18"},
            {"O86.300","18"},
            {"O86.402","18"},
            {"O86.800","18"},
            {"O91.000","18"},
            {"O91.001","18"},
            {"O98.300","18"},
            {"O98.503","18"},
            {"O98.505","18"},
            {"O98.807","18"},
            {"O98.808","18"},
            {"O99.506","18"},
            {"O99.510","18"},
		};

        public static string GetPregnancyComplicationsB(IEnumerable<Diagnosis> diagnosises)
        {
            HashSet<string> pregnancyComplications = new HashSet<string>();
            foreach (var diagnosis in diagnosises)
            {
                if (!PregnancyComplicationsB_SD.ContainsKey(diagnosis.diag_code))
                    continue;
                var fsCode = PregnancyComplicationsB_SD[diagnosis.diag_code];
                if (pregnancyComplications.Contains(fsCode))
                    continue;
                pregnancyComplications.Add(fsCode);
            }
            if (diagnosises.FirstOrDefault(c=>c.diag_code == "O11.x02")!=null )
            {
                pregnancyComplications.Add("4");
                pregnancyComplications.Add("7");
            }
            if (diagnosises.FirstOrDefault(c => c.diag_code == "O14.901") != null)
            {
                pregnancyComplications.Add("5");
                pregnancyComplications.Add("6");
            }
            
            if (pregnancyComplications.Count==0)
            {
                pregnancyComplications.Add("1");
            }
            return string.Join(",",pregnancyComplications);
        }
    }
}
