using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VL.API.Common.Controllers;
using VL.API.PT.Entities;
using VL.API.PT.Services;

namespace VL.API.Controllers
{
    /// <summary>
    /// 产科
    /// </summary>
    public class PTController : V3ControllerBase
    {
        //for test 
        //https://localhost:44347/api/pt/GetPregnantInfoById?id=63816

        [HttpGet]
        public PregnantInfo GetPregnantInfoById([FromServices] PTService ptService, int id)
        {
            var entity = ptService.GetPregnantInfoById(id);
            return entity;
        } 

        #region for test
        //for test 
        //{"newitems":{"pregnanthistory":[{"index":"1","pregstatus":"无","babysex":"1","pregnantage":"2019.8","otherpregnanthistory":" "},{"index":"2","pregstatus":"足月产-健","babysex":"2","pregnantage":"2018.2","otherpregnanthistory":""}]},"olditems":{},"allitems_value":{"personname":"张晓玲","mobilenumber":"13566262593","patientaccount":"2012040000545393","phonenumber":"家庭电话家庭电话","idtype":"1","idcard":"330304199012189760","sexcode":"2","birthday":"1990-12-18","nationcode":"01","nationalitycode":"156","maritalstatuscode":"1","educationcode":"","workname":"办事人员和有关人员","workplace":"","bloodtypecode":"","rhbloodcode":"","ownerarea":"","registrationtype":"","isagrregister":"2","healthplace":"","restregioncode":"","restregiontext":"","homeaddress":"330100000000","homeaddress_text":"户籍详情户籍详情","liveplace":"330100000000","liveplace_text":"浙江省温州市瓯海区新桥街道站前路１９７号１１幢４０５室","iscreatebook":"2","createage":"29","pregnantbookid":"","createbookunit":"","husbandname":"33","husbandworkname":"","husbandworkplace":"","husbandfamilyhistorytext":"无","husbandidtype":"1","husbandidcard":"333333333333333","husbandbirthday":"1935-10-03","husbandage":"84","husbandhomeaddress":"12","husbandhomeaddress_text":"户籍详情户籍详情","husbandliveaddresscode":"11","husbandliveaddresstext":"居住地址详情居住地址详情","husbandmobile":"1111111111111","husbandbloodtypecode":"","husbandrhbloodcode":"","weight":"","height":"","bmi":"","menarcheage":"","menstrualperiodmin":"","menstrualperiodmax":"","cyclemin":"","cyclemax":"","menstrualblood":"","dysmenorrhea":"","sbp":"","dbp":"","pulse":"","lastmenstrualperiod":"2020-01-01","dateofprenatal":"2020-10-07","dateofprenatalmodifyreason":"","tpregnancymanner":"3","tpregnancymanner_text":"怀孕详情怀孕详情","eggretrievaltime":"2020-03-04","implanttime":"2020-03-05","gravidity":"1","parity":"0","vaginaldeliverynum":"2","caesareansectionnum":"3","bloodtransfution":"输血史输血史","personalhistory":"","gestationneuropathy":"无","pasthistory":"无","familyhistory":"父亲:无父亲疾病史 母亲:无母亲疾病史 兄弟姐妹:无兄弟姐妹疾病史 子女:无子女疾病史","operationhistory":"无手术史","gynecologyops":"妇科手术史妇科手术史","allergichistory":"有","poisontouchhis":"毒物接触史毒物接触史","heredityfamilyhistory":"无","create_localuser":"管理员","sourceunit":"330302014","editorname":"管理员","createdate":"2020-03-13","gestationalweeks":"10","gestationaldays":"2","changein_ascription":"","changein_unit":"","changein_date":"","id":"64116","editorcode":"DBA","patientid":"1429738"},"allitems_text":{"personname":"张晓玲","mobilenumber":"13566262593","patientaccount":"2012040000545393","phonenumber":"家庭电话家庭电话","idtype":"居民身份证","idcard":"330304199012189760","sexcode":"女","birthday":"1990-12-18","nationcode":"汉族","nationalitycode":"中国","maritalstatuscode":"未婚","educationcode":"","workname":"办事人员和有关人员","workplace":"","bloodtypecode":"","rhbloodcode":"","ownerarea":"","registrationtype":"","isagrregister":"否","healthplace":"","restregioncode":"","restregiontext":"","homeaddress":"浙江省杭州市","homeaddress_text":"户籍详情户籍详情","liveplace":"浙江省杭州市","liveplace_text":"浙江省温州市瓯海区新桥街道站前路１９７号１１幢４０５室","iscreatebook":"否","createage":"29","pregnantbookid":"","createbookunit":"","husbandname":"33","husbandworkname":"","husbandworkplace":"","husbandfamilyhistorytext":"无","husbandidtype":"居民身份证","husbandidcard":"333333333333333","husbandbirthday":"1935-10-03","husbandage":"84","husbandhomeaddress":"天津市","husbandhomeaddress_text":"户籍详情户籍详情","husbandliveaddresscode":"北京市","husbandliveaddresstext":"居住地址详情居住地址详情","husbandmobile":"1111111111111","husbandbloodtypecode":"","husbandrhbloodcode":"","weight":"","height":"","bmi":"","menarcheage":"","menstrualperiodmin":"","menstrualperiodmax":"","cyclemin":"","cyclemax":"","menstrualblood":"","dysmenorrhea":"","sbp":"","dbp":"","pulse":"","lastmenstrualperiod":"2020-01-01","dateofprenatal":"2020-10-07","dateofprenatalmodifyreason":"","tpregnancymanner":"试管婴儿","tpregnancymanner_text":"怀孕详情怀孕详情","eggretrievaltime":"2020-03-04","implanttime":"2020-03-05","gravidity":"1","parity":"0","vaginaldeliverynum":"2","caesareansectionnum":"3","pregstatus":"足月产-健","babysex":"女","pregnantage":"2018.2","otherpregnanthistory":"","bloodtransfution":"输血史输血史","personalhistory":"","gestationneuropathy":"无","pasthistory":"无","familyhistory":"父亲:无父亲疾病史 母亲:无母亲疾病史 兄弟姐妹:无兄弟姐妹疾病史 子女:无子女疾病史","operationhistory":"无手术史","gynecologyops":"妇科手术史妇科手术史","allergichistory":"有","poisontouchhis":"毒物接触史毒物接触史","heredityfamilyhistory":"无","create_localuser":"管理员","sourceunit":"330302014","editorname":"管理员","createdate":"2020-03-13","gestationalweeks":"10","gestationaldays":"2","changein_ascription":"","changein_unit":"","changein_date":"","id":"64116","editorcode":"DBA","patientid":"1429738"},"id":64116,"reqitems":[],"jobnumber":"DBA","doctorname":"管理员","mechanism":"107","departmentcode":"107","departmentname":"产科","bingrenid":"1429738","shenfenzh":"330304199012189760","bingrenxm":"张晓玲","jiuzhenid":"1000189789","jigoudm":"330302014","userid":"DBA","username":"管理员","timestamp":"20200327153716"} 
        #endregion

