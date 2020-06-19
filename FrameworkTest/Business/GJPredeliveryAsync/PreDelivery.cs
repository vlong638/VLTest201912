using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.GJPredeliveryAsync
{
    /// <summary>
    /// 待产,FM_DaiChan
    /// </summary>
    [Table("FM_DaiChan")]
    public class PreDelivery
    {
        public long id { get; set; }
        public long chanfu_Id { get; set; }
        public long OperatorId { set; get; }


        public DateTime daichan_date { get; set; }

        public DateTime daichan_time { get; set; }

        public String tai_fangwei { get; set; }

        public String tai_xin { get; set; }

        public String gongsuo_qd { get; set; }

        public String chixu { get; set; }

        public String jiange { get; set; }

        /// <summary>
        /// 先露高低
        /// </summary>
        public String xianlu { get; set; }

        public String gongkou { get; set; }

        public String gongjing { get; set; }

        public String tai_mo { get; set; }

        public String jiancha_Type { get; set; }

        public String xueya1 { get; set; }

        public String xueya2 { get; set; }

        public String maibo { get; set; }

        public String jiancha_ren { get; set; }

        public String yaowu { get; set; }

        public String qita { get; set; }

        public String yangshuixz { get; set; }

        public String tiwen { get; set; }

        public String xueyang { get; set; }

        public PreDeliveryStatus status { get; set; }

        public DateTime create_time { get; set; }

        public DateTime update_time { get; set; }

        /// <summary>
        /// 先露
        /// </summary>
        public FetalPresentationType FetalPresentation { set; get; }

        /// <summary>
        /// 容受
        /// </summary>
        public string Capacity { set; get; }
    }
    /// <summary>
    /// 待产记录 的状态
    /// </summary>
    public enum PreDeliveryStatus
    {
        /// <summary>
        /// 删除的,无效的数据
        /// </summary>
        Deleted = 0,
        /// <summary>
        /// 未删除的,有效的数据
        /// </summary>
        Active = 1,
    }
    /// <summary>
    /// 胎先露类型
    /// </summary>
    public enum FetalPresentationType
    {
        /// <summary>
        /// 未填写
        /// </summary>
        [Description("")]
        None = 0,
        /// <summary>
        /// 头先露
        /// </summary>
        [Description("头先露")]
        头先露 = 1,
        /// <summary>
        /// 臀先露
        /// </summary>
        [Description("臀先露")]
        臀先露 = 2,
        /// <summary>
        /// 肩先露
        /// </summary>
        [Description("肩先露")]
        肩先露 = 3,
        /// <summary>
        /// 不定
        /// </summary>
        [Description("不定")]
        不定 = 4,
        /// <summary>
        /// 足先露
        /// </summary>
        [Description("足先露")]
        足先露 = 5,
    }
}
