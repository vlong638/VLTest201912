using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebService.VLServer.Framework4
{
    /// <summary>
    /// SampleWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://microsoft.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class SampleWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public string HelloWorldStr(string name)
        {
            return "HelloWorld from:" + name;
        }

        [WebMethod]
        public void HelloWorldReturnJson(string name)
        {
            Context.Response.Charset = "UTF-8"; //设置字符集类型 
            Context.Response.ContentType = "application/json";
            Context.Response.Write(string.Format(@"{0}", name));
        }

        [WebMethod]
        public string HelloWorld(HelloRequest hello)
        {
            return "common 15:33";
        }

        [WebMethod]
        public string TransTest10kb(HelloRequest hello)
        {
            return @"
			set identity_insert fm_daichan on
	
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10229,5412,'2020-09-22','2020-09-22 08:35:00','','140','中等','20','8','-3',1,'100','2','','','内诊'
        ,'116','72','80','张美贤','','入室','','36.5','99',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10230,5412,'2020-09-22','2020-09-22 08:50:00','','142','中等','20','8','','','','','','','','','','','张美贤','','开始缩宫素8d/min静点','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10231,5412,'2020-09-22','2020-09-22 09:10:00','','144','中等','20','5','','','','','','','','','','','张美贤','','缩宫素调至16d/min，下床活动','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10243,5412,'2020-09-22','2020-09-22 09:40:00','','142','中等','20','3','','','','','','','','104','58','80','张美贤','','上床休息，吸氧','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10244,5412,'2020-09-22','2020-09-22 10:10:00','','142','中等','25','3','','','','','','','','','','','张美贤','','','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10245,5412,'2020-09-22','2020-09-22 10:40:00','','140','强','30','3','','','','','','','','','','','张美贤','','','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10251,5412,'2020-09-22','2020-09-22 11:10:00','','144','强','30','3','','','','','','','','115','77','78','张美贤','','自行排尿一次','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10255,5412,'2020-09-22','2020-09-22 11:35:00','','142','强','30','3','-3',1,'100','2','中 中','存','内诊','115','70','78','张美贤','','行分娩镇痛','','','99',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10256,5412,'2020-09-22','2020-09-22 11:50:00','','140','强','30','3','','','','','','','','','','','张美贤','','','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10257,5412,'2020-09-22','2020-09-22 12:05:00','','136','强','30','3','','','','','','','','120','70','78','张美贤','','胎心监护显示减速，报告范美玲主任，指示停止缩宫素静点，吸氧，左侧卧位','','36.9','99',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10258,5412,'2020-09-22','2020-09-22 12:20:00','','138','一般','15','5','','','','','','','','','','','张美贤','','','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10259,5412,'2020-09-22','2020-09-22 12:35:00','','134','一般','15','5','','','','','','','','','','','张美贤','','范美玲主任指示缩宫素调至20d/min','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10268,5412,'2020-09-22','2020-09-22 12:50:00','','140','强','30','3','','','','','','','','','','','张美贤','','出现减速，报告范美玲主任，停止缩宫素静点，吸氧，左侧卧位，快速补液','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10269,5412,'2020-09-22','2020-09-22 13:00:00','','146','强','30','3','-3',1,'100','2','','存','内诊','','','','张美贤','','范美玲主任会诊，内诊','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10316,5412,'2020-09-22','2020-09-22 13:30:00','','','','','','','','','','','','','','','','张美贤','','','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10317,5412,'2020-09-22','2020-09-22 11:40:00','','','','','','','','','','','','','','','','张美贤','','','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10318,5412,'2020-09-22','2020-09-22 14:30:00','','','','','','','','','','','','','','','','张美贤','','','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10319,5412,'2020-09-22','2020-09-22 15:00:00','','','','','','','','','','','','','','','','张美贤','','14点40分自然破水','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10322,5412,'2020-09-22','2020-09-22 15:30:00','','132','中等','30','3','-5',1,'100','3','中 中','破','内诊','','','','张美贤','','','','36.4','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10323,5412,'2020-09-22','2020-09-22 15:40:00','','128','中等','30','3','','','','','','','','','','','张美贤','','胎心监护图出现减速，遵医嘱停止缩宫素静点，给予面罩吸调整体位','','','',0,13);
INSERT INTO fm_daichan(create_time,update_time,[id], [chanfu_id], [daichan_date], [daichan_time], [tai_fangwei], [tai_xin], [gongsuo_qd], [chixu], [jiange], [xianlu], [FetalPresentation], [Capacity], [gongkou], [gongjing], [tai_mo], [jiancha_type], [xueya1], [xueya2], [maibo], [jiancha_ren], [yaowu], [qita], [yangshuixz], [tiwen], [xueyang], [status], [OperatorId]) values(GETDATE(),GETDATE(),10325,5412,'2020-09-22','2020-09-22 15:50:00','','136','中等','30','3','','','','','','','','','','','张美贤','','遵医嘱静点5%糖加维生素c','','','',0,13);
set identity_insert fm_daichan off
";
        }
    }

    public class HelloRequest
    {
        public string Name { set; get; }
    }
}