        [HttpPost]
        public bool SavePregnantInfo([FromServices] PTService ptService, [FromForm] string input)
        {
            input = System.Web.HttpUtility.UrlDecode(input, System.Text.Encoding.GetEncoding("UTF-8"));
            Dictionary<string, object> inputs = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(input);
            //参数转换
            //VLCore:关键原则
            //如果有一些辅助性的逻辑,认为用户填写了A,B项同时可以解释C,D的录入
            //录入的辅助在此处理,可以考虑构建Factory进行统一
            //特殊情况:有些复杂逻辑在业务内进行业务组织得以进行,针对这些,将其规划为领域内的业务逻辑
            var pregnant = new PregnantInfo()
            {
                Id = Common.Utils.DicUtil.GetDicValue<int>("Id", inputs),
                PersonName = Common.Utils.DicUtil.GetDicValue<string>("PersonName", inputs),
                Photo = Common.Utils.DicUtil.GetDicValue<string>("Photo", inputs),
            };
            //数据校验
            var validateResult = pregnant.Validate();
            if (!validateResult.IsValidated)
            {
                return false;
            }
            //业务逻辑
            if (pregnant.Id > 0)
            {
                var serviceResult = ptService.CreatePregnantInfo(pregnant);
                if (serviceResult.IsSuccess)
                    return serviceResult.Data > 0;
                else
                    return false;
            }
            else
            {
                var serviceResult = ptService.UpdatePregnantInfo(pregnant);
                if (serviceResult.IsSuccess)
                    return serviceResult.Data;
                else
                    return false;
            }
        } 
    }
}
