using Dapper;
using Dapper.Contrib.Extensions;
using FrameworkTest.Business.TaskScheduler;
using FrameworkTest.Common.ConfigSolution;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.HttpSolution;
using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FS.SyncTask
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        static VLScheduler Scheduler;
        static string ConnectingString = "";
        static string TargetService = "";
        static Dictionary<int, Func<string>> Works;

        private void Form1_Load(object sender, EventArgs e)
        {
            //验证顺德的转码
            //var decode = System.Web.HttpUtility.UrlDecode(@"%5B%7B%22D2%22%3A%224406000000000035%22%2C%22D57%22%3A%22%22%2C%22D70%22%3A%22%22%2C%22D71%22%3A%22%22%2C%22D72%22%3A%22%22%2C%22D1%22%3A%2200000035%22%2C%22D3%22%3A%22%E6%B5%8B%E8%AF%95%22%2C%22D4%22%3A%22CN%22%2C%22D5%22%3A%2201%22%2C%22D6%22%3A%2202%22%2C%22D7%22%3A%2212345678998798%22%2C%22D8%22%3A%221990-01-01%22%2C%22curdate1%22%3A%22%22%2C%22D9%22%3A%2232%22%2C%22D10%22%3A%222%22%2C%22D11%22%3A%2213211111111%22%2C%22D12%22%3A%222%22%2C%22D69%22%3A%22%E4%BD%9B%E5%B1%B1%E5%B8%82%E5%A6%87%E5%B9%BC%E4%BF%9D%E5%81%A5%E9%99%A2%22%2C%22D13%22%3A%22%E5%8D%95%E4%BD%8D%22%2C%22D14%22%3A%22%22%2C%22D15%22%3A%2244%22%2C%22D16%22%3A%224419%22%2C%22D17%22%3A%22441901%22%2C%22D18%22%3A%22%22%2C%22D19%22%3A%22%22%2C%22D20%22%3A%22%E5%B9%BF%E4%B8%9C%E7%9C%81%E4%B8%9C%E8%8E%9E%E5%B8%82%E5%B8%82%E7%9B%B4%E8%BE%96%E4%B9%A1%E4%B8%9C%E5%B9%B3%E7%A4%BE%E5%8C%BA%E5%B1%85%E5%A7%94%E4%BC%9A%22%2C%22D21%22%3A%2244%22%2C%22D22%22%3A%224406%22%2C%22D23%22%3A%22440604%22%2C%22D24%22%3A%22440604009%22%2C%22D25%22%3A%22440604009025%22%2C%22D26%22%3A%22%E5%B9%BF%E4%B8%9C%E7%9C%81%E4%BD%9B%E5%B1%B1%E5%B8%82%E7%A6%85%E5%9F%8E%E5%8C%BA%E7%9F%B3%E6%B9%BE%E9%95%87%E8%A1%97%E9%81%93%E4%B8%9C%E5%B9%B3%E7%A4%BE%E5%8C%BA%E5%B1%85%E5%A7%94%E4%BC%9A%22%2C%22D27%22%3A%2244%22%2C%22D28%22%3A%224401%22%2C%22D29%22%3A%22440114%22%2C%22D30%22%3A%22%22%2C%22D31%22%3A%22%22%2C%22D32%22%3A%22%E5%B9%BF%E4%B8%9C%E7%9C%81%E5%B9%BF%E5%B7%9E%E5%B8%82%E8%8A%B1%E9%83%BD%E5%8C%BA%22%2C%22D33%22%3A%221%22%2C%22D34%22%3A%222%22%2C%22D35%22%3A%22%22%2C%22D36%22%3A%221%22%2C%22D37%22%3A%22%22%2C%22D38%22%3A%22%22%2C%22D62%22%3A%22%22%2C%22D63%22%3A%22%22%2C%22D64%22%3A%222%22%2C%22D65%22%3A%221%22%2C%22D66%22%3A%221%22%2C%22D67%22%3A%221%22%2C%22D68%22%3A%224%22%2C%22D39%22%3A%22%E8%AF%B7%E9%97%AE%22%2C%22D40%22%3A%22CN%22%2C%22D41%22%3A%2201%22%2C%22D42%22%3A%2204%22%2C%22D43%22%3A%221111111111%22%2C%22D44%22%3A%221990-01-01%22%2C%22D45%22%3A%2230%22%2C%22D46%22%3A%22%22%2C%22D47%22%3A%22%E5%B9%BF%E4%B8%9C%22%2C%22D48%22%3A%221322222222%22%2C%22D49%22%3A%22%22%2C%22D50%22%3A%22%22%2C%22D51%22%3A%2244%22%2C%22D52%22%3A%224406%22%2C%22D53%22%3A%22440605%22%2C%22D54%22%3A%22440605124%22%2C%22D55%22%3A%22%22%2C%22D56%22%3A%22%E5%B9%BF%E4%B8%9C%E7%9C%81%E4%BD%9B%E5%B1%B1%E5%B8%82%E5%8D%97%E6%B5%B7%E5%8C%BA%E7%8B%AE%E5%B1%B1%E9%95%87%E6%B2%99%E7%A4%BE%E5%8C%BA%E5%B1%85%E5%A7%94%E4%BC%9A%22%2C%22D58%22%3A%222020-01-10%22%2C%22D59%22%3A%22440023366%22%2C%22D60%22%3A%22%E9%83%AD%E6%99%93%E7%8E%B2%22%2C%22D61%22%3A%22%22%7D%5D");
            //var encode = System.Web.HttpUtility.UrlEncode(decode);

            ConnectingString = ConfigHelper.GetAppConfig(nameof(ConnectingString));
            TargetService = ConfigHelper.GetAppConfig(nameof(TargetService));
            Works = new Dictionary<int, Func<string>>()
            {
                {
                    1,()=>{
                        CookieContainer container =new CookieContainer();
                        var context = DBHelper.GetSqlDbContext(ConnectingString);
                        var serviceResult = context.DelegateTransaction((group) =>
                        {
                            List<PregnantInfo> pregnantInfos = GetPregnantInfo(group);
                            foreach (var pregnantInfo in pregnantInfos)
                            {
                                var url=$@"http://{TargetService}/FSFY/disPatchJsonOut?&clazz=SENDJBXX&encode=0";
                                var postData = new Target_BaseInfo(pregnantInfo).ToJson();
                                var result = HttpHelper.Post(url,postData ,ref container);
                                var message = result?.Substring(0,result.Length>500? 500:result.Length);
                                var syncForFS=new SyncForFS()
                                {
                                    TargetType =TargetType.PregnantInfo,
                                    SourceId = pregnantInfo.Id.ToString(),
                                    SyncTime=DateTime.Now,
                                    ErrorMessage = message,
                                    HasError =!string.IsNullOrEmpty(message),
                                };

                                var id = group.Connection.Insert(syncForFS,transaction: group.Transaction);
                            }

                            return $"执行同步-`基本信息`,同步数量:{pregnantInfos.Count()}";
                        });
                        return serviceResult.Data;
                    }
                }
            };
            Scheduler = new VLScheduler();
            Scheduler.DoLogEvent += SetText;
            Scheduler.UpdateConfigEvent += SetDataGrid;
            Scheduler.Start(Works);
            dgv_task.DataSource = VLScheduler.TaskConfigs;
        }

        delegate void SetTextCallBack(string text);
        private void SetText(string text)
        {
            if (this.listBox1.InvokeRequired)
            {
                SetTextCallBack stcb = new SetTextCallBack(SetText);
                this.Invoke(stcb, new object[] { text });
            }
            else
            {
                if (listBox1.Items.Count > 50)
                {
                    listBox1.Items.Clear();
                }
                listBox1.Items.Add(text);
            }
        }

        delegate void SetDataGridCallBack(List<TaskConfig> configs);
        private void SetDataGrid(List<TaskConfig> configs)
        {
            if (this.dgv_task.InvokeRequired)
            {
                SetDataGridCallBack stcb = new SetDataGridCallBack(SetDataGrid);
                this.Invoke(stcb, new object[] { configs });
            }
            else
            {
                dgv_task.DataSource = VLScheduler.TaskConfigs;
                dgv_task.Refresh();
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
        }


        private static List<PregnantInfo> GetPregnantInfo(DbGroup group)
        {
            return new List<PregnantInfo>() {
                new PregnantInfo(){
                    Id=0,
                    idcard ="12345678998798",
                    birthday=new DateTime(2000,1,1,1,1,1),
                    educationcode="educationcode",
                    homeaddress = "homeaddress",
                    husbandbirthday =new DateTime(2000,1,2,1,1,1),
                    husbandeducationcode = "husbandeducationcode",
                    husbandidcard = "husbandidcard",
                    husbandidtype = "husbandidtype",
                    husbandliveaddresscode = "husbandliveaddresscode",
                    husbandmobile = "husbandmobile",
                    husbandname = "husbandname",
                    husbandnationalitycode = "husbandnationalitycode",
                    husbandnationcode = "husbandnationcode",
                    idtype = "idtype",
                    liveplace = "liveplace",
                    mobilenumber = "mobilenumber",
                    nationalitycode = "nationalitycode",
                    nationcode = "nationcode",
                    personname = "personname",
                    pregnantbookid = "pregnantbookid",
                    registrationtype = "registrationtype",
                    workcode = "workcode",
                    workname = "workname",
                    workplace = "workplace",
                    zipcode = "zipcode",
                }
            };

            return group.Connection.Query<PregnantInfo>(@"
select Top 1 s.id sid,pi.* from PregnantInfo pi
left join SyncForFS s on s.TargetType = 1 and s.SourceId = pi.Id
where s.id is null ", transaction: group.Transaction).ToList();

        }
    }
    public class PregnantInfo
    {
        public int Id { set; get; }

        //保健号 HEALTH_CODE pregnantbookid 上传时为空，接口成功返回更新
        public string pregnantbookid { set; get; }
        //姓名  MOTHER_NAME personname
        public string personname { set; get; }
        //出生日期 BIRTH_DATE  birthday 为空的话根据合法身份证号计算
        public DateTime birthday { set; get; }
        //证件类型 ID_TYPE idtype
        public string idtype { set; get; }
        //证件号码    ID_NUM idcard
        public string idcard { set; get; }
        //国籍 NATIONALITY nationalitycode
        public string nationalitycode { set; get; }
        //民族  NATION nationcode
        public string nationcode { set; get; }
        //户口类型 ACCOUNT_TYPE    registrationtype
        public string registrationtype { set; get; }
        //职业  OCCUPATION workname/workcode
        public string workname { set; get; }
        public string workcode { set; get; }
        //TODO
        //文化程度    DEGREE1 educationcode
        public string educationcode { set; get; }
        //工作单位 COMPANY workplace
        public string workplace { set; get; }
        //户籍地址省   MOTHER_PROVINCE homeaddress
        //户籍地址市 MOTHER_CITY homeaddress
        //户籍地址区（县）	MOTHER_REGION homeaddress
        //户籍地址街道（乡）	MOTHER_STREET homeaddress
        //户籍地址村 MOTHER_VILLAGE  homeaddress
        public string homeaddress { set; get; }
        //TODO

        //户籍地址详细地址    MOTHER_DOOR_NUM 需核对
        //TODO


        //现住址省 NOW_PROVINCE    liveplace
        //现住址市    NOW_CITY liveplace
        //现住址区（县）	NOW_REGION liveplace
        //现住址街道（乡）	NOW_STREET liveplace
        //现住址村 NOW_VILLAGE liveplace
        //现住址详细地址 NOW_DOOR_NUM liveplace
        public string liveplace { set; get; }
        //TODO

        //邮编 POSTNUM zipcode
        public string zipcode { set; get; }
        //联系电话    TEL mobilenumber
        public string mobilenumber { set; get; }
        //丈夫姓名 NFQXM   husbandname
        public string husbandname { set; get; }
        //丈夫国籍    NFQGJ husbandnationalitycode
        public string husbandnationalitycode { set; get; }
        //丈夫民族 NFQMZ   husbandnationcode
        public string husbandnationcode { set; get; }
        //丈夫证件类型  NFQSFZJLX husbandidtype
        public string husbandidtype { set; get; }
        //丈夫证件号码 NFQSFZJH    husbandidcard
        public string husbandidcard { set; get; }
        //丈夫出生日期  NFQCSRQ husbandbirthday 为空的话根据合法身份证号计算
        public DateTime husbandbirthday { set; get; }
        //丈夫文化程度  NFQWHCD husbandeducationcode
        public string husbandeducationcode { set; get; }
        //丈夫电话 NFQDH   husbandmobile
        public string husbandmobile { set; get; }
        //丈夫现住址省  NFQXSHENG husbandliveaddresscode
        //丈夫现住址市 NFQXSHI husbandliveaddresscode
        //丈夫现住址区县 NFQXQUXIAN husbandliveaddresscode
        //丈夫现住址街镇 NFQXJIEZHEN husbandliveaddresscode
        //丈夫现住址村  NFQXCUN husbandliveaddresscode
        public string husbandliveaddresscode { set; get; }
        //TODO

        //丈夫现住址详细地址 NFQXXX  需核对
        //public string NFQXXX { set; get; }
        //TODO

        //推送人 SEND_PERSON 需核对 若与平台用户无关则对应editorname
        //public string NFQXXX { set; get; }
        //TODO

        //推送机构    SEND_UNIT	45608491-9	顺德妇保机构代码：45608491-9
        //public string NFQXXX { set; get; }
        //TODO

        //推送时间 SEND_DATE   当前时间

        //医院系统id  HIS_ID 需核对 顺德妇保机构代码：45608491-9
    }
    /// <summary>
    /// ([\w（）]+)\s+(\w+)\s(.+)
    /// </summary>
    public class Target_BaseInfo
    {
        #region Properties
        //保健号 
        //pregnantbookid 上传时为空，接口成功返回更新
        public string HEALTH_CODE { set; get; }
        //姓名 
        //personname
        public string MOTHER_NAME { set; get; }
        //出生日期 
        // birthday 为空的话根据合法身份证号计算
        public string BIRTH_DATE { set; get; }
        //证件类型 
        //idtype
        public string ID_TYPE { set; get; }
        //证件号码 
        //idcard
        public string ID_NUM { set; get; }
        //国籍 
        //nationalitycode
        public string NATIONALITY { set; get; }
        //民族 
        //nationcode
        public string NATION { set; get; }
        //户口类型 
        //   registrationtype
        public string ACCOUNT_TYPE { set; get; }
        //职业 
        //workname/workcode
        public string OCCUPATION { set; get; }
        //文化程度 
        //educationcode
        public string DEGREE1 { set; get; }
        //工作单位 
        //workplace
        public string COMPANY { set; get; }
        //户籍地址省 
        //homeaddress
        public string MOTHER_PROVINCE { set; get; }
        //户籍地址市 
        //homeaddress
        public string MOTHER_CITY { set; get; }
        //户籍地址区（县） 
        //homeaddress
        public string MOTHER_REGION { set; get; }
        //户籍地址街道（乡） 
        //homeaddress
        public string MOTHER_STREET { set; get; }
        //户籍地址村 
        // homeaddress
        public string MOTHER_VILLAGE { set; get; }
        //户籍地址详细地址 
        //需核对
        public string MOTHER_DOOR_NUM { set; get; }
        //现住址省 
        //   liveplace
        public string NOW_PROVINCE { set; get; }
        //现住址市 
        //liveplace
        public string NOW_CITY { set; get; }
        //现住址区（县） 
        //liveplace
        public string NOW_REGION { set; get; }
        //现住址街道（乡） 
        //liveplace
        public string NOW_STREET { set; get; }
        //现住址村 
        //liveplace
        public string NOW_VILLAGE { set; get; }
        //现住址详细地址 
        //liveplace
        public string NOW_DOOR_NUM { set; get; }
        //邮编 
        //zipcode
        public string POSTNUM { set; get; }
        //联系电话 
        //mobilenumber
        public string TEL { set; get; }
        //丈夫姓名 
        //  husbandname
        public string NFQXM { set; get; }
        //丈夫国籍 
        //husbandnationalitycode
        public string NFQGJ { set; get; }
        //丈夫民族 
        //  husbandnationcode
        public string NFQMZ { set; get; }
        //丈夫证件类型 
        //husbandidtype
        public string NFQSFZJLX { set; get; }
        //丈夫证件号码 
        //   husbandidcard
        public string NFQSFZJH { set; get; }
        //丈夫出生日期 
        //husbandbirthday 为空的话根据合法身份证号计算
        public string NFQCSRQ { set; get; }
        //丈夫文化程度 
        //husbandeducationcode
        public string NFQWHCD { set; get; }
        //丈夫电话 
        //  husbandmobile
        public string NFQDH { set; get; }
        //丈夫现住址省 
        //husbandliveaddresscode
        public string NFQXSHENG { set; get; }
        //丈夫现住址市 
        //husbandliveaddresscode
        public string NFQXSHI { set; get; }
        //丈夫现住址区县 
        //husbandliveaddresscode
        public string NFQXQUXIAN { set; get; }
        //丈夫现住址街镇 
        //husbandliveaddresscode
        public string NFQXJIEZHEN { set; get; }
        //丈夫现住址村 
        //husbandliveaddresscode
        public string NFQXCUN { set; get; }
        //丈夫现住址详细地址 
        // 需核对
        public string NFQXXX { set; get; }
        //推送人 
        //需核对 若与平台用户无关则对应editorname
        public string SEND_PERSON { set; get; }
        //推送机构 
        //45608491-9	顺德妇保机构代码：45608491-9
        public string SEND_UNIT { set; get; }
        //推送时间 
        //  当前时间
        public string SEND_DATE { set; get; }
        //医院系统id 
        //需核对 顺德妇保机构代码：45608491-9 
        public string HIS_ID { set; get; }
        #endregion

        public Target_BaseInfo(PregnantInfo p)
        {
            this.HEALTH_CODE = p.pregnantbookid;//
            this.MOTHER_NAME = p.personname;//
            //this.BIRTH_DATE = p.birthday;//
            this.ID_TYPE = p.idtype;//
            this.ID_NUM = p.idcard;//
            this.NATIONALITY = p.nationalitycode;//
            this.NATION = p.nationcode;//
            this.ACCOUNT_TYPE = p.registrationtype;//
            this.OCCUPATION = p.workname;// 
            this.DEGREE1 = p.educationcode;// 
            this.COMPANY = p.workplace;// 
            this.MOTHER_PROVINCE = p.homeaddress;//
            this.MOTHER_CITY = p.homeaddress;// 
            this.MOTHER_REGION = p.homeaddress;//
            this.MOTHER_STREET = p.homeaddress;//
            this.MOTHER_VILLAGE = p.homeaddress;//
            //this.MOTHER_DOOR_NUM = p.需核对;//
            this.NOW_PROVINCE = p.liveplace;//
            this.NOW_CITY = p.liveplace;//
            this.NOW_REGION = p.liveplace;//
            this.NOW_STREET = p.liveplace;//
            this.NOW_VILLAGE = p.liveplace;//
            this.NOW_DOOR_NUM = p.liveplace;//
            this.POSTNUM = p.zipcode;//
            this.TEL = p.mobilenumber;//
            this.NFQXM = p.husbandname;//
            this.NFQGJ = p.husbandnationalitycode;//
            this.NFQMZ = p.husbandnationcode;//
            this.NFQSFZJLX = p.husbandidtype;//
            this.NFQSFZJH = p.husbandidcard;//
            //this.NFQCSRQ = p.husbandbirthday;//
            this.NFQWHCD = p.husbandeducationcode;//
            this.NFQDH = p.husbandmobile;//
            this.NFQXSHENG = p.husbandliveaddresscode;//
            this.NFQXSHI = p.husbandliveaddresscode;//
            this.NFQXQUXIAN = p.husbandliveaddresscode;//
            this.NFQXJIEZHEN = p.husbandliveaddresscode;//
            this.NFQXCUN = p.husbandliveaddresscode;//
            //this.NFQXXX = p.需核对;//
            //this.SEND_PERSON = p.需核对;//
            //this.SEND_UNIT = p.45608491;// 
            //this.SEND_DATE = p.SEND_DATE;//  
            //this.HIS_ID = p.HIS_ID;// 需核对
        }
    }


}
