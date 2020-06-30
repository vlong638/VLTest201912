using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FrameworkTest.Business.SDMockCommit
{

    [Table("[PregnantInfo]")]
    public class PregnantInfo
    {

        public PregnantInfo()
        {
        }
        public PregnantInfo(string iDCard, string name, string phoneNumber)
        {
            idcard = iDCard;
            personname = name;
            mobilenumber = phoneNumber;
        }

        public int Id { set; get; }

        //保健号 HEALTH_CODE pregnantbookid 上传时为空，接口成功返回更新
        public string pregnantbookid { set; get; }
        //姓名  MOTHER_NAME personname
        public string personname { set; get; }
        //出生日期 BIRTH_DATE  birthday 为空的话根据合法身份证号计算
        public DateTime? birthday { set; get; }
        //建册年龄
        public string createage;
        //产后休养地址
        public string restregioncode;
        public string restregiontext;
        //丈夫建册年龄
        public string husbandage;
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
        //户籍地址详细地址    MOTHER_DOOR_NUM 需核对
        public string homeaddress_text { set; get; }
        //现住址省 NOW_PROVINCE    liveplace
        //现住址市    NOW_CITY liveplace
        //现住址区（县）	NOW_REGION liveplace
        //现住址街道（乡）	NOW_STREET liveplace
        //现住址村 NOW_VILLAGE liveplace
        //现住址详细地址 NOW_DOOR_NUM liveplace
        public string liveplace { set; get; }
        public string liveplace_text;

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
        public DateTime? husbandbirthday { set; get; }
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
        public string husbandliveaddresstext;

        //是否是农籍
        public string isagrregister;
        //婚姻状况
        public string maritalstatuscode;
        /// <summary>
        /// 妇科手术史
        /// </summary>
        internal string gynecologyops;
        /// <summary>
        /// 手术史
        /// </summary>
        internal string operationhistory;
        /// <summary>
        /// 末次月经时间
        /// </summary>
        internal DateTime? lastmenstrualperiod;
        /// <summary>
        /// 预产期
        /// </summary>
        internal DateTime? dateofprenatal;
        /// <summary>
        /// 本次怀孕方式
        /// </summary>
        internal string tpregnancymanner;
        /// <summary>
        /// 既往病史
        /// </summary>
        internal string pasthistory;
        /// <summary>
        /// 过敏史
        /// </summary>
        internal string allergichistory;
        /// <summary>
        /// 输血史
        /// </summary>
        internal string bloodtransfution;
        /// <summary>
        /// 家族史
        /// </summary>
        internal string familyhistory;
        /// <summary>
        /// 初潮年龄
        /// </summary>
        internal string menarcheage;
        /// <summary>
        /// 经期下限
        /// </summary>
        internal string menstrualperiodmin;
        /// <summary>
        /// 经期上限
        /// </summary>
        internal string menstrualperiodmax;
        /// <summary>
        /// 周期下限
        /// </summary>
        internal string cyclemin;
        /// <summary>
        /// 周期上限
        /// </summary>
        internal string cyclemax;
        /// <summary>
        /// 月经量
        /// </summary>
        internal menstrualblood menstrualblood;
        /// <summary>
        /// 痛经
        /// </summary>
        internal string dysmenorrhea;
        /// <summary>
        /// 毒物接触史
        /// </summary>
        internal string poisontouchhis;
        /// <summary>
        /// 植入时间
        /// </summary>
        internal DateTime? implanttime;
        /// <summary>
        /// 取卵时间
        /// </summary>
        internal DateTime? eggretrievaltime;
        /// <summary>
        /// 遗传家族史
        /// </summary>
        internal string heredityfamilyhistory;
        /// <summary>
        /// 生育史
        /// </summary>
        internal string pregnanthistory;
        /// <summary>
        /// 孕次
        /// </summary>
        internal string gravidity;
        /// <summary>
        /// 阴道分娩
        /// </summary>
        internal string vaginaldeliverynum;
        /// <summary>
        /// 体重
        /// </summary>
        internal string weight;
        /// <summary>
        /// 身高
        /// </summary>
        internal string height;
        /// <summary>
        /// BMI
        /// </summary>
        internal string bmi;

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

    public enum menstrualblood
    {
        [Description("")]
        None = 0,
        [Description("多")]
        plenty = 1,
        [Description("中")]
        medium =2,
        [Description("少")]
        few =3,
    }

    //[{ "index":"0", "pregstatus":"人流", "babysex":"0", "babyweight":"", "pregnantage":"2017年6月" },{ "index":"2","pregstatus":"","babysex":"","babyweight":"","pregnantage":""}]
    public class pregnanthistory
    {
        public string index { set; get; }
        public string pregstatus { set; get; }
        public string babysex { set; get; }
        public string babyweight { set; get; }
        public string pregnantage { set; get; }


        public List<string> Pregstatuss
        {
            get
            {
                if (pregstatuss == null)
                {
                    pregstatuss = new List<string>();
                    if (!string.IsNullOrEmpty(pregstatus))
                    {
                        pregstatuss.AddRange(pregstatus.Split(','));
                    }
                }
                return pregstatuss;
            }
        }
        private List<string> pregstatuss;
    }
}
